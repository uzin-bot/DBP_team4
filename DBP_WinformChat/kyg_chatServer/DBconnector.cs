using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kyg_chatServer
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

        // read  쿼리
        public DataTable Query(string sql)
        {
            using (var conn = new MySqlConnection(_connectionStr))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);

                var reader = cmd.ExecuteReader();

                var table = new DataTable();
                table.Load(reader);

                return table;
            }
        }

        public int NonQuery(string sql)
        {
            using (var conn = new MySqlConnection(_connectionStr))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);

                return cmd.ExecuteNonQuery();
            }
        }

    }
}
