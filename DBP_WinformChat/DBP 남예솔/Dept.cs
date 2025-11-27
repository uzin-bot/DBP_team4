using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using 남예솔;

namespace DBP_Chat
{
	public partial class Dept : Form
	{
		string connStr = "server=223.130.151.111;port=3306;database=company;uid=root;pwd=비밀번호;";

		public Dept()
		{
			InitializeComponent();

			this.Load += Dept_Load;

			// 직원 더블클릭 -> 채팅
			tvdept.NodeMouseDoubleClick += tvdept_NodeMouseDoubleClick;

			tvdept.AfterSelect += tvdept_AfterSelect;

			btnsearch.Click += btnsearch_Click;
			btnadd.Click += btnadd_Click;
			btndelete.Click += btndelete_Click;
			btnChat.Click += btnChat_Click;
		}

		private void Dept_Load(object sender, EventArgs e)
		{
			LoadTreeView();
			LoadFavoriteList();
		}

		// SearchResultForm이 호출할 수 있는 함수
		public void RefreshFavorites()
		{
			LoadFavoriteList();
		}

		// 회사 → 부서 → 직원 로드
		private void LoadTreeView()
		{
			tvdept.Nodes.Clear();
			TreeNode companyNode = new TreeNode("회사");
			tvdept.Nodes.Add(companyNode);

			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();

				string sqlDept = "SELECT dept_id, dept_name FROM department";
				MySqlCommand cmdDept = new MySqlCommand(sqlDept, conn);
				MySqlDataReader drDept = cmdDept.ExecuteReader();
				DataTable dtDept = new DataTable();
				dtDept.Load(drDept);

				foreach (DataRow dept in dtDept.Rows)
				{
					TreeNode deptNode = new TreeNode(dept["dept_name"].ToString());
					deptNode.Tag = dept["dept_id"];
					companyNode.Nodes.Add(deptNode);

					string sqlEmp = "SELECT emp_id, emp_name, status FROM employee WHERE dept_id = @dept_id";

					MySqlCommand cmdEmp = new MySqlCommand(sqlEmp, conn);
					cmdEmp.Parameters.AddWithValue("@dept_id", dept["dept_id"]);
					MySqlDataReader drEmp = cmdEmp.ExecuteReader();

					while (drEmp.Read())
					{
						//ID + 이름 + 상태를 TreeView에 표시하도록 변경
						string text = $"({drEmp["emp_id"]}) {drEmp["emp_name"]} ({drEmp["status"]})";

						TreeNode empNode = new TreeNode(text);
						empNode.Tag = drEmp["emp_id"];   
						deptNode.Nodes.Add(empNode);
					}
					drEmp.Close();
				}
			}

			tvdept.ExpandAll();
		}

		private void tvdept_AfterSelect(object sender, TreeViewEventArgs e)
		{
			
		}

		//직원 더블클릭 → 채팅창 열기
		private void tvdept_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Level != 2) return;

			string empId = e.Node.Tag.ToString();
			new ChatForm(empId).Show();
		}

		//검색 버튼 클릭시 검색창 열기
		private void btnsearch_Click(object sender, EventArgs e)
		{
			string id = txtID.Text.Trim();
			string name = txtname.Text.Trim();
			string dept = txtdept.Text.Trim();

			SearchResultForm s = new SearchResultForm(id, name, dept, this);
			s.Show();
		}

		//즐겨찾기 리스트 로딩
		private void LoadFavoriteList()
		{
			lBlist.Items.Clear();

			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();
				string sql =
					@"SELECT e.emp_id, e.emp_name, e.status
                      FROM employee e
                      JOIN favorite f ON e.emp_id = f.emp_id";

				MySqlCommand cmd = new MySqlCommand(sql, conn);
				MySqlDataReader dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					lBlist.Items.Add($"{dr["emp_id"]} - {dr["emp_name"]} ({dr["status"]})");
				}
			}
		}

		//즐겨찾기 추가
		private void btnadd_Click(object sender, EventArgs e)
		{
			string empId = txtID.Text.Trim();

			if (empId == "")
			{
				MessageBox.Show("직원을 선택하세요!");
				return;
			}

			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();
				string checkSql = "SELECT COUNT(*) FROM favorite WHERE emp_id=@emp";
				MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
				checkCmd.Parameters.AddWithValue("@emp", empId);

				if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
				{
					MessageBox.Show("이미 즐겨찾기에 등록되어 있습니다!");
					return;
				}

				string sql = "INSERT INTO favorite (emp_id) VALUES (@emp)";
				MySqlCommand cmd = new MySqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@emp", empId);
				cmd.ExecuteNonQuery();
			}

			MessageBox.Show("즐겨찾기에 추가되었습니다!");
			LoadFavoriteList();
		}

		//즐겨찾기 삭제
		private void btndelete_Click(object sender, EventArgs e)
		{
			if (lBlist.SelectedItem == null)
			{
				MessageBox.Show("삭제할 대상을 선택하세요!");
				return;
			}

			string empId = lBlist.SelectedItem.ToString().Split('-')[0].Trim();

			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();
				string sql = "DELETE FROM favorite WHERE emp_id=@emp";
				MySqlCommand cmd = new MySqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@emp", empId);
				cmd.ExecuteNonQuery();
			}

			MessageBox.Show("삭제되었습니다!");
			LoadFavoriteList();
		}

		//즐겨찾기 → 채팅하기
		private void btnChat_Click(object sender, EventArgs e)
		{
			if (lBlist.SelectedItem == null)
			{
				MessageBox.Show("대화할 대상을 선택하세요!");
				return;
			}

			string empId = lBlist.SelectedItem.ToString().Split('-')[0].Trim();
			new ChatForm(empId).Show();
		}
	}
}
