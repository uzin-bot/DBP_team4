using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

public class DBHelper
{
    // 제시된 DB 연결 문자열 사용
    private readonly string connectionString = "Server=223.130.151.111;Database=s5762981;Uid=s5762981;Pwd=s5762981;";

    // DB 연결 객체 반환
    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }

    // SELECT 쿼리 실행 (DataTable 반환)
    public DataTable ExecuteSelect(string query, MySqlParameter[] parameters = null)
    {
        DataTable dt = new DataTable();
        using (var connection = GetConnection())
        {
            connection.Open();
            using (var cmd = new MySqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        return dt;
    }

    // INSERT, UPDATE, DELETE 쿼리 실행 (영향 받은 행 수 반환)
    public int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            using (var cmd = new MySqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteNonQuery();
            }
        }
    }

    /*

    // 5-A 검정: 채팅 메시지 저장 및 6-A 대화목록 업데이트
    public int InsertChatMessage(string senderId, string receiverId, string content)
    {
        // 1. ChatMessage에 메시지 INSERT
        string query = @"
            INSERT INTO ChatMessage (SenderID, ReceiverID, Content, SendTime)
            VALUES (@SenderID, @ReceiverID, @Content, NOW())"; // MySQL의 NOW() 사용

        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@SenderID", senderId),
            new MySqlParameter("@ReceiverID", receiverId),
            new MySqlParameter("@Content", content)
        };

        int result = ExecuteNonQuery(query, parameters);

        // 2. RecentChat 업데이트/삽입 (대화목록 갱신)
        UpdateRecentChat(senderId, receiverId);
        UpdateRecentChat(receiverId, senderId); // 상대방의 RecentChat도 업데이트

        return result;
    }



    // RecentChat 테이블 업데이트/삽입 로직
    private void UpdateRecentChat(string userId, string partnerId)
    {
        // MySQL의 INSERT ON DUPLICATE KEY UPDATE 문을 사용하여 레코드가 있으면 UPDATE, 없으면 INSERT
        string query = @"
            INSERT INTO RecentChat (UserID, PartnerID, LastMessageTime)
            VALUES (@UserID, @PartnerID, NOW())
            ON DUPLICATE KEY UPDATE LastMessageTime = NOW()";

        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@UserID", userId),
            new MySqlParameter("@PartnerID", partnerId)
        };

        ExecuteNonQuery(query, parameters);
    }

    public DataTable GetChatHistory(string user1Id, string user2Id)
    {
        // 두 사용자 ID 사이의 모든 메시지를 시간 순으로 조회합니다.
        string query = @"
        SELECT 
            SenderID, Content, SendTime
        FROM ChatMessage
        WHERE (SenderID = @user1Id AND ReceiverID = @user2Id) 
           OR (SenderID = @user2Id AND ReceiverID = @user1Id)
        ORDER BY SendTime ASC"; // 오래된 것부터 최신 순으로 정렬

        MySqlParameter[] parameters = new MySqlParameter[]
        {
        // MySql은 순서와 관계없이 이름 기반으로 매개변수를 매칭합니다.
        new MySqlParameter("@user1Id", user1Id),
        new MySqlParameter("@user2Id", user2Id)
        };

        return ExecuteSelect(query, parameters);


    }

    */
}