using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System;
using MySql.Data.MySqlClient; // DB Exception 처리를 위해 필요

public class ChatServer
{
    private static TcpListener listener;
    // [UserID, TcpClient 객체] 맵: 로그인한 사용자 ID와 해당 클라이언트 연결 매핑
    private static Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
    private const int PORT = 8888;
    private static DBHelper dbHelper = new DBHelper();

    public static void StartServer()
    {
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

    private static void HandleClient(TcpClient tcpClient)
    {
        NetworkStream stream = tcpClient.GetStream();
        byte[] buffer = new byte[1024];
        string userId = string.Empty;

        try
        {
            while (tcpClient.Connected)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"[Received Raw] {receivedMessage}");

                // 메시지 포맷: [TYPE]:[SENDER_ID]:[RECEIVER_ID]:[CONTENT]
                string[] parts = receivedMessage.Split(new char[] { ':' }, 4);
                if (parts.Length < 2) continue;

                string type = parts[0];

                if (type == "LOGIN")
                {
                    userId = parts[1];
                    lock (clients)
                    {
                        clients[userId] = tcpClient;
                    }
                    Console.WriteLine($"[Server] User logged in: {userId}");
                }
                else if (type == "CHAT" && parts.Length == 4)
                {
                    string senderId = parts[1];
                    string receiverId = parts[2];
                    string content = parts[3];

                    // 수신자에게 메시지 전달 (1:1 채팅 구조)
                    if (clients.ContainsKey(receiverId))
                    {
                        TcpClient receiverClient = clients[receiverId];
                        NetworkStream receiverStream = receiverClient.GetStream();

                        byte[] data = Encoding.UTF8.GetBytes(receivedMessage);
                        receiverStream.Write(data, 0, data.Length);
                    }

                    // DB 저장 로직 (ChatServer가 직접 SQL 실행)
                    try
                    {
                        // 1. ChatMessage INSERT (메시지 저장)
                        string chatQuery = @"
                            INSERT INTO ChatMessage (SenderID, ReceiverID, Content, SendTime)
                            VALUES (@SenderID, @ReceiverID, @Content, NOW())";

                        MySqlParameter[] chatParams = new MySqlParameter[]
                        {
                            new MySqlParameter("@SenderID", senderId),
                            new MySqlParameter("@ReceiverID", receiverId),
                            new MySqlParameter("@Content", content)
                        };
                        dbHelper.ExecuteNonQuery(chatQuery, chatParams);

                        // 2. RecentChat UPDATE/INSERT (대화 목록 시간 갱신)
                        UpdateRecentChat(senderId, receiverId); // 송신자 목록 갱신
                        UpdateRecentChat(receiverId, senderId); // 수신자 목록 갱신

                        Console.WriteLine($"[DB Success] Message saved from {senderId} to {receiverId}");
                    }
                    catch (MySql.Data.MySqlClient.MySqlException sqlEx)
                    {
                        Console.WriteLine($"[DB ERROR] SQL Exception: {sqlEx.Message}. Code: {sqlEx.Number}");
                    }
                    catch (Exception dbEx)
                    {
                        Console.WriteLine($"[DB ERROR] General Exception: {dbEx.Message}");
                    }

                    Console.WriteLine($"[Chat] {senderId} -> {receiverId}: {content}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Client Error] {userId}: {ex.Message}");
        }
        finally
        {
            // 연결 해제 처리
            if (!string.IsNullOrEmpty(userId))
            {
                lock (clients)
                {
                    clients.Remove(userId);
                }
                Console.WriteLine($"[Server] User logged out: {userId}");
            }
            tcpClient?.Close();
        }
    }

    // RecentChat 테이블 업데이트/삽입 로직 (ChatServer에 구현)
    private static void UpdateRecentChat(string userId, string partnerId)
    {
        string query = @"
            INSERT INTO RecentChat (UserID, PartnerID, LastMessageTime)
            VALUES (@UserID, @PartnerID, NOW())
            ON DUPLICATE KEY UPDATE LastMessageTime = NOW()";

        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@UserID", userId),
            new MySqlParameter("@PartnerID", partnerId)
        };

        dbHelper.ExecuteNonQuery(query, parameters);
    }
}