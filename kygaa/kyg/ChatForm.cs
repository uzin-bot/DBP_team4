using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System;
using System.Threading;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing;
using System.Resources;
using System.Collections.Generic;
using System.IO.Compression;

// ChatForm은 RichTextBox: rtbChatLog, TextBox: txtInput, Button: btnSend, 
// NotifyIcon: niChatAlert, TextBox: txtSearch, Button: btnSearch, Button: btnSendFile, 
// Button: btnEmojiSmiley, btnEmojiCrying, btnEmojiHeart 를 가진다
namespace DBP_finalproject_chatting
{

    public partial class ChatForm : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private string myId;
        private string partnerId;
        private DBHelper dbHelper = new DBHelper();
        private bool isSending = false;

        private ResourceManager formResourceManager;
        private Dictionary<string, Image> emojiMap = new Dictionary<string, Image>();

        public ChatForm(string myId, string partnerId, string partnerName)
        {
            InitializeComponent();
            this.myId = myId;
            this.partnerId = partnerId;
            this.Text = $"{partnerName} 님과의 채팅 ({myId})";

            // 🚨🚨🚨 5-E: 이모티콘 맵 초기화 (Resources 폴더 직접 참조) 🚨🚨🚨
            LoadEmojisFromDirectory();

            // 5-E: 이모티콘 맵 초기화 (리소스 로드)
            //formResourceManager = new ResourceManager(typeof(ChatForm));
            //emojiMap.Add("EMO1", (Image)formResourceManager.GetObject("smiley"));
            //emojiMap.Add("EMO2", (Image)formResourceManager.GetObject("crying"));
            //emojiMap.Add("EMO3", (Image)formResourceManager.GetObject("heart"));

            LoadChatHistory();
            ConnectToServer();

            // 이벤트 핸들러 연결
            //this.btnSearch.Click += btnSearch_Click;
            //this.btnSendFile.Click += btnSendFile_Click;
            //this.btnEmojiSmiley.Click += btnEmojiSmiley_Click;
            //this.btnEmojiCrying.Click += btnEmojiCrying_Click;
            //this.btnEmojiHeart.Click += btnEmojiHeart_Click;
            //this.FormClosing += ChatForm_FormClosing;
        }

        // ChatForm.cs (LoadEmojisFromDirectory 메서드)

