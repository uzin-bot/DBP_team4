using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System;
using System.Threading;
using System.Data;
using MySqlConnector;
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
        private DateTime lastEmojiSendTime = DateTime.MinValue; // 이모지 마지막 전송 시간

        // [ChatForm.cs] 클래스 멤버 변수 선언부
        private bool isDownloading = false;      // 다운로드 중인지 여부
        private long remainingBytes = 0;         // 남은 파일 크기
        private FileStream fileStream = null;    // 파일 저장용 스트림


        private ResourceManager formResourceManager; // 폼 리소스 접근용
        private Dictionary<string, Image> emojiMap = new Dictionary<string, Image>(); // 5-E: 이모티콘 맵
        private PermissionManager permissionManager; // 어드민 추가

        public ChatForm(int myId, int partnerId) // 생성자 수정
        {
            InitializeComponent();
            this.myId = myId;
            this.partnerId = partnerId;
            this.permissionManager = new PermissionManager(); // 어드민 추가

            // 어드민: 채팅창 열기 전 권한 체크
            var result = permissionManager.CanSendMessage(myId, partnerId);
            if (!result.CanSend)
            {
                MessageBox.Show(result.Reason, "채팅 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close(); // 폼 로드 후 바로 닫기
                return;
            }

            // 상대방 이름 가져오기 (수정)
            string partnerName = GetUserName(partnerId);
            this.Text = $"{partnerName} 님과의 채팅 ({myId})";


            // 5-E: 이모티콘 맵 초기화 (Resources 폴더 직접 참조)
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
            //if (niChatAlert != null) niChatAlert.Visible = true;


            // 3주차 5-C: 대화 기록 로드
            LoadChatHistory();

            // 2주차 5-A: 서버 연결
            ConnectToServer();

            // 이벤트 핸들러 연결
            //this.btnSearch.Click += btnSearch_Click;
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
            // 중복 전송 방지 플래그 체크 (기존 로직)
            if (isSending) return;

            // 쿨다운(Delay) 시간 체크 (새로운 로직)
            // 마지막 전송 시간과 현재 시간의 차이가 2초 미만이면 전송을 막고 경고 메시지 표시
            TimeSpan elapsed = DateTime.Now - lastEmojiSendTime;
            if (elapsed.TotalSeconds < 2)
            {
                // 쿨다운 남은 시간 계산 (소수점 첫째 자리까지만 표시)
                double remaining = 2.0 - elapsed.TotalSeconds;
                MessageBox.Show($"이모지는 2초에 한 번만 전송할 수 있습니다. {remaining:F1}초 후 다시 시도해 주세요.",
                                "전송 제한", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // 전송 중단
            }

            isSending = true;

            try
            {

                // 어드민: 전송 전 권한 체크 추가
                var result = permissionManager.CanSendMessage(myId, partnerId);
                if (!result.CanSend)
                {
                    MessageBox.Show(result.Reason, "전송 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 1. 이미지 존재 여부 확인 (로드 실패 방지)
                if (!emojiMap.ContainsKey(emojiCode) || emojiMap[emojiCode] == null)
                {
                    MessageBox.Show($"이모지 코드 '{emojiCode}'에 해당하는 이미지를 찾을 수 없습니다. resx 파일과 코드의 리소스 이름이 일치하는지 확인하세요.", "이모지 로드 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (client == null || !client.Connected) throw new Exception("서버에 연결되지 않았습니다.");

                // 2. 서버로 메시지 전송 (기존 로직)
                string contentToSend = $"EMOJI:{emojiCode}"; // Content 필드에 들어갈 내용
                string chatMsg = $"CHAT:{myId}:{partnerId}:{contentToSend}";

                byte[] data = Encoding.UTF8.GetBytes(chatMsg);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                // 3. 내 화면에 표시 (로컬 출력) (기존 로직)
                string currentTime = DateTime.Now.ToString("tt hh:mm");
                DisplayEmoji(myId, emojiCode, currentTime, " (1)");

                // 4. 전송 성공 시 마지막 전송 시간 업데이트
                lastEmojiSendTime = DateTime.Now;
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
                string query = $" SELECT  FromUserId, Content, SentAt, IsRead FROM ChatMessage" +
                    $" WHERE (FromUserId = {myId} AND ToUserId = {partnerId}) " +
                    $"OR (FromUserId = {partnerId} AND ToUserId = {myId}) ORDER BY SentAt ASC";

                DataTable history = DBconnector.GetInstance().Query(query);

                if (history == null) return;

                foreach (DataRow row in history.Rows)
                {
                    int senderId = Convert.ToInt32(row["FromUserId"]); // string -> int 로 수정
                    string content = row["Content"].ToString();
                    // SendAT 컬럼을 DateTime 형식으로 읽어와 포맷
                    DateTime sendTime = (DateTime)row["SentAt"];
                    string timeString = sendTime.ToString("tt hh:mm");
                    int isRead = Convert.ToInt32(row["IsRead"]);

                    // 내가 보낸 메시지에만 읽음 표시
                    string readStatus = "";
                    if (senderId == myId && isRead == 0)
                    {
                        readStatus = " (1)"; // 안 읽음
                    }

                    if (content.StartsWith("EMOJI:"))
                    {
                        DisplayEmoji(senderId, content.Substring(6), timeString, readStatus);
                    }
                    else
                    {
                        // DisplayMessage에 timeString 전달
                        string senderLabel = senderId == myId ? "나" : senderId.ToString();
                        DisplayMessage($"[{senderLabel}]: {content}{readStatus}", senderId == myId, timeString);
                    }
                }
                // 대화 기록 로드 후 읽음 처리
                MarkMessagesAsRead();
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
                client = new TcpClient("51.21.27.234", 12345);
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

                // 어드민: 전송 전 권한 체크 추가
                var result = permissionManager.CanSendMessage(myId, partnerId);
                if (!result.CanSend)
                {
                    MessageBox.Show(result.Reason, "전송 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (client == null || !client.Connected) { return; }

                string chatMsg = $"CHAT:{myId}:{partnerId}:{content}";
                byte[] data = Encoding.UTF8.GetBytes(chatMsg);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                // 현재 시간을 포맷하여 DisplayMessage에 전달
                string currentTime = DateTime.Now.ToString("tt hh:mm");
                DisplayMessage($"[나]: {content} (1)\n", true, currentTime);
                txtInput.Clear();
            }
            finally
            {
                isSending = false;
            }
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[8192];
            StringBuilder messageBuilder = new StringBuilder();

            while (client != null && client.Connected)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    // [1] 파일 다운로드 모드 (바이너리 데이터 처리)
                    if (isDownloading && fileStream != null)
                    {
                        int writeSize = (int)Math.Min(bytesRead, remainingBytes);
                        fileStream.Write(buffer, 0, writeSize);
                        remainingBytes -= writeSize;

                        if (remainingBytes <= 0)
                        {
                            fileStream.Flush();
                            fileStream.Close();
                            fileStream = null;
                            isDownloading = false;
                            this.Invoke((MethodInvoker)delegate {
                                MessageBox.Show("다운로드가 완료되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            });
                        }
                        continue;
                    }

                    // [2] 텍스트 메시지 처리
                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    messageBuilder.Append(received);
                    string fullMessage = messageBuilder.ToString();
                    string[] messages = fullMessage.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

                    bool lastMessageComplete = fullMessage.EndsWith("\0");
                    int messagesToProcess = lastMessageComplete ? messages.Length : messages.Length - 1;

                    for (int i = 0; i < messagesToProcess; i++)
                    {
                        string msg = messages[i].Trim();

                        // ▼▼▼ [핵심 수정] 5가 아니라 4로 변경해야 내용이 안 잘립니다! ▼▼▼
                        // 구조: TYPE : Sender : Receiver : Content(나머지 전체)
                        string[] parts = msg.Split(new char[] { ':' }, 4);

                        if (parts.Length < 1) continue;
                        string type = parts[0];

                        // [A] 파일 응답 헤더 처리 (FILE_RESP)
                        // 서버 전송 포맷: FILE_RESP:SERVER:ReqID:FileName:Size
                        // Split(4)를 했으므로 parts[3]에 "FileName:Size"가 들어있음
                        if (type == "FILE_RESP" && parts.Length >= 4)
                        {
                            string filePayload = parts[3]; // "파일명:크기"
                            int lastColonIndex = filePayload.LastIndexOf(':');

                            if (lastColonIndex != -1)
                            {
                                string sizeStr = filePayload.Substring(lastColonIndex + 1);
                                if (long.TryParse(sizeStr, out long size))
                                {
                                    remainingBytes = size;
                                    isDownloading = true;
                                    continue;
                                }
                            }
                        }
                        // [B] 채팅 및 알림 처리 (CHAT)
                        else if (type == "CHAT" && parts.Length >= 4)
                        {
                            int senderId = Convert.ToInt32(parts[1]);
                            string content = parts[3]; // 이제 잘리지 않은 전체 내용이 들어옵니다.

                            this.Invoke((MethodInvoker)delegate
                            {
                                if (this.IsDisposed) return;
                                string currentTime = DateTime.Now.ToString("tt hh:mm");

                                // 파일 도착 알림 확인
                                if (content.StartsWith("FILE_RECEIVED:"))
                                {
                                    try
                                    {
                                        // 내용 파싱: FILE_RECEIVED : 파일명 : 경로
                                        string[] fileInfo = content.Split(new char[] { ':' }, 3);
                                        string fileName = fileInfo.Length > 1 ? fileInfo[1] : "unknown";

                                        DialogResult dr = MessageBox.Show(
                                            $"'{senderId}'님이 '{fileName}' 파일을 보냈습니다.\n다운로드 하시겠습니까?",
                                            "파일 수신", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                        if (dr == DialogResult.Yes)
                                        {
                                            SaveFileDialog sfd = new SaveFileDialog();
                                            sfd.FileName = fileName;
                                            sfd.Filter = "All Files (*.*)|*.*";

                                            if (sfd.ShowDialog(this) == DialogResult.OK)
                                            {
                                                fileStream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);

                                                string reqMsg = $"FILE_DOWNLOAD_REQ:{myId}:{fileName}";
                                                byte[] reqData = Encoding.UTF8.GetBytes(reqMsg);
                                                stream.Write(reqData, 0, reqData.Length);
                                                stream.Flush();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("오류: " + ex.Message);
                                    }

                                    // 채팅창에는 알림 문구만 출력
                                    DisplayMessage($"[{senderId}]: 파일이 도착했습니다 ({content.Split(':')[1]})", false, currentTime);
                                }
                                else if (content.StartsWith("EMOJI:"))
                                {
                                    DisplayEmoji(senderId, content.Substring(6), currentTime);
                                }
                                else
                                {
                                    DisplayMessage($"[{senderId}]: {content}", false, currentTime);
                                }

                                if (CanMarkAsRead()) MarkMessagesAsRead();
                            });
                        }
                        else if (type == "READ_CONFIRM")
                        {
                            this.Invoke((MethodInvoker)delegate {
                                if (!this.IsDisposed) { rtbChatLog.Clear(); LoadChatHistory(); }
                            });
                        }
                    }

                    if (!lastMessageComplete && messages.Length > 0)
                    {
                        messageBuilder.Clear();
                        messageBuilder.Append(messages[messages.Length - 1]);
                    }
                    else { messageBuilder.Clear(); }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); break; }
            }
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
        private void DisplayEmoji(int senderId, string emojiCode, string timeString, string readStatus = "")
        {
            // 1. 커서를 RichTextBox의 끝으로 이동
            rtbChatLog.SelectionStart = rtbChatLog.TextLength;
            rtbChatLog.SelectionLength = 0;

            // 2. 발신자 ID 텍스트 출력
            // 텍스트 출력 시 시간 포함
            // 읽음 표시 추가
            rtbChatLog.AppendText($"[{senderId}] ({timeString}){readStatus}: ");

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

            // 어드민: 전송 전 권한 체크 추가
            var result = permissionManager.CanSendMessage(myId, partnerId);
            if (!result.CanSend)
            {
                MessageBox.Show(result.Reason, "파일 전송 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
            try
            {
                // ✅ 무조건 읽음 처리 (조건 없이)
                string queryChatMessage = $@"
            UPDATE ChatMessage 
            SET IsRead = 1 
            WHERE FromUserId = {partnerId} 
            AND ToUserId = {myId} 
            AND IsRead = 0";

                int updatedCount = DBconnector.GetInstance().NonQuery(queryChatMessage);

                // ✅ RecentChat도 무조건 업데이트
                string queryRecentChat = $@"
            UPDATE RecentChat
            SET UnreadCount = 0
            WHERE UserId = {myId}
            AND PartnerUserId = {partnerId}";

                DBconnector.GetInstance().NonQuery(queryRecentChat);

                Console.WriteLine($"[ChatForm] FormClosing에서 읽음 처리: ChatMessage {updatedCount}개 업데이트");

                // ✅ READ_CONFIRM 전송
                if (updatedCount > 0 && client != null && client.Connected)
                {
                    try
                    {
                        string confirmMsg = $"READ_CONFIRM:{myId}:{partnerId}::";
                        byte[] data = Encoding.UTF8.GetBytes(confirmMsg);
                        stream.Write(data, 0, data.Length);
                        stream.Flush();

                        System.Threading.Thread.Sleep(500); // 서버 처리 대기
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChatForm] FormClosing 오류: {ex.Message}");
            }
            finally
            {
                client?.Close();
            }
        }

        // 상대방이 나한테 보낸 안 읽은 메시지를 읽음 처리 (디비에)
        private void MarkMessagesAsRead()
        {
            try
            {
                string query = $@"
            UPDATE ChatMessage 
            SET IsRead = 1 
            WHERE FromUserId = {partnerId} 
            AND ToUserId = {myId} 
            AND IsRead = 0";

                int updatedCount = DBconnector.GetInstance().NonQuery(query);

                if (updatedCount > 0)
                { // ✅ 2. RecentChat의 UnreadCount도 0으로 업데이트 추가!
                    string updateRecent = $@"
                UPDATE RecentChat
                SET UnreadCount = 0
                WHERE UserId = {myId}
                AND PartnerUserId = {partnerId}";

                    DBconnector.GetInstance().NonQuery(updateRecent);

                    System.Threading.Thread.Sleep(1000);
                    SendReadConfirm();
                }

                Console.WriteLine($"[ChatForm] {partnerId}로부터 받은 메시지 읽음 처리 완료");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChatForm] 읽음 처리 오류: {ex.Message}");
            }
        }

        // 읽음 확인 서버에 전송
        private void SendReadConfirm()
        {
            try
            {
                if (client == null || !client.Connected) return;

                // 서버에 읽음 확인 전송
                string confirmMsg = $"READ_CONFIRM:{myId}:{partnerId}::";
                byte[] data = Encoding.UTF8.GetBytes(confirmMsg);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                Console.WriteLine($"[ChatForm] 읽음 확인 전송: {myId} → {partnerId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChatForm] 읽음 확인 전송 오류: {ex.Message}");
            }
        }

        // 창이 실제로 보이고, 최소화 아니고, 포커스가 내 폼에 있을 때만 읽음 처리
        private bool CanMarkAsRead()
        {
            return this.Visible && this.WindowState == FormWindowState.Normal && this.ContainsFocus;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            MarkMessagesAsRead();
        }
    }
}