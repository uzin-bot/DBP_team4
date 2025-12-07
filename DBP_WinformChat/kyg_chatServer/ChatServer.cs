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
    // [UserID, TcpClient 객체] 맵
    private static Dictionary<string, List<TcpClient>> clients = new Dictionary<string, List<TcpClient>>();
    private const int PORT = 12345;

    // 파일 저장 경로 (실행 파일 위치 기준)
    private static readonly string FILE_STORAGE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChatFiles");

    public static void StartServer()
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (!Directory.Exists(FILE_STORAGE_PATH))
        {
            Directory.CreateDirectory(FILE_STORAGE_PATH);
        }

        try
        {
            listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            Console.WriteLine($"[Server] TCP Server Started on Port {PORT}...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine($"[Server] New client connected: {client.Client.RemoteEndPoint}");

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

    /*
    private static void HandleClient(TcpClient tcpClient)
    {
        NetworkStream stream = tcpClient.GetStream();
        byte[] buffer = new byte[8192]; // 버퍼 크기 증가 (대용량 파일 대비)
        string userId = string.Empty;

        // 파일 업로드(수신) 상태 변수
        bool isReceivingFile = false;
        long fileSize = 0;
        string fileName = string.Empty;
        string receiverId = string.Empty;
        string senderId = string.Empty;
        string fullPath = string.Empty;

        FileStream fileStream = null;
        long remainingBytes = 0;

        // 패킷 조립용 빌더 (Sticky Packet 해결용)
        StringBuilder messageBuilder = new StringBuilder();

        try
        {
            while (tcpClient.Connected)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                // [1] 파일 데이터 수신 모드 (클라이언트 -> 서버 업로드)
                if (isReceivingFile)
                {
                    if (fileStream == null)
                    {
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
                        fileStream.Dispose();
                        fileStream = null;
                        isReceivingFile = false;

                        string fileNotifyContent = $"FILE_RECEIVED:{fileName}:{fullPath}";
                        string fileNotifyMsg = $"CHAT:{senderId}:{receiverId}:{fileNotifyContent}";

                        SendMessageToClient(receiverId, fileNotifyMsg);
                        SaveChatMessageAndRecentChat(senderId, receiverId, $"[파일 전송 완료] {fileName}", true, fullPath);

                        Console.WriteLine($"[File Success] {fileName} saved at {fullPath}");
                    }
                    continue;
                }

                // [2] 텍스트 명령어 처리 모드
                // 받은 데이터를 문자열로 변환하여 빌더에 추가
                string receivedChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                messageBuilder.Append(receivedChunk);
                string fullMessage = messageBuilder.ToString();

                // \0 기준으로 메시지 분리 (Sticky Packet 해결)
                string[] messages = fullMessage.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

                // 마지막 패킷이 \0로 끝났는지 확인
                bool lastMessageComplete = fullMessage.EndsWith("\0");
                int messagesToProcess = lastMessageComplete ? messages.Length : messages.Length - 1;

                for (int i = 0; i < messagesToProcess; i++)
                {
                    string msg = messages[i].Trim();
                    if (string.IsNullOrEmpty(msg)) continue;

                    // 2-1. 파일 업로드 헤더 감지
                    if (msg.StartsWith("FILE_HEADER:"))
                    {
                        string[] fh = msg.Split(new char[] { ':' }, 5);
                        if (fh.Length == 5)
                        {
                            senderId = fh[1];
                            receiverId = fh[2];
                            fileName = fh[3];
                            if (long.TryParse(fh[4], out fileSize))
                            {
                                isReceivingFile = true;
                                messageBuilder.Clear(); // 파일 모드로 전환 전 빌더 초기화
                                Console.WriteLine($"[FileHeader] Upload Start: {fileName}");
                                break; // 루프 탈출 후 다음 Read부터 파일 데이터로 처리
                            }
                        }
                    }

                    // 2-2. 일반 명령어 파싱
                    string[] parts = msg.Split(new char[] { ':' }, 4);
                    if (parts.Length < 1) continue;

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

                        if (senderId != receiverId)
                        {
                            SendMessageToClient(receiverId, msg);
                        }
                        SaveChatMessageAndRecentChat(senderId, receiverId, content);
                        Console.WriteLine($"[Chat] {senderId} -> {receiverId}: {content}");
                    }
                    else if (type == "READ_CONFIRM" && parts.Length >= 3)
                    {
                        string readerId = parts[1];
                        string originalSenderId = parts[2];
                        Console.WriteLine($"[Server] Read Confirm: {readerId} read {originalSenderId}'s message");

                        string confirmMsg = $"READ_CONFIRM:{readerId}:{originalSenderId}:";
                        SendMessageToClient(originalSenderId, confirmMsg);
                    }
                    // ▼▼▼ [추가됨] 파일 다운로드 요청 처리 (서버 -> 클라이언트 전송) ▼▼▼
                    else if (type == "FILE_DOWNLOAD_REQ")
                    {
                        string requesterId = parts.Length > 1 ? parts[1] : "Unknown";
                        string targetFileName = parts.Length > 2 ? parts[2] : "Unknown";

                        string userFileDir = Path.Combine(FILE_STORAGE_PATH, requesterId);
                        string targetFilePath = Path.Combine(userFileDir, targetFileName);

                        if (File.Exists(targetFilePath))
                        {
                            try
                            {
                                long len = new FileInfo(targetFilePath).Length;

                                // 헤더 전송
                                string header = $"FILE_RESP:SERVER:{requesterId}:{targetFileName}:{len}";
                                byte[] headerBytes = Encoding.UTF8.GetBytes(header);

                                lock (tcpClient)
                                {
                                    stream.Write(headerBytes, 0, headerBytes.Length);
                                    stream.Flush();

                                    // ★★★ [ZIP 파일 문제 해결 핵심] ★★★
                                    // 헤더와 파일 데이터가 붙어서 0KB로 인식되는 것을 방지하기 위해 1초 대기
                                    Thread.Sleep(1000);

                                    // 파일 데이터 전송
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
                    }
                    // ▲▲▲ [추가 끝] ▲▲▲
                }

                // 처리하고 남은 데이터 정리
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
    */

    private static void HandleClient(TcpClient tcpClient)
    {
        NetworkStream stream = tcpClient.GetStream();
        byte[] buffer = new byte[8192]; // 버퍼 8KB
        string userId = string.Empty;

        // 파일 전송 관련 변수
        bool isReceivingFile = false;
        long fileSize = 0;
        string fileName = string.Empty;
        string receiverId = string.Empty;
        string senderId = string.Empty;
        string fullPath = string.Empty;

        FileStream fileStream = null;
        long remainingBytes = 0;

        // 패킷 뭉침(Sticky Packet) 해결용 빌더
        StringBuilder messageBuilder = new StringBuilder();

        try
        {
            while (tcpClient.Connected)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                // =========================================================
                // [Mode 1] 파일 업로드 수신 (클라이언트 -> 서버)
                // =========================================================
                if (isReceivingFile)
                {
                    if (fileStream == null)
                    {
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
                        fileStream.Dispose();
                        fileStream = null;
                        isReceivingFile = false;

                        string fileNotifyContent = $"FILE_RECEIVED:{fileName}:{fullPath}";
                        string fileNotifyMsg = $"CHAT:{senderId}:{receiverId}:{fileNotifyContent}";

                        SendMessageToClient(receiverId, fileNotifyMsg);
                        SaveChatMessageAndRecentChat(senderId, receiverId, $"[파일 전송 완료] {fileName}", true, fullPath);
                        Console.WriteLine($"[File Success] {fileName} saved at {fullPath}");
                    }
                    continue; // 파일 데이터 처리 후 루프 건너뜀
                }

                // =========================================================
                // [Mode 2] 텍스트 명령어 처리
                // =========================================================
                string receivedChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                messageBuilder.Append(receivedChunk);
                string fullMessage = messageBuilder.ToString();

                // \0 기준으로 메시지 분리 (Sticky Packet 해결)
                string[] messages = fullMessage.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

                bool lastMessageComplete = fullMessage.EndsWith("\0");
                int messagesToProcess = lastMessageComplete ? messages.Length : messages.Length - 1;

                for (int i = 0; i < messagesToProcess; i++)
                {
                    string msg = messages[i].Trim();
                    if (string.IsNullOrEmpty(msg)) continue;

                    // 2-1. 파일 업로드 헤더 감지
                    if (msg.StartsWith("FILE_HEADER:"))
                    {
                        string[] fh = msg.Split(new char[] { ':' }, 5);
                        if (fh.Length == 5)
                        {
                            senderId = fh[1];
                            receiverId = fh[2];
                            fileName = fh[3];
                            if (long.TryParse(fh[4], out fileSize))
                            {
                                isReceivingFile = true;
                                messageBuilder.Clear(); // 파일 모드 전환 전 텍스트 버퍼 초기화
                                Console.WriteLine($"[FileHeader] Upload Start: {fileName}");
                                break; // 즉시 루프 탈출 -> 다음 Read부터 파일 데이터로 처리
                            }
                        }
                    }

                    // 2-2. 일반 명령어 파싱
                    string[] parts = msg.Split(new char[] { ':' }, 4);
                    if (parts.Length < 1) continue;
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

                        if (senderId != receiverId) SendMessageToClient(receiverId, msg);
                        SaveChatMessageAndRecentChat(senderId, receiverId, content);
                        Console.WriteLine($"[Chat] {senderId} -> {receiverId}: {content}");
                    }
                    else if (type == "READ_CONFIRM" && parts.Length >= 3)
                    {
                        string readerId = parts[1];
                        string originalSenderId = parts[2];
                        Console.WriteLine($"[Server] Read Confirm: {readerId} read {originalSenderId}'s message");

                        string confirmMsg = $"READ_CONFIRM:{readerId}:{originalSenderId}:";
                        SendMessageToClient(originalSenderId, confirmMsg);
                    }
                    // ▼▼▼ [다운로드 로직] ZIP 파일 0KB 방지 핵심 ▼▼▼
                    else if (type == "FILE_DOWNLOAD_REQ")
                    {
                        string requesterId = parts.Length > 1 ? parts[1] : "Unknown";
                        string targetFileName = parts.Length > 2 ? parts[2] : "Unknown";

                        string userFileDir = Path.Combine(FILE_STORAGE_PATH, requesterId);
                        string targetFilePath = Path.Combine(userFileDir, targetFileName);

                        if (File.Exists(targetFilePath))
                        {
                            try
                            {
                                long len = new FileInfo(targetFilePath).Length;

                                // 헤더 뒤에 반드시 \0 추가
                                string header = $"FILE_RESP:SERVER:{requesterId}:{targetFileName}:{len}\0";
                                byte[] headerBytes = Encoding.UTF8.GetBytes(header);

                                lock (tcpClient)
                                {
                                    stream.Write(headerBytes, 0, headerBytes.Length);
                                    stream.Flush();

                                    // 헤더와 파일 내용 섞임 방지 (1초 대기)
                                    Thread.Sleep(1000);

                                    byte[] fileData = File.ReadAllBytes(targetFilePath);
                                    stream.Write(fileData, 0, fileData.Length);
                                    stream.Flush();
                                }
                                Console.WriteLine($"[Server] Sent file '{targetFileName}' to {requesterId}");
                            }
                            catch (Exception ex) { Console.WriteLine($"[Error] File send failed: {ex.Message}"); }
                        }
                    }
                }

                // 남은 텍스트 데이터 보관
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

    private static void SendMessageToClient(string receiverId, string message)
    {
        lock (clients)
        {
            if (!clients.ContainsKey(receiverId)) return;

            List<TcpClient> userClients = clients[receiverId];
            List<TcpClient> deadClients = new List<TcpClient>();

            foreach (TcpClient receiverClient in userClients)
            {
                try
                {
                    if (!receiverClient.Connected)
                    {
                        deadClients.Add(receiverClient);
                        continue;
                    }

                    NetworkStream receiverStream = receiverClient.GetStream();
                    // 반드시 \0을 붙여서 전송 (클라이언트의 Split을 위해)
                    byte[] data = Encoding.UTF8.GetBytes(message + "\0");
                    receiverStream.Write(data, 0, data.Length);
                    receiverStream.Flush();
                }
                catch
                {
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
            }
        }
    }

    private static void SaveChatMessageAndRecentChat(string senderId, string receiverId, string content, bool isFile = false, string filePath = null)
    {
        try
        {
            string filePathValue = filePath != null ? $"'{filePath.Replace("\\", "\\\\")}'" : "NULL"; // 경로 이스케이프 처리

            string query = $@"
            INSERT INTO ChatMessage (FromUserId, ToUserId, Content, SentAt, IsRead, IsFile, FilePath)
            VALUES ({senderId}, {receiverId}, '{content}', NOW(), 0, {(isFile ? 1 : 0)}, {filePathValue});
            SELECT LAST_INSERT_ID();";

            DataTable dt = DBconnector.GetInstance().Query(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                int messageId = Convert.ToInt32(dt.Rows[0][0]);
                UpdateRecentChat(senderId, receiverId, messageId, incrementUnread: false);
                UpdateRecentChat(receiverId, senderId, messageId, incrementUnread: true);
            }
        }
        catch (Exception dbEx)
        {
            Console.WriteLine($"[DB ERROR] {dbEx.Message}");
        }
    }

    private static void UpdateRecentChat(string userId, string partnerId, int lastMessageId, bool incrementUnread)
    {
        try
        {
            string checkQuery = $"SELECT COUNT(*) FROM RecentChat WHERE UserId = {userId} AND PartnerUserId = {partnerId}";
            DataTable dt = DBconnector.GetInstance().Query(checkQuery);
            int count = Convert.ToInt32(dt.Rows[0][0]);

            string query;
            if (count > 0)
            {
                string unreadUpdate = incrementUnread ? "UnreadCount = UnreadCount + 1" : "UnreadCount = UnreadCount";
                query = $"UPDATE RecentChat SET LastMessageId = {lastMessageId}, LastMessageAt = NOW(), {unreadUpdate} WHERE UserId = {userId} AND PartnerUserId = {partnerId}";
            }
            else
            {
                int initialUnread = incrementUnread ? 1 : 0;
                query = $"INSERT INTO RecentChat (UserId, PartnerUserId, LastMessageId, LastMessageAt, is_pinned, UnreadCount) VALUES ({userId}, {partnerId}, {lastMessageId}, NOW(), 0, {initialUnread})";
            }
            DBconnector.GetInstance().NonQuery(query);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[DB ERROR] UpdateRecentChat: " + ex.Message);
        }
    }
}
