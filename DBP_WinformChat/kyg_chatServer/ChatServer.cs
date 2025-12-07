using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using MySqlConnector;
using kyg_chatServer;
using System.Data;
using System.Data.Common;

public class kyg
{
    private static TcpListener listener;
    // [UserID, TcpClient 객체] 맵: 로그인한 사용자 ID와 해당 클라이언트 연결 매핑
    private static Dictionary<string, List<TcpClient>> clients = new Dictionary<string, List<TcpClient>>();
    private const int PORT = 12345;
    // 서버 측 파일 저장 디렉토리 (4주차 5-F 검정)
    private const string FILE_STORAGE_PATH = "C:\\DBP_ChatFiles\\";

    public static void StartServer()
    {
        // ✅ 콘솔 인코딩 설정 추가
        Console.OutputEncoding = Encoding.UTF8;

        // 파일 저장 경로가 없으면 생성
        if (!Directory.Exists(FILE_STORAGE_PATH))
        {
            Directory.CreateDirectory(FILE_STORAGE_PATH);
        }

        try
        {
            listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start(); // TCP 리스너 시작
            Console.WriteLine($"[Server] TCP Server Started on Port {PORT}...");

            while (true)
            {
                // 클라이언트 연결 요청이 들어올 때까지 대기(블로킹)
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine($"[Server] New client connected: {client.Client.RemoteEndPoint}");

                // 새 연결은 별도 스레드에서 처리
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.IsBackground = true;
                clientThread.Start();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Server Error] {ex.Message}");
        }
        finally
        {
            listener?.Stop();
        }
    }

