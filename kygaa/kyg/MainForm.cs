using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DBP_finalproject_chatting
{
    // MainForm은 TreeView (tvDepartmentEmployees)와 ListView (lvRecentChats)
    public partial class MainForm : Form
    {
        private DBHelper dbHelper = new DBHelper();
        // 로그인에 성공한 사용자 ID는 전역적으로 접근 가능하다고 가정합니다.
        // 실제로는 로그인 폼에서 전달받아 클래스 멤버 변수에 저장해야 합니다.
        private string loggedInUserId; // 예시 ID 사용
        private TcpClient alertClient;
        private NetworkStream alertStream;

        public MainForm()
        {
            InitializeComponent();

            // 폼 로드 시 대화상대 및 대화목록을 로드
            //LoadDepartmentEmployees(); // 1주차 기능
            //LoadRecentChats();         // 2주차 기능 (6-A 검정)
            //ConnectAlertClient();  //3주차 기능 (알람)

            // TreeView 더블클릭 이벤트 연결 (Designer에서 설정 가능)
            this.tvDepartmentEmployees.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tvDepartmentEmployees_NodeMouseDoubleClick);
            // ListView 더블클릭 이벤트 연결 (Designer에서 설정 가능)
            this.lvRecentChats.DoubleClick += new EventHandler(lvRecentChats_DoubleClick);
        }

        //로그인 버튼(임시)
        private void button1_Click(object sender, EventArgs e)
        {
            loggedInUserId = textBox1.Text;
            if (!string.IsNullOrEmpty(loggedInUserId))
            {
                // ID 설정 완료 후 기능 로드 및 서버 연결 시도
                LoadDepartmentEmployees();
                LoadRecentChats();

                // MainForm이 로그인 ID를 가지고 서버에 연결하는 코드
                ConnectAlertClient();
            }
            else
            {
                MessageBox.Show("사용자 ID를 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /*
        private void ConnectAlertClient()
        {
            // 이미 연결되어 있으면 재시도하지 않습니다.
            if (alertClient != null && alertClient.Connected) return;
            try
            {

                // 1. 기존 연결 정리 (혹시 모를 잔여 핸들 제거)
                alertClient?.Close();
                alertClient = new TcpClient();
                // 서버 연결 (ChatForm과는 별개로 MainForm의 알림 기능을 위해 연결)
                alertClient = new TcpClient("127.0.0.1", 8888);
                alertStream = alertClient.GetStream();

                // 서버에 로그인 ID 등록 (ChatForm과 동일하게 로그인 시도)
                string loginMsg = $"LOGIN:{loggedInUserId}:::";
                byte[] loginData = Encoding.UTF8.GetBytes(loginMsg);
                alertStream.Write(loginData, 0, loginData.Length);

                // 메시지 수신용 스레드 시작
                Thread receiveThread = new Thread(ReceiveAlertMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
                Console.WriteLine("MainForm 알림 클라이언트 연결 성공.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MainForm 알림 클라이언트 연결 오류: {ex.Message}");
                MessageBox.Show($"서버 알림 연결 오류: {ex.Message}", "연결 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        */

        private void ConnectAlertClient()
        {
            if (alertClient != null && alertClient.Connected) return;

            try
            {
                // 1. 기존 연결 정리
                alertClient?.Close();
                alertClient = new TcpClient();

                // 2. 새로운 연결 시도
                //alertClient.Connect("127.0.0.1", 8888);
                alertClient.Connect("10.201.21.210", 8888);

                alertStream = alertClient.GetStream();

                // 3. 서버에 로그인 ID 등록
                string loginMsg = $"LOGIN:{loggedInUserId}:::";
                byte[] loginData = Encoding.UTF8.GetBytes(loginMsg);
                alertStream.Write(loginData, 0, loginData.Length);

                // 4. 메시지 수신용 스레드 시작
                Thread receiveThread = new Thread(ReceiveAlertMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
                Console.WriteLine("MainForm 알림 클라이언트 연결 성공.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MainForm 알림 클라이언트 연결 오류: {ex.Message}");
                // 연결 실패 시 재연결 루프가 필요하므로, 이 시점에 바로 Task.Run을 호출합니다.

                Task.Run(() =>
                {
                    Thread.Sleep(3000); // 3초 대기
                    ConnectAlertClient(); // 재귀적으로 재연결 시도
                });
            }
        }


        /*
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
                    string[] parts = received.Split(new char[] { ':' }, 4);

                    if (parts.Length == 4 && parts[0] == "CHAT")
                    {
                        string senderId = parts[1];
                        string content = parts[3];

                        // UI 스레드로 전환하여 알림 및 ListView 갱신
                        this.Invoke((MethodInvoker)delegate
                        {
                            ShowAlertOnMainForm(senderId, content);
                        });
                    }
                }
                catch (System.IO.IOException) { break; }
                catch (Exception) { break; }
            }

            // 연결 종료 후 재연결 시도

            // UI 스레드에서 재연결 함수 호출
            this.Invoke((MethodInvoker)delegate
            {
                if (this.IsDisposed || !this.IsHandleCreated) return;

                // 1. 기존 연결 정리
                alertClient?.Close();
                alertClient = null;

                // 연결이 끊어졌으므로 잠시 후 재연결을 시도합니다.
                // 재연결 로직을 새 스레드에 맡겨 MainForm UI가 멈추지 않도록 합니다.
                new Thread(() =>
                {
                    Thread.Sleep(3000); // 3초 대기 후 재연결 시도
                    ConnectAlertClient();
                }).Start();
            });
        }
        */

        private void ReceiveAlertMessages()
        {
            if (alertClient == null || !alertClient.Connected) return;

            byte[] buffer = new byte[1024];

            while (alertClient.Connected)
            {
                try
                {
                    int bytesRead = alertStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // 서버가 연결을 끊었음을 감지 (연결 종료)

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string[] parts = received.Split(new char[] { ':' }, 5);

                    if (parts.Length >= 4 && parts[0] == "CHAT")
                    {
                        string senderId = parts[1];
                        string content = parts[3];

                        this.Invoke((MethodInvoker)delegate
                        {
                            if (this.IsDisposed || !this.IsHandleCreated) return;
                            ShowAlertOnMainForm(senderId, content);
                        });
                    }
                }
                catch (Exception)
                {
                    break; // 오류 발생 시 루프 종료
                }
            }

            // 🚨 연결 종료 후 복구 로직 (UI 스레드에서 Task 시작) 🚨
            this.Invoke((MethodInvoker)delegate
            {
                if (this.IsDisposed || !this.IsHandleCreated) return;

                alertClient?.Close();
                alertClient = null;

                // Task를 시작하여 메인 스레드를 블로킹하지 않고 재연결 시도
                Task.Run(() =>
                {
                    Thread.Sleep(3000); // 3초 대기 후 재연결 시도
                    ConnectAlertClient();
                });

                Console.WriteLine("MainForm 알림 클라이언트 재연결을 비동기로 예약했습니다.");
            });
        }



        private void ShowAlertOnMainForm(string senderId, string content)
        {
            // 1. 대화목록을 갱신하여 최신 메시지가 위로 오게 함
            LoadRecentChats();

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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 폼이 닫힐 때 알림 클라이언트 연결도 해제
                alertClient?.Close();
            }
            catch { }
            
        }


        // 1. 대화상대 목록 로드 (1주차 기능)
        public void LoadDepartmentEmployees()
        {
            tvDepartmentEmployees.Nodes.Clear();

            // 부서 및 직원 정보 조회 쿼리 (1주차에 제시된 쿼리 사용)
            string query = @"
            SELECT 
                D.DeptID, D.DeptName, 
                U.UserID, U.UserName, U.Nickname 
            FROM Department D 
            LEFT JOIN User U ON D.DeptID = U.DeptID 
            ORDER BY D.DeptName, U.UserName";

            DataTable dt = dbHelper.ExecuteSelect(query);

            TreeNode companyNode = new TreeNode("우리 회사") { Tag = "Company" };
            tvDepartmentEmployees.Nodes.Add(companyNode);

            string currentDeptName = string.Empty;
            TreeNode currentDeptNode = null;

            foreach (DataRow row in dt.Rows)
            {
                string deptName = row["DeptName"].ToString();
                string deptId = row["DeptID"].ToString();

                if (deptName != currentDeptName)
                {
                    currentDeptName = deptName;
                    currentDeptNode = new TreeNode(deptName) { Tag = $"Dept|{deptId}" };
                    companyNode.Nodes.Add(currentDeptNode);
                }

                if (row["UserID"] != DBNull.Value)
                {
                    string userId = row["UserID"].ToString();
                    string userName = row["UserName"].ToString();
                    string nickname = row["Nickname"].ToString();

                    TreeNode employeeNode = new TreeNode($"{userName} ({nickname})")
                    {
                        Tag = $"User|{userId}", // Tag에 사용자 ID 저장
                    };
                    currentDeptNode.Nodes.Add(employeeNode);
                }
            }
            companyNode.Expand();
        }

        // 2. TreeView 직원 더블클릭 시 채팅창 열기 (2주차 기능)
        private void tvDepartmentEmployees_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Tag가 "User|"로 시작하는지 확인 (직원 노드인지 확인)
            if (e.Node.Tag is string tag && tag.StartsWith("User|"))
            {
                string partnerId = tag.Split('|')[1];
                string partnerName = e.Node.Text;

                OpenChatForm(partnerId, partnerName);
            }
        }

        // 3. 대화목록 로드 (2주차 6-A 검정)
        public void LoadRecentChats()
        {
            lvRecentChats.Items.Clear();

            string query = @"
            SELECT 
                R.PartnerID,
                U.UserName,
                R.LastMessageTime
            FROM RecentChat R
            JOIN User U ON R.PartnerID = U.UserID
            WHERE R.UserID = @myId
            ORDER BY R.LastMessageTime DESC";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
            new MySqlParameter("@myId", loggedInUserId)
            };

            DataTable dt = dbHelper.ExecuteSelect(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                string partnerId = row["PartnerID"].ToString();
                string partnerName = row["UserName"].ToString();
                // 시간 형식을 원하는 대로 포맷팅할 수 있습니다.
                string lastTime = row["LastMessageTime"].ToString();

                ListViewItem item = new ListViewItem(partnerId);
                item.SubItems.Add(partnerName);
                item.SubItems.Add(lastTime);
                item.Tag = partnerId; // Tag에 PartnerID 저장

                lvRecentChats.Items.Add(item);
            }
        }

        // 4. ListView 대화목록 더블클릭 시 채팅창 열기 (2주차 기능)
        private void lvRecentChats_DoubleClick(object sender, EventArgs e)
        {
            if (lvRecentChats.SelectedItems.Count > 0)
            {
                // 선택된 항목의 Tag에서 PartnerID 가져오기
                string partnerId = lvRecentChats.SelectedItems[0].Tag.ToString();
                string partnerName = lvRecentChats.SelectedItems[0].SubItems[1].Text;

                OpenChatForm(partnerId, partnerName);
            }
        }

        // 5. 채팅창 열기 공통 메서드
        private void OpenChatForm(string partnerId, string partnerName)
        {
            // 새 ChatForm 인스턴스를 생성하고 ID 정보 전달
            ChatForm chatForm = new ChatForm(this.loggedInUserId, partnerId, partnerName);
            chatForm.Show();
        }

        // 기타: MyPage, 관리자 버튼 등 다른 팀원과 연계되는 기능
    }
}
