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
using DBP_WinformChat;

// ChatForm은 RichTextBox: rtbChatLog, TextBox: txtInput, Button: btnSend, 
// NotifyIcon: niChatAlert, TextBox: txtSearch, Button: btnSearch, Button: btnSendFile, 
// Button: btnEmojiSmiley, btnEmojiCrying, btnEmojiHeart 를 가진다고 가정합니다.
namespace kyg
{
    public partial class ChatForm : Form
    {
        // 수정
        private TcpClient client;
        private NetworkStream stream;
        private int myId;           // string → int 변경
        private int partnerId;      // string → int 변경
        private bool isSending = false; // 중복 전송 방지 플래그

        private ResourceManager formResourceManager; // 폼 리소스 접근용
        private Dictionary<string, Image> emojiMap = new Dictionary<string, Image>(); // 5-E: 이모티콘 맵

        public ChatForm(int myId, int partnerId) // 생성자 수정
        {
            InitializeComponent();
            this.myId = myId;
            this.partnerId = partnerId;

            // 상대방 이름 가져오기 (수정)
            string partnerName = GetUserName(partnerId);
            this.Text = $"{partnerName} 님과의 채팅 ({myId})";


            // 🚨🚨🚨 5-E: 이모티콘 맵 초기화 (Resources 폴더 직접 참조) 🚨🚨🚨
            LoadEmojisFromDirectory();
            // 5-E: 이모티콘 맵 초기화 (ChatForm.resx 리소스 사용)
            //formResourceManager = new ResourceManager(typeof(ChatForm));

            /*
            try
            {
                emojiMap.Add("EMO1", (Image)formResourceManager.GetObject("smiley"));
                emojiMap.Add("EMO2", (Image)formResourceManager.GetObject("crying"));
                emojiMap.Add("EMO3", (Image)formResourceManager.GetObject("heart"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("이모티콘 리소스 로드 중 오류 발생. resx 파일 확인 필요: " + ex.Message, "리소징 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
            if (niChatAlert != null) niChatAlert.Visible = true;


            // 3주차 5-C: 대화 기록 로드
            LoadChatHistory();

            // 2주차 5-A: 서버 연결
            ConnectToServer();

            // 이벤트 핸들러 연결
            this.btnSearch.Click += btnSearch_Click;
            this.btnSendFile.Click += btnSendFile_Click;

            // 5-E: 세 개의 개별 이모지 버튼 이벤트 연결
            this.btnEmojiSmiley.Click += btnEmojiSmiley_Click;
            this.btnEmojiCrying.Click += btnEmojiCrying_Click;
            this.btnEmojiHeart.Click += btnEmojiHeart_Click;
            this.FormClosing += ChatForm_FormClosing;
        }

        private void LoadEmojisFromDirectory()
        {
            // 1. 실행 파일이 있는 디렉토리를 기준으로 'Resources\Emojis' 폴더 경로를 생성합니다.
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string emojiDirPath = Path.Combine(baseDir, "Resources", "Emojis");

            // 2. 파일에서 이미지를 로드하고 맵에 추가
            try
            {
                // MemoryStream을 사용하여 파일 잠금을 방지하며 이미지를 로드
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


        // 추가 메서드
        // User 테이블에서 이름 가져오기
        private string GetUserName(int userId)
        {
            try
            {
                string query = $"SELECT Name FROM User WHERE UserId = {userId}";
                DataTable dt = DBconnector.GetInstance().Query(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("사용자 이름 조회 중 오류: " + ex.Message);
            }
            return "Unknown";
        }

        // 5-E: 개별 이모지 버튼 클릭 이벤트
        private void btnEmojiSmiley_Click(object sender, EventArgs e)
        {
            SendEmoji("EMO1"); // smiley 코드 전송
        }

        private void btnEmojiCrying_Click(object sender, EventArgs e)
        {
            SendEmoji("EMO2"); // crying 코드 전송
        }

        private void btnEmojiHeart_Click(object sender, EventArgs e)
        {
            SendEmoji("EMO3"); // heart 코드 전송
        }

        // 5-E: 공통 이모지 전송 로직
        private void SendEmoji(string emojiCode)
        {
            if (isSending) return;
            isSending = true;

            try
            {
                // 1. 이미지 존재 여부 확인 (로드 실패 방지)
                if (!emojiMap.ContainsKey(emojiCode) || emojiMap[emojiCode] == null)
                {
                    MessageBox.Show($"이모지 코드 '{emojiCode}'에 해당하는 이미지를 찾을 수 없습니다. resx 파일과 코드의 리소스 이름이 일치하는지 확인하세요.", "이모지 로드 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (client == null || !client.Connected) throw new Exception("서버에 연결되지 않았습니다.");

                // 2. 서버로 메시지 전송
                string contentToSend = $"EMOJI:{emojiCode}"; // Content 필드에 들어갈 내용
                string chatMsg = $"CHAT:{myId}:{partnerId}:{contentToSend}";

                byte[] data = Encoding.UTF8.GetBytes(chatMsg);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                // 3. 내 화면에 표시 (로컬 출력)
                // 시간 표시 통합
                string currentTime = DateTime.Now.ToString("tt hh:mm");
                DisplayEmoji(myId, emojiCode, currentTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show("이모티콘 전송 중 오류 발생: " + ex.Message, "전송 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isSending = false;
            }
        }

        // 디비커넥터 수정
        // ChatMessage 테이블에서 대화 기록 로드
        private void LoadChatHistory()
        {
            // 3주차 5-C: DB에서 과거 대화 기록을 조회하여 화면에 출력
            try
            {
                string query = $" SELECT  FromUserId, Content, SentAt FROM ChatMessage" +
                    $" WHERE (FromUserId = '{myId}' AND ToUserId = '{partnerId}') " +
                    $"OR (FromUserId = '{partnerId}' AND ToUserId = '{myId}') ORDER BY SentAt ASC";

                DataTable history = DBconnector.GetInstance().Query(query);

                if (history == null) return;

                foreach (DataRow row in history.Rows)
                {
                    int senderId = Convert.ToInt32(row["FromUserId"]); // string -> int 로 수정
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
            // 2주차 5-A: 서버 연결 및 ID 등록
            try
            {
                client = new TcpClient("127.0.0.1", 8888);
                //client = new TcpClient("10.201.21.210", 8888);
                stream = client.GetStream();
                string loginMsg = $"LOGIN:{myId}:::";
                byte[] loginData = Encoding.UTF8.GetBytes(loginMsg);
                stream.Write(loginData, 0, loginData.Length);

                // NotifyIcon이 폼에 포함되어 있다면 항상 표시되도록 보장
                if (niChatAlert != null) niChatAlert.Visible = true;

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

                // 현재 시간을 포맷하여 DisplayMessage에 전달
                string currentTime = DateTime.Now.ToString("tt hh:mm");
                DisplayMessage($"[나]: {content}\n", true, currentTime);
                txtInput.Clear();
            }
            finally
            {
                isSending = false;
            }
        }

        private void ReceiveMessages()
        {
            // 2주차 5-A & 3주차 5-B: 메시지 수신 및 알림 로직
            byte[] buffer = new byte[1024];
            while (client.Connected)
            {
                try
                {
                    // 서버로부터 데이터 읽기
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // 연결 종료 시 루프 탈출

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // 메시지 포맷 파싱 (최대 5개의 파트: TYPE:SENDER:RECEIVER:CONTENT[:EXTRA])
                    string[] parts = received.Split(new char[] { ':' }, 4);

                    if (parts.Length < 1) continue;

                    if (parts[0] == "CHAT" && parts.Length >= 4)
                    {
                        int senderId = Convert.ToInt32(parts[1]); // string -> int 
                        string content = parts[3];

                        //if (this.IsDisposed || !this.IsHandleCreated) continue; // 폼이 닫혔다면 다음 루프로 넘어감
                        // UI 스레드에서 UI 업데이트 및 알림 처리 (Invoke 필수)
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (this.IsDisposed) return;
                            // 5-E: 이모티콘 수신 처리

                            // 현재 시간을 포맷하여 출력 함수에 전달
                            string currentTime = DateTime.Now.ToString("tt hh:mm");
                            if (content.StartsWith("EMOJI:"))
                            {
                                DisplayEmoji(senderId, content.Substring(6), currentTime); // EMOJI: 뒤의 코드만 전달
                            }
                            // 5-F: 파일 수신 알림 처리
                            else if (content.StartsWith("FILE_RECEIVED:"))
                            {
                                //string[] fileInfo = content.Split(':');
                                //string fileName = fileInfo[1];
                                //string fullPath = fileInfo[2];
                                string[] fileInfo = content.Split(new char[] { ':' }, 3);
                                string fileName = fileInfo.Length > 1 ? fileInfo[1] : "(unknown)";
                                string fullPath = fileInfo.Length > 2 ? fileInfo[2] : "(unknown)";

                                // 파일 다운로드 여부 및 경로 알림
                                if (MessageBox.Show($"'{senderId}'님이 '{fileName}' 파일을 보냈습니다.\n다운로드 하시겠습니까?\n(경로: {fullPath})", "파일 수신", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    MessageBox.Show($"파일은 서버 경로 '{fullPath}'에 저장되어 있습니다.", "다운로드 경로");
                                }
                                DisplayMessage($"[{senderId}]: 파일 전송 알림: {fileName}", false, currentTime);
                            }
                            else
                            {
                                // 일반 메시지 수신 (2주차 5-A)
                                DisplayMessage($"[{senderId}]: {content}", false, currentTime);
                            }

                            // 3주차 5-B: 대화 도착 알림 기능 (폼이 비활성/최소화 상태일 때)
                            try
                            {
                                if (niChatAlert != null)
                                {
                                    if (!niChatAlert.Visible) niChatAlert.Visible = true;
                                    if (this.WindowState == FormWindowState.Minimized || !this.ContainsFocus)
                                    {
                                        niChatAlert.BalloonTipTitle = $"새 메시지 도착: {senderId}";
                                        niChatAlert.BalloonTipText = content.Length > 50 ? content.Substring(0, 50) + "..." : content;
                                        niChatAlert.ShowBalloonTip(5000);
                                        // 보조: 창 깜박임도 실행
                                        FlashWindow.Flash(this);
                                    }
                                }
                            }
                            catch (ObjectDisposedException)
                            {
                                // NotifyIcon이 Dispose 된 경우 무시(재생성/다시 표시 시 다음 연결에서 처리)
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

            if (this.IsDisposed || !this.IsHandleCreated) return; // 폼이 닫혔다면 함수 종료

            /*
            this.Invoke((MethodInvoker)delegate
            {
                if (this.IsDisposed) return;
                // 폼이 유효할 때만 UI 조작
                rtbChatLog.AppendText(">> 연결이 종료되었습니다.\n"); // 이전에 주석 처리되었던 부분
                client?.Close();
            });
            */
        }

