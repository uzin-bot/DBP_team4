using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace leehaeun
{
    internal class DBManager
    {
        private DBManager() { }

        public static DBManager Instance = new DBManager();

        private string _connectionStr = "";

        public void InitServer()
        {
            string server = "223.130.151.111";
            int port = 3306;
            string user = "s5763561";

            _connectionStr = $"Server={server};Port={port};Database={user};" +
                $"User Id={user};Password={user};";
        }

        public bool Connection()
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionStr))
                {
                    conn.Open();
                }
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
        }

        public bool Query(string query)
        {
            using (var conn = new MySqlConnection(_connectionStr))
            {
                conn.Open();
                var cmd = new MySqlCommand(query, conn);
                var result = Convert.ToInt32(cmd.ExecuteScalar());
                bool exists = (result == 1);
                return exists;
            }
        }

        public bool NonQuery(string query)
        {
            using (var conn = new MySqlConnection(_connectionStr))
            {
                conn.Open();
                var cmd = new MySqlCommand(query, conn);
                int result = cmd.ExecuteNonQuery();
                bool success = (result > 0);
                return success;
            }
        }
    }
}