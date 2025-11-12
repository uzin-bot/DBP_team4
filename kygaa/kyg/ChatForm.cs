using System;
using System.Net.Sockets;
using System.Text;
using System.Data;
using System.Threading; // Thread 사용을 위해 추가
using System.Windows.Forms;

// ChatForm은 RichTextBox: rtbChatLog, TextBox: txtInput, Button: btnSend 

namespace DBP_finalproject_chatting
{

    public partial class ChatForm : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private string myId;
        private string partnerId;
        private DBHelper dbHelper = new DBHelper(); // DB 작업을 위해 추가

        // 이 부분은 Main Form에서 로그인된 사용자 ID와 선택된 상대 ID를 넘겨받아야 합니다.
        public ChatForm(string myId, string partnerId, string partnerName)
        {
            InitializeComponent();
            this.myId = myId;
            this.partnerId = partnerId;
            // lblPartnerName.Text = partnerName; // 상대방 이름 표시 (디자인에 따라 다름)
            this.Text = $"{myId} 님과 {partnerName} 님의 채팅";

            // 3주차 5-C 검정: 대화 기록 로드 (서버 연결 전에 실행)
            LoadChatHistory();

            // 2주차 5-A 검정: 서버 연결 (실시간 메시지 수신 준비)
            ConnectToServer();
        }

        // 3주차 5-C 검정: 대화 기록 로드 및 출력 메서드
        private void LoadChatHistory()
        {
            try
            {
                // DBHelper를 사용하여 두 사용자 간의 메시지 기록을 가져옴
                DataTable history = dbHelper.GetChatHistory(myId, partnerId);

                foreach (DataRow row in history.Rows)
                {
                    string senderId = row["SenderID"].ToString();
                    string content = row["Content"].ToString();

                    // 메시지 출력
                    if (senderId == myId)
                    {
                        // 내가 보낸 메시지
                        DisplayMessage($"[나]: {content}\n", true);
                    }
                    else
                    {
                        // 상대방이 보낸 메시지
                        DisplayMessage($"[{senderId}]: {content}\n", false);
                    }
                }
            }
            catch (Exception ex)
            {
                // DB 연결 또는 쿼리 오류 시 경고
                MessageBox.Show("대화 기록을 불러오는 중 오류 발생: " + ex.Message, "DB 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConnectToServer()
        {
            try
            {
                // O 로그인 사용자 ID + 선택 상대 ID로 서버와 통신
                client = new TcpClient("127.0.0.1", 8888); // 서버 IP 및 포트
                stream = client.GetStream();

                // 1. 서버에 로그인 ID 등록 메시지 전송
                string loginMsg = $"LOGIN:{myId}::";
                byte[] loginData = Encoding.UTF8.GetBytes(loginMsg);
                stream.Write(loginData, 0, loginData.Length);

                // 2. 메시지 수신용 스레드 시작
                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                rtbChatLog.AppendText(">> 서버에 연결되었습니다.\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버 연결 오류: " + ex.Message, "연결 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string content = txtInput.Text;
            if (string.IsNullOrWhiteSpace(content)) return;
            if (client == null || !client.Connected)
            {
                MessageBox.Show("서버에 연결되지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. 서버로 메시지 전송 (5-A 검정)
            // 포맷: [TYPE]:[SENDER_ID]:[RECEIVER_ID]:[MESSAGE_CONTENT]
            string chatMsg = $"CHAT:{myId}:{partnerId}:{content}";
            byte[] data = Encoding.UTF8.GetBytes(chatMsg);
            stream.Write(data, 0, data.Length);

            //스트림 버퍼를 강제로 비워 즉시 전송 보장 (추가) 
            //stream.Flush();

            // 2. 내 채팅창에 메시지 표시
            DisplayMessage($"[나]: {content}\n", true);

            // 3. ChatMessage 테이블에 INSERT (DB 저장 로직)
            /*
            try
            {
                dbHelper.InsertChatMessage(myId, partnerId, content);
            }
            catch (Exception dbEx)
            {
                // DB 저장 실패는 통신과 별개로 처리 (로깅 필요)
                Console.WriteLine("채팅 메시지 DB 저장 오류: " + dbEx.Message);
            }
            */

            txtInput.Clear();
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            while (client.Connected)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // 서버 연결 종료

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // 수신된 메시지 파싱
                    string[] parts = received.Split(new char[] { ':' }, 4);
                    if (parts[0] == "CHAT" && parts.Length == 4)
                    {
                        string senderId = parts[1];
                        string content = parts[3];

                        // UI 스레드에서 메시지 표시 (Invoke 필요)
                        this.Invoke((MethodInvoker)delegate
                        {
                            // 5-A 검정: 1:1 대화 수신 기능 완성
                            DisplayMessage($"[{senderId}]: {content}\n", false);

                            // 3주차 5-B 검정: 대화 도착 알림 기능
                            // Form이 활성화되지 않았거나 최소화 상태일 때 알림
                            if (this.WindowState == FormWindowState.Minimized || !this.ContainsFocus)
                            {
                                // NotifyIcon을 이용한 풍선 도움말 알림 (5초 유지)
                                // niChatAlert 컨트롤이 ChatForm에 추가되어 있다고 가정
                                niChatAlert.BalloonTipTitle = $"새 메시지 도착: {senderId}";
                                niChatAlert.BalloonTipText = content.Length > 50 ? content.Substring(0, 50) + "..." : content;

                                // 5000ms (5초) 동안 알림 표시
                                niChatAlert.ShowBalloonTip(5000);

                                // 작업표시줄 깜빡임 기능 실행 (FlashWindow 헬퍼 클래스 필요)
                                FlashWindow.Flash(this);
                            }
                        });
                    }
                }



                catch (System.IO.IOException) // 연결 끊김 또는 스트림 오류
                {
                    break;
                }
                catch (Exception)
                {
                    // 기타 예외 처리
                }
            }
            
            // 연결 종료 처리 (UI 스레드에서 처리
            this.Invoke((MethodInvoker)delegate
            {
                rtbChatLog.AppendText(">> 연결이 종료되었습니다.\n");
                client?.Close();
            });
        }

        private void DisplayMessage(string message, bool isMine)
        {
            // 텍스트 색상, 정렬 등 커스터마이징 가능
            rtbChatLog.AppendText(message);
            rtbChatLog.ScrollToCaret(); // 자동 스크롤
        }

        // Form이 닫힐 때 연결 해제
        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client?.Close();
        }
    }
}