    private static void HandleClient(TcpClient tcpClient)
    {
        NetworkStream stream = tcpClient.GetStream();
        byte[] buffer = new byte[1024];
        string userId = string.Empty;

        // 5-F: 파일 전송 상태 관리 변수
        bool isReceivingFile = false;
        long fileSize = 0;
        string fileName = string.Empty;
        string receiverId = string.Empty;
        string senderId = string.Empty;
        string fullPath = string.Empty;

        FileStream fileStream = null; // 현재 파일 스트림 객체
        long remainingBytes = 0;     // 남은 파일 크기

        try
        {
            while (tcpClient.Connected)
            {
                // 클라이언트로부터 데이터 읽기
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // 연결 종료

                if (isReceivingFile)
                {
                    // 1. 파일 데이터 수신 모드
                    if (fileStream == null)
                    {
                        // FileStream 열기 (첫 파일 데이터 Read 시점)
                        string saveDir = Path.Combine(FILE_STORAGE_PATH, receiverId);
                        Directory.CreateDirectory(saveDir);
                        fullPath = Path.Combine(saveDir, fileName);
                        fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                        remainingBytes = fileSize;
                    }

                    // 현재 버퍼의 데이터를 파일에 쓰고 남은 크기 업데이트
                    int writeSize = (int)Math.Min(bytesRead, remainingBytes);
                    fileStream.Write(buffer, 0, writeSize);
                    remainingBytes -= writeSize;

                    if (remainingBytes <= 0)
                    {
                        // 파일 전송 완료
                        fileStream.Dispose();
                        fileStream = null;
                        isReceivingFile = false;

                        // 2. 파일 전송 완료 알림 중계 및 DB 저장
                        string fileNotifyContent = $"FILE_RECEIVED:{fileName}:{fullPath}";
                        string fileNotifyMsg = $"CHAT:{senderId}:{receiverId}:{fileNotifyContent}";

                        SendMessageToClient(receiverId, fileNotifyMsg);
                        SaveChatMessageAndRecentChat(senderId, receiverId, $"[파일 전송 완료] {fileName}");

                        Console.WriteLine($"[File Success] {fileName} saved at {fullPath}");
                    }

                    continue;
                }
                else
                {
                    // 2. 텍스트 데이터 수신 모드 (LOGIN, CHAT, FILE_HEADER)
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead).TrimEnd('\0');
                    Console.WriteLine($"[Received Raw] {receivedMessage}");

                    // FILE_HEADER는 ':' 많이 포함되므로 5개로 Split
                    if (receivedMessage.StartsWith("FILE_HEADER:"))
                    {
                        string[] fh = receivedMessage.Split(new char[] { ':' }, 5);

                        if (fh.Length == 5)
                        {
                            senderId = fh[1];
                            receiverId = fh[2];
                            fileName = fh[3];

                            if (long.TryParse(fh[4], out fileSize))
                            {
                                isReceivingFile = true;
                                Console.WriteLine($"[FileHeader] {fileName} ({fileSize} bytes) From {senderId} -> {receiverId}");
                                continue;
                            }
                        }
                    }

                    string[] parts = receivedMessage.Split(new char[] { ':' }, 4);  // 중요: max 4 -> content에 ':' 포함 가능
                                                                                    //if (parts.Length < 2) continue;

                    string type = parts[0];

                    if (type == "LOGIN")
                    {
                        // 클라이언트 ID 등록
                        //userId = parts[1];
                        userId = parts.Length > 1 ? parts[1] : string.Empty;
                        lock (clients)
                        {
                            if (!clients.ContainsKey(userId))
                            {
                                clients[userId] = new List<TcpClient>();
                            }
                            clients[userId].Add(tcpClient);
                            Console.WriteLine($"[Server] User logged in: {userId} (총 연결 수: {clients[userId].Count})");
                        }
                    }
                    else if (type == "CHAT" && parts.Length >= 4)
                    {
                        // 2주차 5-A: 일반/이모티콘 메시지 중계 및 DB 저장
                        senderId = parts[1];
                        receiverId = parts[2];
                        // parts[3]에는 content 전체(예: "EMOJI:EMO1" 또는 "plain text: with colon")가 들어옵니다
                        string content = parts[3];

                        System.Threading.Thread.Sleep(1000);

                        // 중계 및 DB 저장
                        //SenderID와 ReceiverID가 다를 때만 전송(나와의 채팅 시 중복 방지)
                        if (senderId != receiverId)
                        {
                            SendMessageToClient(receiverId, receivedMessage);
                        }
                        SaveChatMessageAndRecentChat(senderId, receiverId, content);
                        Console.WriteLine($"[Chat] {senderId} -> {receiverId}: {content}");
                    }
                    // 읽음 관련 처리
                    else if (type == "READ_CONFIRM" && parts.Length >= 3)
                    {
                        // 읽음 확인 처리
                        string readerId = parts[1];   // 읽은 사람
                        string originalSenderId = parts[2];   // 원래 보낸 사람

                        System.Threading.Thread.Sleep(1000);

                        Console.WriteLine($"[Server] 읽음 확인: {readerId}가 {originalSenderId}의 메시지 읽음");

                        // 원래 보낸 사람에게 읽음 확인 전달
                        string confirmMsg = $"READ_CONFIRM:{readerId}::";
                        SendMessageToClient(originalSenderId, confirmMsg);

                        Console.WriteLine($"[Server] {originalSenderId}에게 읽음 확인 전달 완료");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Client Error] {userId}: {ex.Message}");
        }
        finally
        {
            fileStream?.Dispose();

            if (!string.IsNullOrEmpty(userId))
            {
                lock (clients)
                {
                    if (clients.ContainsKey(userId))
                    {
                        clients[userId].Remove(tcpClient);
                        int remainingConnections = clients[userId].Count;

                        if (remainingConnections == 0)
                        {
                            clients.Remove(userId);
                        }

                        Console.WriteLine($"[Server] User logged out: {userId} (남은 연결 수: {remainingConnections})");
                    }
                }
            }

            tcpClient?.Close();
        }
    }

    /*
    private static void SendMessageToClient(string receiverId, string message)
    {
        // 1:1 메시지 중계 로직
        if (clients.ContainsKey(receiverId))
        {
            TcpClient receiverClient = clients[receiverId];
            NetworkStream receiverStream = receiverClient.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(message);
            receiverStream.Write(data, 0, data.Length);
        }
    }
    */

    private static void SendMessageToClient(string receiverId, string message)
    {
        lock (clients)
        {
            if (!clients.ContainsKey(receiverId))
            {
                Console.WriteLine($"[Server] No client found for receiverId: {receiverId}");
                return;
            }

            List<TcpClient> userClients = clients[receiverId];
            List<TcpClient> deadClients = new List<TcpClient>();

            Console.WriteLine($"[Server] Sending to {receiverId} ({userClients.Count} connections)");

            foreach (TcpClient receiverClient in userClients)
            {
                try
                {
                    if (!receiverClient.Connected)
                    {
                        deadClients.Add(receiverClient);
                        Console.WriteLine($"[Server] Dead connection detected for {receiverId}");
                        continue;
                    }

                    NetworkStream receiverStream = receiverClient.GetStream();
                    byte[] data = Encoding.UTF8.GetBytes(message + "\0");
                    receiverStream.Write(data, 0, data.Length);
                    receiverStream.Flush(); // ✅ Flush 추가

                    Console.WriteLine($"[Server] Message sent successfully to {receiverId}: {message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Server] Failed to send to {receiverId}: {ex.Message}");
                    deadClients.Add(receiverClient);
                }
            }

            foreach (TcpClient deadClient in deadClients)
            {
                userClients.Remove(deadClient);
                try { deadClient?.Close(); } catch { }
            }

            if (userClients.Count == 0)
            {
                clients.Remove(receiverId);
                Console.WriteLine($"[Server] All connections closed for {receiverId}, removed from dictionary");
            }
        }
    }


    private static void SaveChatMessageAndRecentChat(string senderId, string receiverId, string content, bool isFile = false, string filePath = null)
    {
        // 2주차 5-A: 메시지 DB 저장 로직 (비즈니스 로직)
        try
        {
            string filePathValue = filePath != null ? $"'{filePath}'" : "NULL";

            // INSERT하고 바로 ID 가져오기 (한 번에!)
            string query = $@"
            INSERT INTO ChatMessage (FromUserId, ToUserId, Content, SentAt, IsRead, IsFile, FilePath)
            VALUES ({senderId}, {receiverId}, '{content}', NOW(), 0, {(isFile ? 1 : 0)}, {filePathValue});
            SELECT LAST_INSERT_ID();";

            // Query()로 실행하면 SELECT 결과를 받을 수 있음
            DataTable dt = DBconnector.GetInstance().Query(query);
            int messageId = Convert.ToInt32(dt.Rows[0][0]);


            // 2. RecentChat UPDATE (2주차 6-A 대화 목록 갱신 기반)
            // 보낸 사람: UnreadCount 증가 안 함 (읽었으니까)
            UpdateRecentChat(senderId, receiverId, messageId, incrementUnread: false);
            //받은 사람: UnreadCount 증가 (안 읽었으니까)
            UpdateRecentChat(receiverId, senderId, messageId, incrementUnread: true);
        }
        catch (Exception dbEx)
        {
            Console.WriteLine($"[DB ERROR] General Exception: {dbEx.Message}");
        }
    }

    private static void UpdateRecentChat(string userId, string partnerId, int lastMessageId, bool incrementUnread)
    {
        // 2주차 6-A: 대화 목록 시간 갱신 로직
        try
        {
            // 먼저 존재 여부 확인 (기존에는 duplicate 써서 간단하게 처리했지만 이렇게 바꿈0
            string checkQuery =
                "SELECT COUNT(*) FROM RecentChat " +
                "WHERE UserId = " + userId + " AND PartnerUserId = " + partnerId;

            DataTable dt = DBconnector.GetInstance().Query(checkQuery);
            int count = Convert.ToInt32(dt.Rows[0][0]);

            string query;
            if (count > 0)
            {
                // UPDATE(incrementUnread가 true일 때만 + 1)
                string unreadUpdate = incrementUnread ? "UnreadCount = UnreadCount + 1" : "UnreadCount = UnreadCount";

                // UPDATE (있으면 업데이트)
                query =
                    "UPDATE RecentChat " +
                    "SET LastMessageId = " + lastMessageId + ", " +
                        "LastMessageAt = NOW(), " +
                        unreadUpdate + " " +
                    "WHERE UserId = " + userId + " AND PartnerUserId = " + partnerId;
            }
            else
            {
                // INSERT (incrementUnread가 true면 1, false면 0)
                int initialUnread = incrementUnread ? 1 : 0;

                // INSERT (없으면 인서트)
                query =
                    "INSERT INTO RecentChat (UserId, PartnerUserId, LastMessageId, LastMessageAt, is_pinned, UnreadCount) " +
                    "VALUES (" + userId + ", " + partnerId + ", " + lastMessageId + ", NOW(), 0, " + initialUnread +" )";
            }

            DBconnector.GetInstance().NonQuery(query);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[DB ERROR] UpdateRecentChat: " + ex.Message);
        }
    }
}