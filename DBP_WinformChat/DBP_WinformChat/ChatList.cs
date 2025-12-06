using DBP_Chat;
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
        private int currentUserId = Convert.ToInt32(UserInfo.User["UserId"]);
        private string currentUserName = UserInfo.User["Name"].ToString();
        private string currentUserNickname = UserInfo.Profile.Rows[0]["Nickname"].ToString();

        private TcpClient alertClient;
        private NetworkStream alertStream;
        private NotifyIcon niChatAlert;

        private PermissionManager permissionManager; // 추가

        public chatlist()
        {
            InitializeComponent();

            btndept.Click += btndept_Click;
            permissionManager = new PermissionManager(); // 추가

            niChatAlert = new NotifyIcon();
            niChatAlert.Icon = SystemIcons.Information;
            niChatAlert.Visible = true;
            niChatAlert.Text = "채팅 알림";
        }

        private void chatlist_Load(object sender, EventArgs e)
        {
            LoadRecentChat();
            ConnectAlertClient();
        }

        // ===== 알림 기능 =====
        private void ConnectAlertClient()
        {
            if (alertClient != null && alertClient.Connected) return;

            try
            {
                alertClient?.Close();
                alertClient = new TcpClient();
                alertClient.Connect("127.0.0.1", 8888);

                alertStream = alertClient.GetStream();

                string loginMsg = $"LOGIN:{currentUserId}:::";
                byte[] loginData = Encoding.UTF8.GetBytes(loginMsg);
                alertStream.Write(loginData, 0, loginData.Length);

                Thread receiveThread = new Thread(ReceiveAlertMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                Console.WriteLine("[chatlist] 알림 클라이언트 연결 성공.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[chatlist] 알림 클라이언트 연결 오류: {ex.Message}");

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
            }
        }

        private void ReceiveAlertMessages()
        {
            if (alertClient == null || !alertClient.Connected) return;

            byte[] buffer = new byte[1024];

            while (alertClient.Connected)
            {
                try
                {
                    int bytesRead = alertStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string[] parts = received.Split(new char[] { ':' }, 5);

                    if (parts.Length >= 4 && parts[0] == "CHAT")
                    {
                        string senderId = parts[1];
                        string receiverId = parts[2];
                        string content = parts[3];

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
                    break;
                }
            }

            this.Invoke((MethodInvoker)delegate
            {
                if (this.IsDisposed || !this.IsHandleCreated) return;

                alertClient?.Close();
                alertClient = null;

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

        private void ShowAlertOnMainForm(string senderId, string content)
        {
            LoadRecentChat();
            FlashWindow.Flash(this);

            if (this.WindowState == FormWindowState.Minimized || !this.ContainsFocus)
            {
                niChatAlert.BalloonTipTitle = $"새 메시지: {senderId}";
                niChatAlert.BalloonTipText = content.Length > 50 ? content.Substring(0, 50) + "..." : content;
                niChatAlert.ShowBalloonTip(5000);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                alertClient?.Close();
            }
            catch { }
        }

        // ===== ★ 수정: 권한 기반 챗리스트 =====
        private void LoadRecentChat()
        {
            lvlist.Items.Clear();

            // 권한 있는 사용자만 필터링
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
                AND (
                    rc.PartnerUserId IN (
                        SELECT VisibleUserId FROM UserVisibleUser WHERE OwnerUserId = {currentUserId}
                    )
                    OR u.DeptId IN (
                        SELECT DeptId FROM UserVisibleDept WHERE OwnerUserId = {currentUserId}
                    )
                )
                ORDER BY rc.is_pinned DESC, rc.LastMessageAt DESC";

            DataTable dt = DBconnector.GetInstance().Query(sql);

            foreach (DataRow row in dt.Rows)
            {
                int partnerUserId = Convert.ToInt32(row["PartnerUserId"]);
                bool isPinned = Convert.ToInt32(row["is_pinned"]) == 1;
                bool isBlocked = !permissionManager.CanChat(currentUserId, partnerUserId);

                ListViewItem item = new ListViewItem();
                item.ImageIndex = isPinned ? 0 : -1;

                item.SubItems.Add(row["PartnerUserId"].ToString());

                // 차단된 사용자 표시
                string name = row["Name"].ToString();
                if (isBlocked) name += " 🚫";
                item.SubItems.Add(name);

                item.SubItems.Add(row["DeptName"].ToString());

                string msg = row["LastMessage"].ToString();
                if (msg.Length > 20)
                    msg = msg.Substring(0, 20) + "…";
                item.SubItems.Add(msg);

                item.SubItems.Add(row["LastMessageAt"].ToString());

                // 차단된 사용자는 회색 처리
                if (isBlocked)
                    item.ForeColor = System.Drawing.Color.Gray;

                lvlist.Items.Add(item);
            }
        }

        private void lvlist_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewItem item = lvlist.GetItemAt(e.X, e.Y);
                if (item != null)
                    item.Selected = true;
            }
        }

        // ★ 수정: 더블클릭 시 권한 체크
        private void lvlist_DoubleClick(object sender, EventArgs e)
        {
            if (lvlist.SelectedItems.Count == 0) return;

            int targetUserId = Convert.ToInt32(lvlist.SelectedItems[0].SubItems[1].Text);

            // 권한 체크 추가
            var result = permissionManager.CanSendMessage(currentUserId, targetUserId);
            if (!result.CanSend)
            {
                MessageBox.Show(result.Reason, "채팅 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            new ChatForm(currentUserId, targetUserId).Show();
        }

        private void PinChat(int partnerUserId)
        {
            string sql = $@"
                UPDATE RecentChat 
                SET is_pinned = 1
                WHERE UserId = {currentUserId} AND PartnerUserId = {partnerUserId}";

            DBconnector.GetInstance().NonQuery(sql);
        }

        private void UnpinChat(int partnerUserId)
        {
            string sql = $@"
                UPDATE RecentChat 
                SET is_pinned = 0 
                WHERE UserId = {currentUserId} AND PartnerUserId = {partnerUserId}";

            DBconnector.GetInstance().NonQuery(sql);
        }

        private void addpin_Click(object sender, EventArgs e)
        {
            if (lvlist.SelectedItems.Count == 0) return;

            int partnerUserId = Convert.ToInt32(lvlist.SelectedItems[0].SubItems[1].Text);
            PinChat(partnerUserId);
            LoadRecentChat();
        }

        private void deletepin_Click(object sender, EventArgs e)
        {
            if (lvlist.SelectedItems.Count == 0) return;

            int partnerUserId = Convert.ToInt32(lvlist.SelectedItems[0].SubItems[1].Text);
            UnpinChat(partnerUserId);
            LoadRecentChat();
        }

        private void btndept_Click(object sender, EventArgs e)
        {
            Dept deptForm = new Dept(LoginForm.LoginId, currentUserName, currentUserNickname);
            deptForm.Show();
        }
    }
}