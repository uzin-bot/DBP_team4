using DBP_Chat; // Dept
using DBP_WinformChat;
using kyg;
using leehaeun;
using MySqlConnector;
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
        private System.Windows.Forms.Timer refreshTimer;

        // ✅ 추가: 마지막 메시지 보낸 사람 ID
        private int lastMessageSenderId = 0;

        public chatlist()
        {
            InitializeComponent();

            btndept.Click += btndept_Click; //클릭시 DeptForm으로 이동 

            // Owner Draw 이벤트 등록
            lvlist.DrawColumnHeader += LvList_DrawColumnHeader;
            lvlist.DrawSubItem += LvList_DrawSubItem;

            // ✅ NotifyIcon 초기화 먼저!
            niChatAlert = new NotifyIcon();
            niChatAlert.Icon = SystemIcons.Information;
            niChatAlert.Visible = true;
            niChatAlert.Text = "채팅 알림";

            // ✅ BalloonTip 클릭 이벤트 (풍선 알림 클릭)
            niChatAlert.BalloonTipClicked += NiChatAlert_BalloonTipClicked;


            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 3000;
            refreshTimer.Tick += (s, e) => LoadRecentChat();

            LoadRecentChat();
            this.Activated += chatlist_Activated;

        }

        private void chatlist_Activated(object sender, EventArgs e)
        {
            LoadRecentChat();
        }

        private void chatlist_Load(object sender, EventArgs e)
        {
            Console.WriteLine($"[chatlist] ==================== chatlist_Load 시작 ====================");
            Console.WriteLine($"[chatlist] currentUserId = {currentUserId}");

            LoadRecentChat();

            Console.WriteLine($"[chatlist] ConnectAlertClient 호출 전");

            //알림 클라이언트 연결
            ConnectAlertClient();

            Console.WriteLine($"[chatlist] chatlist_Load 완료");
        }
        

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            refreshTimer.Stop(); // ← 이거 있어야 한 번만!
            LoadRecentChat();
        }

        // ===== 알림 기능 추가 =====

        // 서버에 알림용 연결 생성
        private void ConnectAlertClient()
        {
            if (alertClient != null && alertClient.Connected)
            {
                Console.WriteLine($"[chatlist] 이미 연결되어 있음");
                return;
            }

            try
            {
                Console.WriteLine($"[chatlist] 서버 연결 시도 중...");

                // 1. 기존 연결 정리
                alertClient?.Close();
                alertClient = new TcpClient();

                // 2. 새로운 연결 시도
                //alertClient.Connect("127.0.0.1", 8888);
                alertClient.Connect("51.21.27.234", 12345);
                //alertClient.Connect("10.201.21.210", 8888);
                Console.WriteLine($"[chatlist] 서버 연결 성공!");
                alertStream = alertClient.GetStream();

                // 3. 서버에 로그인 ID 등록
                string loginMsg = $"LOGIN:{currentUserId}:::";
                byte[] loginData = Encoding.UTF8.GetBytes(loginMsg);
                alertStream.Write(loginData, 0, loginData.Length);
                Console.WriteLine($"[chatlist] LOGIN 전송: {currentUserId}");

                // 4. 메시지 수신용 스레드 시작
                Thread receiveThread = new Thread(ReceiveAlertMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
                Console.WriteLine($"[chatlist] ReceiveAlertMessages 스레드 시작");
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
        // ChatList.cs 파일의 ReceiveAlertMessages() 함수
        private void ReceiveAlertMessages()
        {
            if (alertClient == null || !alertClient.Connected) return;

            byte[] buffer = new byte[4096];
            StringBuilder messageBuilder = new StringBuilder();

            while (alertClient != null && alertClient.Connected)
            {
                try
                {
                    int bytesRead = alertStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    messageBuilder.Append(received);
                    string fullMessage = messageBuilder.ToString();

                    string[] messages = fullMessage.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

                    bool lastMessageComplete = fullMessage.EndsWith("\0");
                    int messagesToProcess = lastMessageComplete ? messages.Length : messages.Length - 1;

                    for (int i = 0; i < messagesToProcess; i++)
                    {
                        string msg = messages[i].Trim();
                        if (string.IsNullOrWhiteSpace(msg)) continue;

                        // 5개로 Split해야 CHAT:senderId:receiverId:content를 올바르게 분리 가능
                        string[] parts = msg.Split(new char[] { ':' }, 5);

                        // CHAT 메시지 형식 확인 및 처리
                        if (parts.Length >= 4 && parts[0] == "CHAT")
                        {
                            string senderId = parts[1];
                            string receiverId = parts[2];
                            string content = parts[3];

                            if (receiverId == currentUserId.ToString())
                            {

                                // ✅ 마지막 메시지 보낸 사람 저장
                                lastMessageSenderId = int.Parse(senderId);

                                // 💡 수정된 부분: 3초 대기 후 UI 업데이트를 요청하는 Task 생성
                                Task.Delay(1000).ContinueWith(_ =>
                                {
                                    try
                                    {

                                        
                                        // UI 스레드에 업데이트 요청
                                        this.BeginInvoke((MethodInvoker)delegate
                                        {
                                            if (this.IsDisposed) return;

                                            //System.Threading.Thread.Sleep(1000);

                                            // 1. 새로고침 (지연 후 실행)
                                            LoadRecentChat();

                                            // 2. 깜빡임 및 알람 (Thread.Sleep 없이 실행)
                                            try
                                            {
                                                FlashWindow.Flash(this);
                                            }
                                            catch { }

                                            try
                                            {
                                                if (this.WindowState == FormWindowState.Minimized || !this.ContainsFocus)
                                                {
                                                    niChatAlert.BalloonTipTitle = $"새 메시지: {senderId}";
                                                    niChatAlert.BalloonTipText = content.Length > 50 ? content.Substring(0, 50) + "..." : content;
                                                    niChatAlert.ShowBalloonTip(5000);
                                                }
                                            }
                                            catch { }
                                        });
                                    }
                                    catch { }
                                });
                            }
                        }
                    }

                    // 메시지 버퍼링 처리 (기존 로직 유지)
                    if (!lastMessageComplete && messages.Length > 0)
                    {
                        messageBuilder.Clear();
                        messageBuilder.Append(messages[messages.Length - 1]);
                    }
                    else
                    {
                        messageBuilder.Clear();
                    }
                }
                catch (System.IO.IOException)
                {
                    break;
                }
                catch { }
            }

            // 재연결 (기존 로직 유지)
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                try
                {
                    if (!this.IsDisposed)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            alertClient?.Close();
                            alertClient = null;
                            ConnectAlertClient();
                        });
                    }
                }
                catch { }
            });
        }


        // 새메세지 도착 알림
        private void ShowAlertOnMainForm(string senderId, string content)
        {
            try
            {
                // 1. 대화목록 갱신
                LoadRecentChat();

                // 2. 작업 표시줄 깜빡임
                try
                {
                    FlashWindow.Flash(this);
                }
                catch { }

                // 3. NotifyIcon 풍선 알림
                try
                {
                    if (this.WindowState == FormWindowState.Minimized || !this.ContainsFocus)
                    {
                        niChatAlert.BalloonTipTitle = $"새 메시지: {senderId}";
                        niChatAlert.BalloonTipText = content.Length > 50 ? content.Substring(0, 50) + "..." : content;
                        niChatAlert.ShowBalloonTip(5000);
                    }
                }
                catch { }
            }
            catch { }

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

            // 닉네임 관련 쿼리 수정
            string sql = $@"
                SELECT 
                    rc.PartnerUserId,
                    u.Name,
                    u.LoginId,
                    p.Nickname,
                    d.DeptName,
                    cm.Content AS LastMessage,       
                    rc.LastMessageAt,
                    rc.is_pinned,
                    rc.UnreadCount
                FROM RecentChat rc
                JOIN User u ON rc.PartnerUserId = u.UserId
                JOIN Department d ON u.DeptId = d.DeptId
                JOIN Profile p ON u.UserId = p.UserId AND p.IsDefault = 1  
                JOIN ChatMessage cm ON rc.LastMessageId = cm.MessageId 
                WHERE rc.UserId = {currentUserId}
                ORDER BY rc.is_pinned DESC, rc.LastMessageAt DESC";

            try
            {
                DataTable dt = DBconnector.GetInstance().Query(sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    bool isPinned = Convert.ToInt32(row["is_pinned"]) == 1;
                    int unreadCount = Convert.ToInt32(row["UnreadCount"]);

                    // 첫 번째 컬럼: 안 읽은 메시지 있으면 ●, 없으면 공백
                    string indicator = unreadCount > 0 ? "●" : "";
                    ListViewItem item = new ListViewItem(indicator);


                    item.ImageIndex = isPinned ? 0 : -1;
                     
                    item.SubItems.Add(row["LoginId"].ToString()); // 로그인 아이디로 수정
                    item.SubItems.Add(row["Name"].ToString());
                    item.SubItems.Add(row["DeptName"].ToString());

                    //최근 메시지 길면 ...으로 잘림 (20제한 >> UI 변경시 늘리거나 해도 O)
                    string msg = row["LastMessage"].ToString();
                    if (msg.Length > 20)
                        msg = msg.Substring(0, 20) + "…";
                    item.SubItems.Add(msg);

                    item.SubItems.Add(row["LastMessageAt"].ToString());


                    // Tag에 실제 UserId 저장 (더블클릭 시 사용)
                    item.Tag = row["PartnerUserId"].ToString();

                    lvlist.Items.Add(item);
                }
            }
            catch (Exception ex)
            {

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

            // 수정
            int targetUserId = Convert.ToInt32(lvlist.SelectedItems[0].Tag);

            // 채팅창 열기 전에 UnreadCount = 0으로
            string updateQuery = $@"
                UPDATE RecentChat
                SET UnreadCount = 0
                WHERE UserId = {currentUserId}
                AND PartnerUserId = {targetUserId}";

            DBconnector.GetInstance().NonQuery(updateQuery);

            new ChatForm(currentUserId, targetUserId).Show();

            LoadRecentChat();
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

            // 수정
            int partnerUserId = Convert.ToInt32(lvlist.SelectedItems[0].Tag);

            PinChat(partnerUserId);
			LoadRecentChat();
		}

		//우클릭 메뉴 → 고정 해제
		private void deletepin_Click(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

            // 수정
            int partnerUserId = Convert.ToInt32(lvlist.SelectedItems[0].Tag);
            UnpinChat(partnerUserId);
			LoadRecentChat();
		}

		//btndept → 친구 목록(DeptForm)으로 이동
		private void btndept_Click(object sender, EventArgs e)
		{
            
			Dept deptForm = new Dept(currentUserId, currentUserName, currentUserNickname);
            deptForm.Show(); // ✅ 먼저 Dept 열고
            
        }

        // 컬럼 헤더는 기본 방식으로
        private void LvList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        // 각 셀 그리기
        private void LvList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // 첫 번째 컬럼(동그라미)만 특별 처리
            if (e.ColumnIndex == 0)
            {
                // 배경 그리기
                e.DrawBackground();

                string text = e.SubItem.Text;

                if (!string.IsNullOrEmpty(text)) // "●" 있을 때만
                {
                    // 빨간색으로 동그라미 그리기
                    using (Brush redBrush = new SolidBrush(Color.Red))
                    {
                        StringFormat sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };

                        using (Font boldFont = new Font(e.Item.Font.FontFamily, 8, FontStyle.Bold))
                        {
                            e.Graphics.DrawString(text, boldFont, redBrush, e.Bounds, sf);
                        }
                    }
                }

                e.DrawFocusRectangle(e.Bounds);
            }
            else
            {
                // 나머지 컬럼은 기본 방식으로
                e.DrawDefault = true;
            }
        }

        // ✅ 풍선 알림 클릭 시 채팅창 열기
        private void NiChatAlert_BalloonTipClicked(object sender, EventArgs e)
        {

             new ChatForm(currentUserId, lastMessageSenderId).Show();
             this.Show(); // chatlist도 보여주기
            
        }
    }
}
