using DBP_Chat; // Dept
using DBP_WinformChat;
using kyg;
using leehaeun;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using System.Net.Sockets;      
using System.Text;              
using System.Threading;         
using System.Threading.Tasks;   

namespace 남예솔
{
	public partial class chatlist : Form
	{
		//현재 로그인한 사용자 정보(UserInfo에서 가져옴)
		//User.Rows[0] -> User
		private int currentUserId = Convert.ToInt32(UserInfo.User["UserId"]);
		private string currentUserName = UserInfo.User["Name"].ToString();
		private string currentUserNickname = UserInfo.Profile.Rows[0]["Nickname"].ToString();

        // 알람용 TCP 클라이언트
        private TcpClient alertClient;
        private NetworkStream alertStream;

        private NotifyIcon niChatAlert;

        public chatlist()
        {
            InitializeComponent();

            btndept.Click += btndept_Click; //클릭시 DeptForm으로 이동 

            // NotifyIcon 초기화 추가 (수정사항)
            niChatAlert = new NotifyIcon();
            niChatAlert.Icon = SystemIcons.Information; // 기본 정보 아이콘
            niChatAlert.Visible = true;
            niChatAlert.Text = "채팅 알림";

        }

        private void chatlist_Load(object sender, EventArgs e)
        {
            LoadRecentChat();

            //알림 클라이언트 연결
            ConnectAlertClient();
        }


        // ===== 알림 기능 추가 =====

