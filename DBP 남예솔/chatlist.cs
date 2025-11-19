using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace 남예솔
{
	public partial class chatlist : Form
	{
		string connStr = "server=223.130.151.111;port=3306;database=company;uid=root;pwd=비밀번호;";

		public chatlist()
		{
			InitializeComponent();
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			LoadRecentChat();
		}

		// RecentChat 테이블에서 현재 채팅중 리스트 불러오기
		private void LoadRecentChat()
		{
			lvlist.Items.Clear();

			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();

				string sql = @"
                    SELECT r.emp_id, e.emp_name, d.dept_name, r.time
                    FROM RecentChat r
                    JOIN employee e ON r.emp_id = e.emp_id
                    JOIN department d ON e.dept_id = d.dept_id
                    ORDER BY r.time DESC";

				MySqlCommand cmd = new MySqlCommand(sql, conn);
				MySqlDataReader dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					ListViewItem item = new ListViewItem(dr["emp_id"].ToString());
					item.SubItems.Add(dr["emp_name"].ToString());
					item.SubItems.Add(dr["dept_name"].ToString());
					item.SubItems.Add(dr["time"].ToString());

					lvlist.Items.Add(item);
				}
			}
		}

		// 더블클릭 → 채팅창 열기
		private void lvlist_DoubleClick(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

			string targetID = lvlist.SelectedItems[0].SubItems[0].Text;

			ChatForm chat = new ChatForm(targetID);
			chat.Show();
		}
	}
}