        private void DisplayMessage(string message, bool isMine, string timeString)
        {
            // 메시지 + 시간 정보를 함께 출력합니다.
            string fullMessage = $"{message} ({timeString})\n";

            rtbChatLog.AppendText(fullMessage);
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

        // 4주차 5-E 구현: RichTextBox에 이미지를 삽입하는 메서드
        private void DisplayEmoji(int senderId, string emojiCode, string timeString)
        {
            // 1. 커서를 RichTextBox의 끝으로 이동
            rtbChatLog.SelectionStart = rtbChatLog.TextLength;
            rtbChatLog.SelectionLength = 0;

            // 2. 발신자 ID 텍스트 출력
            // 텍스트 출력 시 시간 포함
            rtbChatLog.AppendText($"[{senderId}] ({timeString}): ");

            Image img = null;
            // 3. 이모지 맵에서 코드에 해당하는 이미지 로드 확인
            if (emojiMap.TryGetValue(emojiCode, out img) && img != null)
            {
                try
                {
                    // 클립보드에 이미지를 복사한 후, RichTextBox에 붙여넣어 이미지 삽입
                    Clipboard.SetImage(img);
                    rtbChatLog.Paste();
                }
                catch (Exception)
                {
                    // 클립보드 사용 실패 시 텍스트로 대체
                    rtbChatLog.AppendText($"(이모티콘:{emojiCode} - UI 삽입 실패)");
                }
            }
            else
            {
                // 맵에 이미지가 없거나 로드되지 않았을 경우 텍스트로 대체
                rtbChatLog.AppendText($"(이모티콘:{emojiCode} - 이미지 로드 실패)");
            }

            // 4. 줄바꿈 및 자동 스크롤
            rtbChatLog.AppendText("\n");
            rtbChatLog.ScrollToCaret();
        }

        // 4주차 5-F: 파일 전송 기능 (클라이언트 송신)
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
                        ZipFile.CreateFromDirectory(tempDir, zipPath);

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
                        File.Delete(filePath); // 임시 ZIP 파일 삭제
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