        // 서버에 알림용 연결 생성
        private void ConnectAlertClient()
        {
            if (alertClient != null && alertClient.Connected) return;

            try
            {
                // 1. 기존 연결 정리
                alertClient?.Close();
                alertClient = new TcpClient();

                // 2. 새로운 연결 시도
                alertClient.Connect("127.0.0.1", 8888);
                //alertClient.Connect("10.201.21.210", 8888);

                alertStream = alertClient.GetStream();

                // 3. 서버에 로그인 ID 등록
                string loginMsg = $"LOGIN:{currentUserId}:::";
                byte[] loginData = Encoding.UTF8.GetBytes(loginMsg);
                alertStream.Write(loginData, 0, loginData.Length);

                // 4. 메시지 수신용 스레드 시작
                Thread receiveThread = new Thread(ReceiveAlertMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                Console.WriteLine("[chatlist] 알림 클라이언트 연결 성공.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[chatlist] 알림 클라이언트 연결 오류: {ex.Message}");

                // 연결 실패 시 재연결 시도 (3초 후)
                Task.Run(() =>
                {
                    Thread.Sleep(3000);
                    if (!this.IsDisposed)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            ConnectAlertClient(); // 재귀적으로 재연결 시도
                        });
                    }
                });
            }
        }


        // 서버로 부터 알림 메세지 수신
        private void ReceiveAlertMessages()
        {
            if (alertClient == null || !alertClient.Connected) return;

            byte[] buffer = new byte[1024];

            while (alertClient.Connected)
            {
                try
                {
                    int bytesRead = alertStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // 연결 종료

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string[] parts = received.Split(new char[] { ':' }, 5);

                    if (parts.Length >= 4 && parts[0] == "CHAT")
                    {
                        string senderId = parts[1];
                        string receiverId = parts[2];
                        string content = parts[3];

                        // 나에게 온 메시지인 경우에만 처리
                        if (receiverId == currentUserId.ToString())
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                if (this.IsDisposed || !this.IsHandleCreated) return;
                                ShowAlertOnMainForm(senderId, content);
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    break; // 오류 발생 시 루프 종료
                }
            }

            // 연결 종료 후 복구 로직
            this.Invoke((MethodInvoker)delegate
            {
                if (this.IsDisposed || !this.IsHandleCreated) return;

                alertClient?.Close();
                alertClient = null;

                // Task를 시작하여 메인 스레드를 블로킹하지 않고 재연결 시도
                Task.Run(() =>
                {
                    Thread.Sleep(3000);
                    if (!this.IsDisposed)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            ConnectAlertClient();
                        });
                    }
                });

                Console.WriteLine("[chatlist] 알림 클라이언트 재연결 예약.");
            });
        }

        // 새메세지 도착 알림
        private void ShowAlertOnMainForm(string senderId, string content)
        {
            // 1. 대화목록을 갱신하여 최신 메시지가 위로 오게 함
            LoadRecentChat();

            // 2. 작업 표시줄 깜빡임
            FlashWindow.Flash(this); // FlashWindow 헬퍼 클래스가 필요함

            // 3. NotifyIcon 풍선 알림 (niChatAlert 컨트롤이 필요함)
            if (this.WindowState == FormWindowState.Minimized || !this.ContainsFocus)
            {
                niChatAlert.BalloonTipTitle = $"새 메시지: {senderId}";
                niChatAlert.BalloonTipText = content.Length > 50 ? content.Substring(0, 50) + "..." : content;
                niChatAlert.ShowBalloonTip(5000); // 5초 유지
            }

            // 4. ListView 항목 강조 (Optional)
            /*
            foreach (ListViewItem item in lvRecentChats.Items)
            {
                // senderId가 이 항목의 PartnerID라면 
                if (item.Tag != null && item.Tag.ToString() == senderId)
                {
                    item.BackColor = System.Drawing.Color.LightYellow;
                    break;
                }
            }
            */
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 폼이 닫힐 때 알림 클라이언트 연결도 해제
                alertClient?.Close();
            }
            catch { }

        }


        // ===== 챗리스트 기능 ======

        //RecentChat + 고정정렬
        private void LoadRecentChat()
        {
            lvlist.Items.Clear();

            string sql = $@"
                SELECT 
                    rc.PartnerUserId,
                    u.Name,
                    u.Nickname,
                    d.DeptName,
                    cm.Content AS LastMessage,       
                    rc.LastMessageAt,
                    rc.is_pinned
                FROM RecentChat rc
                JOIN User u ON rc.PartnerUserId = u.UserId
                JOIN Department d ON u.DeptId = d.DeptId
            JOIN ChatMessage cm ON rc.LastMessageId = cm.MessageId 
                WHERE rc.UserId = {currentUserId}
                ORDER BY rc.is_pinned DESC, rc.LastMessageAt DESC";

            DataTable dt = DBconnector.GetInstance().Query(sql);


			foreach (DataRow row in dt.Rows)
			{
				bool isPinned = Convert.ToInt32(row["is_pinned"]) == 1;

				ListViewItem item = new ListViewItem();
				item.ImageIndex = isPinned ? 0 : -1;

				item.SubItems.Add(row["PartnerUserId"].ToString());
				item.SubItems.Add(row["Name"].ToString());
				item.SubItems.Add(row["DeptName"].ToString());

				//최근 메시지 길면 ...으로 잘림 (20제한 >> UI 변경시 늘리거나 해도 O)
				string msg = row["LastMessage"].ToString();
				if (msg.Length > 20)
					msg = msg.Substring(0, 20) + "…";
				item.SubItems.Add(msg);

				item.SubItems.Add(row["LastMessageAt"].ToString());

				lvlist.Items.Add(item);
			}
		}

		//우클릭 자동 선택
		private void lvlist_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ListViewItem item = lvlist.GetItemAt(e.X, e.Y);
				if (item != null)
					item.Selected = true;
			}
		}

		//더블클릭 → 채팅창 열기
		private void lvlist_DoubleClick(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

			int targetUserId = Convert.ToInt32(lvlist.SelectedItems[0].SubItems[1].Text);

			new ChatForm(currentUserId, targetUserId).Show();
		}

		//고정하기
		private void PinChat(int partnerUserId)
		{
			string sql = $@"
                UPDATE RecentChat 
                SET is_pinned = 1
                WHERE UserId = {currentUserId} AND PartnerUserId = {partnerUserId}";

			DBconnector.GetInstance().NonQuery(sql);
		}

		//고정 해제
		private void UnpinChat(int partnerUserId)
		{
			string sql = $@"
                UPDATE RecentChat 
                SET is_pinned = 0 
                WHERE UserId = {currentUserId} AND PartnerUserId = {partnerUserId}";

			DBconnector.GetInstance().NonQuery(sql);
		}

		//우클릭 메뉴 → 고정하기
		private void addpin_Click(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

			int partnerUserId = Convert.ToInt32(lvlist.SelectedItems[0].SubItems[1].Text);
			PinChat(partnerUserId);
			LoadRecentChat();
		}

		//우클릭 메뉴 → 고정 해제
		private void deletepin_Click(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

			int partnerUserId = Convert.ToInt32(lvlist.SelectedItems[0].SubItems[1].Text);
			UnpinChat(partnerUserId);
			LoadRecentChat();
		}

		//btndept → 친구 목록(DeptForm)으로 이동
		private void btndept_Click(object sender, EventArgs e)
		{
			Dept deptForm = new Dept(currentUserId, currentUserName, currentUserNickname);
			deptForm.Show();
		}
	}
}
