using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DBConnector
{
    class DBconnector
    {

        private string _connectionStr;
        private static DBconnector dbconnectorObject;

        // 생성자: 외부에서 new 못 하게 private
        private DBconnector()
        {
            string host = "223.130.151.111";
            int port = 3306;
            string username = "s5840357";
            string password = "s5840357";

            _connectionStr =
                $"Server={host};Port={port};Database={username};User Id={username};Password={password};";
        }

        // 싱글톤 인스턴스 가져오기
        public static DBconnector GetInstance()
        {
            if (dbconnectorObject == null)
            {
                dbconnectorObject = new DBconnector();
            }

            return dbconnectorObject;
        }

        // DB 연결 테스트용
        public bool ConnectionTest()
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionStr))
                {
                    conn.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                // 디버깅용으로 오류 메시지 남기기
                Debug.WriteLine("[DB] ConnectionTest 실패: " + ex.Message);
                return false;
            }
        }


        /// SELECT용 쿼리 (읽기)
        /// 사용 예시:
        ///   var dt = DBconnector.GetInstance().Query("SELECT * FROM User");
        ///   var dt = DBconnector.GetInstance().Query(
        ///       "SELECT * FROM User WHERE LoginId = @id",
        ///       new MySqlParameter("@id", loginId));
        public DataTable Query(string sql, params MySqlParameter[] parameters)
        {
            using (var conn = new MySqlConnection(_connectionStr))
            {
                conn.Open();

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        var table = new DataTable();
                        table.Load(reader);
                        return table;
                    }
                }
            }
        }


        /// INSERT / UPDATE / DELETE 같은 non-SELECT 쿼리
        /// 리턴값: 영향 받은 행 수
        /// 사용 예시:
        ///   int rows = DBconnector.GetInstance().NonQuery(
        ///       "INSERT INTO Department (DeptName) VALUES (@name)",
        ///       new MySqlParameter("@name", "개발부서"));
        public int NonQuery(string sql, params MySqlParameter[] parameters)
        {
            using (var conn = new MySqlConnection(_connectionStr))
            {
                conn.Open();

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
