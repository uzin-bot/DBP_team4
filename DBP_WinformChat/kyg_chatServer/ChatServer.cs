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
    //private const string FILE_STORAGE_PATH = "C:\\DBP_ChatFiles\\";
    private static readonly string FILE_STORAGE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChatFiles");


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

        // 5-F: 파일 전송 상태 관리 변수 (클라이언트 -> 서버 업로드용)
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
                    // ========================================================
                    // [모드 1] 클라이언트가 보낸 파일을 서버가 받는 중 (업로드)
                    // ========================================================
                    if (fileStream == null)
                    {
                        // 저장 경로: 실행파일위치/ChatFiles/받는사람ID/파일명
                        string saveDir = Path.Combine(FILE_STORAGE_PATH, receiverId);
                        if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);

                        fullPath = Path.Combine(saveDir, fileName);
                        fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                        remainingBytes = fileSize;
                    }

                    int writeSize = (int)Math.Min(bytesRead, remainingBytes);
                    fileStream.Write(buffer, 0, writeSize);
                    remainingBytes -= writeSize;

                    if (remainingBytes <= 0)
                    {
                        // 업로드 완료
                        fileStream.Dispose();
                        fileStream = null;
                        isReceivingFile = false;

                        // 수신자에게 "파일 왔다" 알림 전송 (경로는 서버 내부 경로지만 식별용으로 보냄)
                        string fileNotifyContent = $"FILE_RECEIVED:{fileName}:{fullPath}";
                        string fileNotifyMsg = $"CHAT:{senderId}:{receiverId}:{fileNotifyContent}";

                        SendMessageToClient(receiverId, fileNotifyMsg);
                        SaveChatMessageAndRecentChat(senderId, receiverId, $"[파일 전송 완료] {fileName}", true, fullPath);

                        Console.WriteLine($"[File Success] {fileName} saved at {fullPath}");
                    }
                    continue;
                }
                else
                {
                    // ========================================================
                    // [모드 2] 일반 텍스트 명령어 처리
                    // ========================================================
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead).TrimEnd('\0');
                    // Console.WriteLine($"[Received Raw] {receivedMessage}"); // 디버깅용 로그

                    // 1. 파일 업로드 헤더 감지
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
                                Console.WriteLine($"[FileHeader] Upload Start: {fileName} ({fileSize} bytes)");
                                continue;
                            }
                        }
                    }

                    // 2. 일반 명령어 파싱
                    string[] parts = receivedMessage.Split(new char[] { ':' }, 4);
                    string type = parts[0];

                    if (type == "LOGIN")
                    {
                        userId = parts.Length > 1 ? parts[1] : string.Empty;
                        lock (clients)
                        {
                            if (!clients.ContainsKey(userId)) clients[userId] = new List<TcpClient>();
                            clients[userId].Add(tcpClient);
                            Console.WriteLine($"[Server] User logged in: {userId}");
                        }
                    }
                    else if (type == "CHAT" && parts.Length >= 4)
                    {
                        senderId = parts[1];
                        receiverId = parts[2];
                        string content = parts[3];

                        // 중계 및 DB 저장
                        if (senderId != receiverId)
                        {
                            SendMessageToClient(receiverId, receivedMessage);
                        }
                        SaveChatMessageAndRecentChat(senderId, receiverId, content);
                        Console.WriteLine($"[Chat] {senderId} -> {receiverId}: {content}");
                    }
                    else if (type == "READ_CONFIRM" && parts.Length >= 3)
                    {
                        string readerId = parts[1];
                        string originalSenderId = parts[2];
                        Console.WriteLine($"[Server] Read Confirm: {readerId} read {originalSenderId}'s message");

                        string confirmMsg = $"READ_CONFIRM:{readerId}::";
                        SendMessageToClient(originalSenderId, confirmMsg);
                    }
                    // ========================================================
                    // [추가된 부분] 파일 다운로드 요청 처리
                    // ========================================================
                    else if (type == "FILE_DOWNLOAD_REQ")
                    {
                        // 요청 포맷: FILE_DOWNLOAD_REQ:요청자ID:파일명
                        // (클라이언트가 '예'를 눌렀을 때 보내는 메시지)
                        string requesterId = parts.Length > 1 ? parts[1] : "Unknown";
                        string targetFileName = parts.Length > 2 ? parts[2] : "Unknown";

                        // 파일 찾기: 파일은 '수신자(requesterId)'의 폴더에 저장되어 있음
                        string userFileDir = Path.Combine(FILE_STORAGE_PATH, requesterId);
                        string targetFilePath = Path.Combine(userFileDir, targetFileName);

                        if (File.Exists(targetFilePath))
                        {
                            try
                            {
                                long len = new FileInfo(targetFilePath).Length;

                                // 1. 응답 헤더 전송 (5파트로 구성하여 파싱 호환성 유지)
                                // 형식: FILE_RESP:SERVER:RequesterID:FileName:Size
                                string header = $"FILE_RESP:SERVER:{requesterId}:{targetFileName}:{len}";
                                byte[] headerBytes = Encoding.UTF8.GetBytes(header);

                                lock (tcpClient)
                                {
                                    stream.Write(headerBytes, 0, headerBytes.Length);
                                    stream.Flush();

                                    // 헤더와 파일 데이터가 붙지 않도록 잠시 대기
                                    Thread.Sleep(200);

                                    // 2. 실제 파일 데이터 전송
                                    byte[] fileData = File.ReadAllBytes(targetFilePath);
                                    stream.Write(fileData, 0, fileData.Length);
                                    stream.Flush();
                                }
                                Console.WriteLine($"[Server] Sent file '{targetFileName}' to {requesterId}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[Error] File send failed: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"[Error] File not found: {targetFilePath}");
                            // 필요하다면 에러 메시지를 클라이언트로 전송하는 로직 추가 가능
                        }
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
                        if (clients[userId].Count == 0) clients.Remove(userId);
                        Console.WriteLine($"[Server] User logged out: {userId}");
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