        private void LoadEmojisFromDirectory()
        {
            // 1. 실행 파일이 있는 디렉토리를 기준으로 'Resources\Emojis' 폴더 경로를 생성합니다.
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string emojiDirPath = Path.Combine(baseDir, "Resources", "Emojis");

            // 2. 파일에서 이미지를 로드하고 맵에 추가
            try
            {
                // 🚨 MemoryStream을 사용하여 파일 잠금을 방지하며 이미지를 로드 🚨
                // EMO1 = smiley.png
                using (var streamSmiley = new MemoryStream(File.ReadAllBytes(Path.Combine(emojiDirPath, "smiley.png"))))
                {
                    emojiMap.Add("EMO1", Image.FromStream(streamSmiley));
                }
                // EMO2 = crying.png
                using (var streamCrying = new MemoryStream(File.ReadAllBytes(Path.Combine(emojiDirPath, "crying.png"))))
                {
                    emojiMap.Add("EMO2", Image.FromStream(streamCrying));
                }
                // EMO3 = heart.png
                using (var streamHeart = new MemoryStream(File.ReadAllBytes(Path.Combine(emojiDirPath, "heart.png"))))
                {
                    emojiMap.Add("EMO3", Image.FromStream(streamHeart));
                }
            }
            catch (FileNotFoundException ex)
            {
                // 파일 경로 오류가 발생하면 사용자에게 정확한 경로를 안내
                MessageBox.Show($"이모지 파일 로드 실패: {ex.Message}.\n(확인 경로: {emojiDirPath})",
                                "파일 경로 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("이미지 로드 중 예상치 못한 오류 발생: " + ex.Message,
                                "로딩 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 5-E: 개별 이모지 버튼 클릭 이벤트
        private void btnEmojiSmiley_Click(object sender, EventArgs e) { SendEmoji("EMO1"); }
        private void btnEmojiCrying_Click(object sender, EventArgs e) { SendEmoji("EMO2"); }
        private void btnEmojiHeart_Click(object sender, EventArgs e) { SendEmoji("EMO3"); }

        // 5-E: 공통 이모지 전송 로직
        private void SendEmoji(string emojiCode)
        {
            if (isSending) return;
            isSending = true;

            try
            {
                if (client == null || !client.Connected) throw new Exception("서버에 연결되지 않았습니다.");

                string contentToSend = $"EMOJI:{emojiCode}";
                string chatMsg = $"CHAT:{myId}:{partnerId}:{contentToSend}";

                byte[] data = Encoding.UTF8.GetBytes(chatMsg);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                // 시간 표시 통합
                string currentTime = DateTime.Now.ToString("tt hh:mm");
                DisplayEmoji(myId, emojiCode, currentTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show("이모티콘 전송 오류: " + ex.Message, "오류");
            }
            finally
            {
                isSending = false;
            }
        }


        private void LoadChatHistory()
        {
            // 3주차 5-C: DB에서 과거 대화 기록을 조회하여 화면에 출력
            try
            {
                string query = @"
                SELECT SenderID, Content, SendTime
                FROM ChatMessage
                WHERE (SenderID = @user1Id AND ReceiverID = @user2Id) 
                   OR (SenderID = @user2Id AND ReceiverID = @user1Id)
                ORDER BY SendTime ASC";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                new MySqlParameter("@user1Id", myId),
                new MySqlParameter("@user2Id", partnerId)
                };

                DataTable history = dbHelper.ExecuteSelect(query, parameters);

                foreach (DataRow row in history.Rows)
                {
                    string senderId = row["SenderID"].ToString();
                    string content = row["Content"].ToString();
                    // SendTime 컬럼을 DateTime 형식으로 읽어와 포맷
                    DateTime sendTime = (DateTime)row["SendTime"];
                    string timeString = sendTime.ToString("tt hh:mm");

                    if (content.StartsWith("EMOJI:"))
                    {
                        DisplayEmoji(senderId, content.Substring(6), timeString);
                    }
                    else
                    {
                        // DisplayMessage에 timeString 전달
                        DisplayMessage($"[{senderId}]: {content}", senderId == myId, timeString);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("대화 기록을 불러오는 중 오류 발생: " + ex.Message, "DB 오류");
            }
        }

        private void ConnectToServer()
        {
            // ... (기존 ConnectToServer 로직 유지)
            try
            {
                client = new TcpClient("127.0.0.1", 8888);
                //client = new TcpClient("10.201.21.210", 8888);
                stream = client.GetStream();
                string loginMsg = $"LOGIN:{myId}:::";
                byte[] loginData = Encoding.UTF8.GetBytes(loginMsg);
                stream.Write(loginData, 0, loginData.Length);

                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                rtbChatLog.AppendText(">> 서버에 연결되었습니다.\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버 연결 오류: " + ex.Message, "연결 실패");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // 2주차 5-A: 일반 텍스트 메시지 전송
            if (isSending) return;
            isSending = true;

            try
            {
                string content = txtInput.Text;
                if (string.IsNullOrWhiteSpace(content)) return;

                if (client == null || !client.Connected) { return; }

                string chatMsg = $"CHAT:{myId}:{partnerId}:{content}";
                byte[] data = Encoding.UTF8.GetBytes(chatMsg);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                // 🚨 현재 시간을 포맷하여 DisplayMessage에 전달 🚨
                string currentTime = DateTime.Now.ToString("tt hh:mm");
                DisplayMessage($"[나]: {content}", true, currentTime);
                txtInput.Clear();
            }
            finally
            {
                isSending = false;
            }
        }

        private void ReceiveMessages()
        {
            // 2주차 5-A & 3주차 5-B: 메시지 수신 및 알림
            byte[] buffer = new byte[1024];
            while (client.Connected)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string[] parts = received.Split(new char[] { ':' }, 4);

                    if (parts[0] == "CHAT" && parts.Length >= 4)
                    {
                        string senderId = parts[1];
                        string content = parts[3];

                        this.Invoke((MethodInvoker)delegate
                        {
                            if (this.IsDisposed) return;

                            // 현재 시간을 포맷하여 출력 함수에 전달
                            string currentTime = DateTime.Now.ToString("tt hh:mm");

                            if (content.StartsWith("EMOJI:") && content.Length > 6)
                            {
                                DisplayEmoji(senderId, content.Substring(6), currentTime);
                            }
                            else if (content.StartsWith("FILE_RECEIVED:"))
                            {
                                string[] fileInfo = content.Split(':');
                                string fileName = fileInfo[1];
                                string fullPath = fileInfo[2];

                                if (MessageBox.Show($"'{senderId}'님이 '{fileName}' 파일을 보냈습니다.\n다운로드 하시겠습니까?\n(경로: {fullPath})", "파일 수신", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    MessageBox.Show($"파일은 서버 경로 '{fullPath}'에 저장되어 있습니다.", "다운로드 경로");
                                }
                                DisplayMessage($"[{senderId}]: 파일 전송 알림: {fileName}", false, currentTime);
                            }
                            else
                            {
                                DisplayMessage($"[{senderId}]: {content}", false, currentTime);
                            }

                            if (this.WindowState == FormWindowState.Minimized || !this.ContainsFocus)
                            {
                                niChatAlert.BalloonTipTitle = $"새 메시지 도착: {senderId}";
                                niChatAlert.BalloonTipText = content.Length > 50 ? content.Substring(0, 50) + "..." : content;
                                niChatAlert.ShowBalloonTip(5000);
                            }
                        });
                    }
                }
                catch (Exception) { break; }
            }
            if (this.IsDisposed) return;
            //this.Invoke((MethodInvoker)delegate { client?.Close(); });
        }

        // DisplayMessage 시그니처 및 내용 수정
        private void DisplayMessage(string message, bool isMine, string timeString)
        {
            // 메시지 + 시간 정보를 함께 출력합니다.
            string fullMessage = $"{message} ({timeString})\n";

            rtbChatLog.AppendText(fullMessage);
            rtbChatLog.ScrollToCaret();
        }

        // ... (btnSearch_Click 메서드 유지)

        // 5-E 구현: 이모티콘 이미지 삽입 로직
        // 2. DisplayEmoji 시그니처 및 내용 수정
        private void DisplayEmoji(string senderId, string emojiCode, string timeString)
        {
            rtbChatLog.SelectionStart = rtbChatLog.TextLength;
            rtbChatLog.SelectionLength = 0;

            // 텍스트 출력 시 시간 포함
            rtbChatLog.AppendText($"[{senderId}] ({timeString}): ");

            Image img = null;
            if (emojiMap.TryGetValue(emojiCode, out img) && img != null)
            {
                Clipboard.SetImage(img);
                rtbChatLog.Paste();
            }
            else
            {
                rtbChatLog.AppendText($"(이모티콘:{emojiCode} - 이미지 로드 실패)");
            }

            rtbChatLog.AppendText("\n");
            rtbChatLog.ScrollToCaret();
        }

        // 4주차 5-D: 대화 내용 검색 기능
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;
            if (string.IsNullOrWhiteSpace(keyword)) return;

            // 기존 하이라이트 초기화
            rtbChatLog.SelectAll();
            rtbChatLog.SelectionBackColor = rtbChatLog.BackColor;

            int searchStartIndex = 0;
            int matches = 0;

            // 키워드를 찾아 노란색으로 하이라이트
            while (searchStartIndex < rtbChatLog.TextLength)
            {
                int index = rtbChatLog.Find(keyword, searchStartIndex, RichTextBoxFinds.None);

                if (index == -1) break;

                rtbChatLog.Select(index, keyword.Length);
                rtbChatLog.SelectionBackColor = System.Drawing.Color.Yellow;
                rtbChatLog.ScrollToCaret();

                searchStartIndex = index + keyword.Length;
                matches++;
            }

            MessageBox.Show($"{matches}개의 결과를 찾았습니다.", "검색 완료");
        }

        // 3. btnSendFile_Click 시그니처 수정
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (isSending) return;
            isSending = true;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath;
                    string fileName;

                    if (ofd.FileNames.Length > 1)
                    {
                        // 여러 파일 선택 시: 임시 ZIP 파일 생성 로직
                        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                        Directory.CreateDirectory(tempDir);

                        foreach (string file in ofd.FileNames)
                        {
                            File.Copy(file, Path.Combine(tempDir, Path.GetFileName(file)));
                        }

                        string zipPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".zip");
                        System.IO.Compression.ZipFile.CreateFromDirectory(tempDir, zipPath);

                        filePath = zipPath;
                        fileName = Path.GetFileName(zipPath);
                        Directory.Delete(tempDir, true); // 임시 폴더 삭제
                    }
                    else
                    {
                        filePath = ofd.FileName;
                        fileName = Path.GetFileName(filePath);
                    }

                    long fileSize = new FileInfo(filePath).Length;

                    // 1. 서버에게 파일 전송 헤더 전송
                    string headerMsg = $"FILE_HEADER:{myId}:{partnerId}:{fileName}:{fileSize}";
                    byte[] headerData = Encoding.UTF8.GetBytes(headerMsg);
                    stream.Write(headerData, 0, headerData.Length);
                    stream.Flush();

                    // 2. 파일 데이터를 서버로 전송
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    stream.Write(fileBytes, 0, fileBytes.Length);
                    stream.Flush();

                    // 현재 시간을 포맷하여 DisplayMessage에 전달
                    string currentTime = DateTime.Now.ToString("tt hh:mm");
                    DisplayMessage($"[나]: 파일 전송 요청 완료: {fileName}", true, currentTime);

                    if (ofd.FileNames.Length > 1)
                    {
                        File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("파일 전송 중 오류: " + ex.Message);
                }
            }
            isSending = false;
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client?.Close();
        }
    }
}