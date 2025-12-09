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
using DBP_Chat;

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
        public int PartnerUserId { get; private set; } //1208추가
        private bool isSending = false; // 중복 전송 방지 플래그
        private DateTime lastEmojiSendTime = DateTime.MinValue; // 이모지 마지막 전송 시간

        // 문서3에만 있음 - 클래스 필드 선언부
        private bool isDownloading = false;
        private long remainingBytes = 0;
        private FileStream fileStream = null;

        private ResourceManager formResourceManager; // 폼 리소스 접근용
        private Dictionary<string, Image> emojiMap = new Dictionary<string, Image>(); // 5-E: 이모티콘 맵
        private PermissionManager permissionManager; // 어드민 추가

        public ChatForm(int myId, int partnerId) // 생성자 수정
        {
            InitializeComponent();
            
            this.myId = myId;
            this.partnerId = partnerId;
            this.permissionManager = new PermissionManager(); // 어드민 추가
            this.PartnerUserId = partnerId;

            // 전역 테마 변경 이벤트 구독
            ThemeManager.ThemeChanged += mode => this.OnThemeChanged(mode);

            // 현재 테마 상태에 따라 초기 스타일 적용
            if (ThemeManager.CurrentMode == ThemeMode.Dark)
            {
                this.ApplyDarkTheme();
            }
            else
            {
                this.ApplyChatFormUIHelper();
            }
            // 어드민: 채팅창 열기 전 권한 체크
            var result = permissionManager.CanSendMessage(myId, partnerId);
            if (!result.CanSend)
            {
                MessageBox.Show(result.Reason, "채팅 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close(); // 폼 로드 후 바로 닫기
                return;
            }

            // ✅ 멀티프로필 닉네임 가져오기
            string partnerNickname = GetNicknameForUser(partnerId);
            string partnerName = GetUserName(partnerId);

            // ✅ 타이틀 변경: name(nickname) 님 형식
            this.Text = $"{partnerName}({partnerNickname}) 님과의 채팅 ({myId})";

            /*
            // 상대방 이름 가져오기 (수정)
            string partnerName = GetUserName(partnerId);
            this.Text = $"{partnerName} 님과의 채팅 ({myId})";
            */

            // 5-E: 이모지 맵 초기화 (Resources 폴더 직접 참조)
            LoadEmojisFromDirectory();

            //if (niChatAlert != null) niChatAlert.Visible = true;

            // 2주차 5-A: 서버 연결
            ConnectToServer();

            // 3주차 5-C: 대화 기록 로드
            LoadChatHistory();

            // 이벤트 핸들러 연결
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

        // ✅ 멀티프로필을 고려한 닉네임 가져오기 메서드 (새로 추가)
        private string GetNicknameForUser(int targetUserId)
        {
            try
            {
                // 1. UserProfileMap 확인 (상대방이 나에게 보여주는 멀티프로필)
                string mapQuery = $@"
            SELECT ProfileId 
            FROM UserProfileMap 
            WHERE OwnerUserId = {targetUserId} 
            AND TargetUserId = {myId}";

                DataTable mapDt = DBconnector.GetInstance().Query(mapQuery);

                if (mapDt != null && mapDt.Rows.Count > 0)
                {
                    // 멀티프로필이 있는 경우
                    int profileId = Convert.ToInt32(mapDt.Rows[0]["ProfileId"]);

                    string profileQuery = $@"
                SELECT Nickname 
                FROM Profile 
                WHERE ProfileId = {profileId}";

                    DataTable profileDt = DBconnector.GetInstance().Query(profileQuery);

                    if (profileDt != null && profileDt.Rows.Count > 0)
                    {
                        return profileDt.Rows[0]["Nickname"].ToString();
                    }
                }

                // 2. 멀티프로필이 없으면 기본 프로필 사용
                string defaultQuery = $@"
            SELECT Nickname 
            FROM Profile 
            WHERE UserId = {targetUserId} 
            AND IsDefault = 1";

                DataTable defaultDt = DBconnector.GetInstance().Query(defaultQuery);

                if (defaultDt != null && defaultDt.Rows.Count > 0)
                {
                    return defaultDt.Rows[0]["Nickname"].ToString();
                }

                return "Unknown";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetNicknameForUser] 오류: {ex.Message}");
                return "Unknown";
            }
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

                // ✅ 상대방 닉네임 미리 가져오기
                string partnerNickname = GetNicknameForUser(partnerId);

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
                        //원래 " (1)" 임 
                        readStatus = " "; // 안 읽음
                    }

                    if (content.StartsWith("EMOJI:"))
                    {
                        DisplayEmojiWithNickname(senderId, partnerNickname, content.Substring(6), timeString, readStatus);
                        //DisplayEmoji(senderId, content.Substring(6), timeString, readStatus);
                    }
                    else
                    {
                        /*
                        // DisplayMessage에 timeString 전달
                        string senderLabel = senderId == myId ? "나" : senderId.ToString();
                        DisplayMessage($"[{senderLabel}]: {content}{readStatus}", senderId == myId, timeString);
                        */
                        // ✅ 일반 메시지도 닉네임으로 표시
                        string senderLabel = senderId == myId ? "나" : partnerNickname;
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

        /*
        private void ReceiveMessages()
        {
            // 2주차 5-A & 3주차 5-B: 메시지 수신 및 알림 로직
            byte[] buffer = new byte[4096]; // ✅ 버퍼 크기 증가
            StringBuilder messageBuilder = new StringBuilder(); // ✅ 메시지 버퍼링용

            while (client != null && client.Connected) // ✅ null 체크 추가
            {
                try
                {
                    // 서버로부터 데이터 읽기
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // 연결 종료 시 루프 탈출

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"[ChatForm] 원본 수신: [{received}]"); // ✅ 디버깅용

                    // 메시지 누적
                    messageBuilder.Append(received);
                    string fullMessage = messageBuilder.ToString();

                    // null 문자로 메시지 구분
                    string[] messages = fullMessage.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

                    // 마지막 메시지가 완전하지 않을 수 있으므로 체크
                    bool lastMessageComplete = fullMessage.EndsWith("\0");
                    int messagesToProcess = lastMessageComplete ? messages.Length : messages.Length - 1;

                    // 완전한 메시지만 처리
                    for (int i = 0; i < messagesToProcess; i++)
                    {
                        string msg = messages[i].Trim();
                        if (string.IsNullOrWhiteSpace(msg)) continue;

                        Console.WriteLine($"[ChatForm] 처리할 메시지: [{msg}]"); // ✅ 디버깅용

                        // 메시지 포맷 파싱 (최대 4개의 파트: TYPE:SENDER:RECEIVER:CONTENT)
                        string[] parts = msg.Split(new char[] { ':' }, 4); // ✅ msg로 변경

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

                                System.Threading.Thread.Sleep(1000);

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

                                // 메시지 받은 후 즉시 읽음 처리
                                if (CanMarkAsRead())
                                {
                                    MarkMessagesAsRead();
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
                        else if (parts[0] == "READ_CONFIRM" && parts.Length >= 2)
                        {
                            int readerId = Convert.ToInt32(parts[1]); // 읽은 사람 ID

                            Console.WriteLine($"[ChatForm] 읽음 확인 수신: {readerId}가 내 메시지를 읽음");

                            this.Invoke((MethodInvoker)delegate
                            {
                                if (this.IsDisposed) return;

                                System.Threading.Thread.Sleep(1000);

                                // 화면 전체 다시 로드 (읽음 상태 반영)
                                rtbChatLog.Clear();
                                LoadChatHistory();
                            });
                        }
                    }

                    // 미처리 메시지 보관
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
                catch (System.IO.IOException) // 연결 끊김 또는 스트림 오류
                {
                    Console.WriteLine("[ChatForm] 연결 끊김"); // ✅ 디버깅용
                    break;
                }
                catch (Exception ex)
                {
                    // 기타 예외 처리
                    Console.WriteLine($"[ChatForm] 수신 오류: {ex.Message}"); // ✅ 디버깅용
                }
            }

            // 연결 종료 후 처리
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
        //}

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

                    // ============================================================
                    // [중요] 파일 다운로드 모드 (ZIP 파일 0KB/깨짐 방지)
                    // ============================================================
                    // 파일 받는 중에는 절대 GetString()을 하지 않고 바이트 그대로 씁니다.
                    // 그래야 ZIP 내부의 0x00 바이트가 문자열 종료로 인식되어 잘리는 것을 막습니다.
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
                        // 파일 데이터를 썼으면 이번 루프는 여기서 끝내고 다시 Read 대기
                        continue;
                    }

                    // ============================================================
                    // [텍스트 모드] 일반 채팅, 알림, 헤더 처리
                    // ============================================================
                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    messageBuilder.Append(received);
                    string fullMessage = messageBuilder.ToString();

                    // \0 기준으로 메시지 쪼개기
                    string[] messages = fullMessage.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

                    bool lastMessageComplete = fullMessage.EndsWith("\0");
                    int messagesToProcess = lastMessageComplete ? messages.Length : messages.Length - 1;

                    for (int i = 0; i < messagesToProcess; i++)
                    {
                        string msg = messages[i].Trim();
                        if (string.IsNullOrEmpty(msg)) continue;

                        // 파싱 (최대 5개로 분할)
                        string[] parts = msg.Split(new char[] { ':' }, 5);
                        if (parts.Length < 1) continue;

                        string type = parts[0];

                        // [A] 파일 다운로드 시작 헤더 (FILE_RESP)
                        if (type == "FILE_RESP" && parts.Length >= 5)
                        {
                            if (long.TryParse(parts[4], out long size))
                            {
                                remainingBytes = size;
                                isDownloading = true;

                                // 헤더 처리 끝났으니 빌더 비우고, 즉시 break하여
                                // 다음 버퍼부터는 위쪽의 [파일 다운로드 모드] if문으로 들어가게 함
                                messageBuilder.Clear();
                                break;
                            }
                        }

                        // [B] 읽음 확인 (READ_CONFIRM)
                        else if (type == "READ_CONFIRM")
                        {
                            this.Invoke((MethodInvoker)delegate {
                                if (!this.IsDisposed) { rtbChatLog.Clear(); LoadChatHistory(); }
                            });
                        }

                        // [C] 채팅 및 파일 수신 알림 (CHAT)
                        else if (type == "CHAT" && parts.Length >= 4)
                        {
                            int senderId = Convert.ToInt32(parts[1]);
                            // parts[3]에 내용이 들어있음 (4조각으로 잘라도 되지만 5조각 로직 유지)
                            string content = (parts.Length > 3) ? parts[3] : "";

                            // 만약 5조각으로 잘라서 내용이 더 뒤에 있다면 이어붙임 (안전장치)
                            if (parts.Length > 4) content += ":" + parts[4];

                            this.Invoke((MethodInvoker)delegate
                            {
                                if (this.IsDisposed) return;
                                string currentTime = DateTime.Now.ToString("tt hh:mm");

                                string partnerNickname = GetNicknameForUser(partnerId);

                                if (content.StartsWith("FILE_RECEIVED:"))
                                {
                                    try
                                    {
                                        string[] fileInfo = content.Split(new char[] { ':' }, 3);
                                        string fileName = fileInfo.Length > 1 ? fileInfo[1] : "unknown";

                                        DialogResult dr = MessageBox.Show(
                                            $"'{senderId}'님이 '{fileName}' 파일을 보냈습니다.\n다운로드 하시겠습니까?",
                                            "파일 수신", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                        if (dr == DialogResult.Yes)
                                        {
                                            SaveFileDialog sfd = new SaveFileDialog();
                                            sfd.FileName = fileName;
                                            if (sfd.ShowDialog(this) == DialogResult.OK)
                                            {
                                                // ★★★ 핵심 수정: fileStream 먼저 생성 ★★★
                                                fileStream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);

                                                // 서버에 요청: FILE_DOWNLOAD_REQ:내ID:파일명
                                                string reqMsg = $"FILE_DOWNLOAD_REQ:{myId}:{fileName}\0";  // ← \0 추가
                                                byte[] reqData = Encoding.UTF8.GetBytes(reqMsg);
                                                stream.Write(reqData, 0, reqData.Length);
                                                stream.Flush();
                                            }
                                        }
                                    }
                                    catch (Exception ex) { MessageBox.Show("오류: " + ex.Message); }

                                    DisplayMessage($"[{senderId}]: 파일 도착 알림 ({content.Split(':')[1]})", false, currentTime);
                                }
                                else if (content.StartsWith("EMOJI:"))
                                {
                                    DisplayEmoji(senderId, content.Substring(6), currentTime);
                                }
                                else
                                {
                                    //DisplayMessage($"[{senderId}]: {content}", false, currentTime);
                                    DisplayMessage($"[{partnerNickname}]: {content}", false, currentTime);
                                }

                                // [1 사라짐 해결] 창이 보이고 최소화 상태가 아니면 즉시 읽음 처리
                                if (this.Visible && this.WindowState != FormWindowState.Minimized)
                                {
                                    MarkMessagesAsRead();
                                }
                            });
                        }
                    }

                    // 남은 텍스트 데이터 처리 (다운로드 모드가 아닐 때만)
                    if (!lastMessageComplete && messages.Length > 0 && !isDownloading)
                    {
                        messageBuilder.Clear();
                        messageBuilder.Append(messages[messages.Length - 1]);
                    }
                    else
                    {
                        messageBuilder.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
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

        /*
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
        */

        private void DisplayEmoji(int senderId, string emojiCode, string timeString, string readStatus = "")
        {
            string partnerNickname = GetNicknameForUser(partnerId);
            DisplayEmojiWithNickname(senderId, partnerNickname, emojiCode, timeString, readStatus);
        }

        // ✅ 새로운 메서드 추가
        private void DisplayEmojiWithNickname(int senderId, string partnerNickname, string emojiCode, string timeString, string readStatus = "")
        {
            rtbChatLog.SelectionStart = rtbChatLog.TextLength;
            rtbChatLog.SelectionLength = 0;

            // ✅ 닉네임으로 표시
            string senderLabel = senderId == myId ? "나" : partnerNickname;
            rtbChatLog.AppendText($"[{senderLabel}] ({timeString}){readStatus}: ");

            Image img = null;
            if (emojiMap.TryGetValue(emojiCode, out img) && img != null)
            {
                try
                {
                    Clipboard.SetImage(img);
                    rtbChatLog.Paste();
                }
                catch (Exception)
                {
                    rtbChatLog.AppendText($"(이모티콘:{emojiCode} - UI 삽입 실패)");
                }
            }
            else
            {
                rtbChatLog.AppendText($"(이모티콘:{emojiCode} - 이미지 로드 실패)");
            }

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
            //client?.Close();

            try
            {
                // ChatMessage 업데이트
                string queryChatMessage = $@"
            UPDATE ChatMessage 
            SET IsRead = 1 
            WHERE FromUserId = {partnerId} 
            AND ToUserId = {myId} 
            AND IsRead = 0";

                int updatedCount = DBconnector.GetInstance().NonQuery(queryChatMessage);

                // RecentChat 업데이트
                string queryRecentChat = $@"
            UPDATE RecentChat
            SET UnreadCount = 0
            WHERE UserId = {myId}
            AND PartnerUserId = {partnerId}";

                DBconnector.GetInstance().NonQuery(queryRecentChat);

                // READ_CONFIRM 전송
                if (updatedCount > 0 && client != null && client.Connected)
                {
                    string confirmMsg = $"READ_CONFIRM:{myId}:{partnerId}::";
                    byte[] data = Encoding.UTF8.GetBytes(confirmMsg);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    Thread.Sleep(500);
                }
            }
            catch { }
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

                /*
                if (updatedCount > 0)
                {
                    // ✅ 0.5초 후에 READ_CONFIRM 전송
                    Task.Delay(500).ContinueWith(_ =>
                    {
                        SendReadConfirm();
                    });
                }
                */

                if (updatedCount > 0)
                {
                    string updateRecent = $@"
                        UPDATE RecentChat
                        SET UnreadCount = 0
                        WHERE UserId = {myId}
                        AND PartnerUserId = {partnerId}";

                    DBconnector.GetInstance().NonQuery(updateRecent);
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

        // ThemeManager.ThemeChanged에서 호출되는 핸들러
        private void OnThemeChanged(ThemeMode mode)
        {
            if (mode == ThemeMode.Dark)
            {
                this.ApplyDarkTheme();
            }
            else
            {
                this.ApplyChatFormUIHelper();
            }
        }

        // 다크 모드 스타일 적용
        private void ApplyDarkTheme()
        {
            // 폼 배경
            this.BackColor = Color.FromArgb(32, 32, 32);

            // RichTextBox (채팅 로그)
            rtbChatLog.BackColor = Color.FromArgb(30, 30, 30);
            rtbChatLog.ForeColor = Color.White;

            // TextBox들 (입력창, 검색창)
            txtInput.BackColor = Color.FromArgb(45, 45, 45);
            txtInput.ForeColor = Color.White;
            txtSearch.BackColor = Color.FromArgb(45, 45, 45);
            txtSearch.ForeColor = Color.White;

            // 버튼들
            StyleDarkButton(btnSend);
            StyleDarkButton(btnSearch);
            StyleDarkButton(btnSendFile);
            StyleDarkButton(btnEmojiSmiley);
            StyleDarkButton(btnEmojiCrying);
            StyleDarkButton(btnEmojiHeart);
        }

        // ChatFormUIHelper 스타일 적용 (라이트 모드)
        private void ApplyChatFormUIHelper()
        {
            ChatFormUIHelper.ApplyDisplayStyle(this);
            ChatFormUIHelper.ApplyLightestStyle(rtbChatLog);
            ChatFormUIHelper.ApplyInputStyle(txtInput);
            ChatFormUIHelper.ApplyInputStyle(txtSearch);
            ChatFormUIHelper.ApplyButtonStyle(btnSend);
            ChatFormUIHelper.ApplyButtonStyle(btnSearch);
            ChatFormUIHelper.ApplyButtonStyle(btnSendFile);
            ChatFormUIHelper.ApplyButtonStyle(btnEmojiSmiley);
            ChatFormUIHelper.ApplyButtonStyle(btnEmojiCrying);
            ChatFormUIHelper.ApplyButtonStyle(btnEmojiHeart);
        }

        // 다크 모드 버튼 스타일
        private void StyleDarkButton(Button button)
        {
            button.BackColor = Color.FromArgb(80, 100, 90);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
        }
    }
}