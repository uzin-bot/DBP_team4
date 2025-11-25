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

		private void chatlist_Load(object sender, EventArgs e)
		{
			LoadRecentChat();
		}

		//RecentChat + pinned_chat 반영
		private void LoadRecentChat()
		{
			lvlist.Items.Clear();

			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();

				string sql = @"
                    SELECT 
                        r.emp_id,
                        e.emp_name,
                        d.dept_name,
                        r.time,
                        CASE WHEN p.emp_id IS NOT NULL THEN 1 ELSE 0 END AS is_pinned
                    FROM RecentChat r
                    JOIN employee e ON r.emp_id = e.emp_id
                    JOIN department d ON e.dept_id = d.dept_id
                    LEFT JOIN pinned_chat p ON r.emp_id = p.emp_id
                    ORDER BY is_pinned DESC, r.time DESC;
                ";

				MySqlCommand cmd = new MySqlCommand(sql, conn);
				MySqlDataReader dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					bool isPinned = dr.GetInt32("is_pinned") == 1;

					ListViewItem item = new ListViewItem();
					item.ImageIndex = isPinned ? 0 : -1;

					item.SubItems.Add(dr["emp_id"].ToString());
					item.SubItems.Add(dr["emp_name"].ToString());
					item.SubItems.Add(dr["dept_name"].ToString());
					item.SubItems.Add(dr["time"].ToString());

					lvlist.Items.Add(item);
				}
			}
		}

		//우클릭 항목 자동 선택
		private void lvlist_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ListViewItem item = lvlist.GetItemAt(e.X, e.Y);
				if (item != null)
				{
					item.Selected = true;
				}
			}
		}

		//더블 클릭  → 채팅창 열기
		private void lvlist_DoubleClick(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

			string targetID = lvlist.SelectedItems[0].SubItems[1].Text;

			new ChatForm(targetID).Show();
		}

		//채팅방 고정 추가
		private void PinChat(string empId)
		{
			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();

				string check = "SELECT COUNT(*) FROM pinned_chat WHERE emp_id=@id";
				MySqlCommand c = new MySqlCommand(check, conn);
				c.Parameters.AddWithValue("@id", empId);

				if (Convert.ToInt32(c.ExecuteScalar()) > 0)
					return;

				string insert = "INSERT INTO pinned_chat(emp_id) VALUES(@id)";
				MySqlCommand cmd = new MySqlCommand(insert, conn);
				cmd.Parameters.AddWithValue("@id", empId);
				cmd.ExecuteNonQuery();
			}
		}

		//고정 해제
		private void UnpinChat(string empId)
		{
			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();

				string sql = "DELETE FROM pinned_chat WHERE emp_id=@id";
				MySqlCommand cmd = new MySqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@id", empId);
				cmd.ExecuteNonQuery();
			}
		}

		//우클릭 메뉴 → 고정하기
		private void addpin_Click(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

			string empId = lvlist.SelectedItems[0].SubItems[1].Text;

			PinChat(empId);
			LoadRecentChat();
		}

		//우클릭 메뉴 → 고정 해제
		private void deletepin_Click(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

			string empId = lvlist.SelectedItems[0].SubItems[1].Text;

			UnpinChat(empId);
			LoadRecentChat();
		}
	}
}
