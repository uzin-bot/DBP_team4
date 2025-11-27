using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using 남예솔;

namespace DBP_Chat
{
	public partial class SearchResultForm : Form
	{
		string connStr = "server=223.130.151.111;port=3306;database=company;uid=root;pwd=비밀번호;";

		string id, name, dept;
		Dept parentForm;   

		public SearchResultForm(string id, string name, string dept, Dept parent)
		{
			InitializeComponent();

			this.id = id;
			this.name = name;
			this.dept = dept;
			this.parentForm = parent;

			LoadResult();
		}

		//검색 결과 로딩
		private void LoadResult()
		{
			string sql =
				@"SELECT e.emp_id, e.emp_name, d.dept_name, e.status
                  FROM employee e 
                  JOIN department d ON e.dept_id = d.dept_id
                  WHERE e.visible = 1 ";

			if (id != "") sql += "AND e.emp_id = @id ";
			if (name != "") sql += "AND e.emp_name LIKE @name ";
			if (dept != "") sql += "AND d.dept_name = @dept ";

			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();
				MySqlCommand cmd = new MySqlCommand(sql, conn);

				if (id != "") cmd.Parameters.AddWithValue("@id", id);
				if (name != "") cmd.Parameters.AddWithValue("@name", "%" + name + "%");
				if (dept != "") cmd.Parameters.AddWithValue("@dept", dept);

				MySqlDataReader dr = cmd.ExecuteReader();
				lvResult.Items.Clear();

				while (dr.Read())
				{
					ListViewItem item = new ListViewItem(dr["emp_id"].ToString());
					item.SubItems.Add(dr["emp_name"].ToString());
					item.SubItems.Add(dr["dept_name"].ToString());
					item.SubItems.Add(dr["status"].ToString());

					lvResult.Items.Add(item);
				}
			}
		}

		//더블클릭 → 채팅하기
		private void lvResult_DoubleClick(object sender, EventArgs e)
		{
			if (lvResult.SelectedItems.Count == 0) return;

			string targetId = lvResult.SelectedItems[0].Text;

			new ChatForm(targetId).Show();
		}

		//즐겨찾기 추가
		private void btnAddFavorite_Click(object sender, EventArgs e)
		{
			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();

				foreach (ListViewItem item in lvResult.Items)
				{
					if (item.Checked)
					{
						string empId = item.Text;

						string checkSql = "SELECT COUNT(*) FROM favorite WHERE emp_id=@emp";
						MySqlCommand check = new MySqlCommand(checkSql, conn);
						check.Parameters.AddWithValue("@emp", empId);

						if (Convert.ToInt32(check.ExecuteScalar()) > 0)
							continue;

						string sql = "INSERT INTO favorite (emp_id) VALUES (@emp)";
						MySqlCommand cmd = new MySqlCommand(sql, conn);
						cmd.Parameters.AddWithValue("@emp", empId);
						cmd.ExecuteNonQuery();
					}
				}
			}

			MessageBox.Show("즐겨찾기에 추가되었습니다!");

			//DeptForm 즐겨찾기 자동 새로고침
			if (parentForm != null)
				parentForm.RefreshFavorites();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
