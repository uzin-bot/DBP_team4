using DBP_WinformChat;
using kyg;
using leehaeun;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using 남예솔;

namespace DBP_Chat
{
	public partial class Dept : Form
	{
		private int currentLoginId;
		private string currentUserName;
		private string currentUserNickname;

		public Dept(int LoginId, string name, string nickname)
		{
			InitializeComponent();

			this.currentLoginId = LoginId;
			this.currentUserName = name;
			this.currentUserNickname = nickname;

			this.Load += Dept_Load;

			//TreeView 직원 더블클릭 → 프로필 폼(현재 임시로 메시지만 뜸)
			//추후 메시지는 없애고 폼 연결하면 됩니다!
			tvdept.NodeMouseDoubleClick += tvdept_NodeMouseDoubleClick;

			tvdept.AfterSelect += tvdept_AfterSelect;

			btnsearch.Click += btnsearch_Click;
			btnadd.Click += btnadd_Click;
			btndelete.Click += btndelete_Click;

			//채팅하기 버튼 (즐겨찾기 선택,  친구 목록 선택)
			btnChat.Click += btnChat_Click;

			//클릭시 채팅 목록
			btnchatlist.Click += btnchatlist_Click;

			//친구목록 / 즐겨찾기 중복 선택 방지
			//친구 목록이랑 즐겨찾기 목록에서 동시에 직원 선택 불가하도록 했습니다!
			tvdept.NodeMouseClick += tvdept_NodeMouseClick;
			lBlist.SelectedIndexChanged += lBlist_SelectedIndexChanged;
		}

		private void Dept_Load(object sender, EventArgs e)
		{
			LoadTreeView();
			LoadFavoriteList();
		}

		//채팅 리스트 (chatlist) 폼 열기
		private void btnchatlist_Click(object sender, EventArgs e)
		{
			new chatlist().Show();
		}

		//SearchResultForm 에서 호출
		public void RefreshFavorites()
		{
			LoadFavoriteList();
		}

		//회사 → 부서 → 직원 TreeView 로딩
		private void LoadTreeView()
		{
			tvdept.Nodes.Clear();
			TreeNode companyNode = new TreeNode("회사");
			tvdept.Nodes.Add(companyNode);

			string sqlDept = "SELECT DeptId, DeptName FROM Department";
			DataTable dtDept = DBconnector.GetInstance().Query(sqlDept);

			foreach (DataRow dept in dtDept.Rows)
			{
				TreeNode deptNode = new TreeNode(dept["DeptName"].ToString());
				deptNode.Tag = dept["DeptId"];
				companyNode.Nodes.Add(deptNode);

				//Treeview에서 관리자 제외 >>> 12월 2일 수정
				string sql = $@"
					SELECT LoginId, Name, Nickname 
					FROM User 
					WHERE DeptId = {dept["DeptId"]}
					  AND IsAdmin = 0";

				DataTable dtUser = DBconnector.GetInstance().Query(sql);

				foreach (DataRow user in dtUser.Rows)
				{
					string text = $"({user["LoginId"]}) {user["Name"]} ({user["Nickname"]})";

					//본인이면 (나) 붙이기 >>> 12월 2일 수정
					if (Convert.ToInt32(user["LoginId"]) == currentLoginId)
						text += " (나)";

					TreeNode userNode = new TreeNode(text);
					userNode.Tag = user["LoginId"];
					deptNode.Nodes.Add(userNode);
				}
			}

			tvdept.ExpandAll();
		}

		private void tvdept_AfterSelect(object sender, TreeViewEventArgs e)
		{

		}

		private void tvdept_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Level != 2) return;

			int targetLoginId = Convert.ToInt32(e.Node.Tag);

			var f = new leehaeun.UserInfoForm(targetLoginId);
			f.Show();
		}

		private void btnsearch_Click(object sender, EventArgs e)
		{
			string id = txtID.Text.Trim();
			string name = txtname.Text.Trim();
			string dept = txtdept.Text.Trim();

			SearchResultForm s = new SearchResultForm(id, name, dept, currentLoginId, this);
			s.Show();
		}

		//즐겨찾기 로딩
		private void LoadFavoriteList()
		{
			lBlist.Items.Clear();

			string sql = $@"
                SELECT u.LoginId, u.Name, u.Nickname
                FROM Favorite f
                JOIN User u ON f.FavoriteLoginId = u.LoginId
                WHERE f.LoginId = {currentLoginId}";

			DataTable dt = DBconnector.GetInstance().Query(sql);

			foreach (DataRow row in dt.Rows)
			{
				lBlist.Items.Add($"{row["LoginId"]} - {row["Name"]} ({row["Nickname"]})");
			}
		}

		//즐겨찾기 추가
		private void btnadd_Click(object sender, EventArgs e)
		{
			string LoginId = txtID.Text.Trim();
			if (LoginId == "")
			{
				MessageBox.Show("직원을 선택하세요!");
				return;
			}

			int targetLoginId = int.Parse(LoginId);

			string checkSql = $@"
                SELECT COUNT(*) 
                FROM Favorite 
                WHERE LoginId = {currentLoginId} AND FavoriteLoginId = {targetLoginId}";

			DataTable dt = DBconnector.GetInstance().Query(checkSql);

			if (Convert.ToInt32(dt.Rows[0][0]) > 0)
			{
				MessageBox.Show("이미 즐겨찾기에 등록되어 있습니다!");
				return;
			}

			string sql =
				$"INSERT INTO Favorite (LoginId, FavoriteLoginId) VALUES ({currentLoginId}, {targetLoginId})";

			DBconnector.GetInstance().NonQuery(sql);

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

			string LoginIdText = lBlist.SelectedItem.ToString().Split('-')[0].Trim();
			int targetLoginId = Convert.ToInt32(LoginIdText);

			string sql =
				$"DELETE FROM Favorite WHERE LoginId = {currentLoginId} AND FavoriteLoginId = {targetLoginId}";

			DBconnector.GetInstance().NonQuery(sql);

			MessageBox.Show("삭제되었습니다!");
			LoadFavoriteList();
		}

		private void btnChat_Click(object sender, EventArgs e)
		{
			if (lBlist.SelectedItem != null)
			{
				string LoginIdText = lBlist.SelectedItem.ToString().Split('-')[0].Trim();
				int targetLoginId = Convert.ToInt32(LoginIdText);

				new ChatForm(currentLoginId, targetLoginId).Show();
				return;
			}

			if (tvdept.SelectedNode != null && tvdept.SelectedNode.Level == 2)
			{
				int targetLoginId = Convert.ToInt32(tvdept.SelectedNode.Tag);
				new ChatForm(currentLoginId, targetLoginId).Show();
				return;
			}

			MessageBox.Show("대화할 직원을 선택하세요!");
		}

		private void tvdept_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			lBlist.ClearSelected();
		}

		private void lBlist_SelectedIndexChanged(object sender, EventArgs e)
		{
			tvdept.SelectedNode = null;
		}

		private void logout_button_Click(object sender, EventArgs e)
		{
			// 로그아웃 여부 확인
			LoginForm.Logout = true;
			// 현재 폼 닫기
			this.Close();
		}

		// 프로필 변경
		private void change_profile_button_Click(object sender, EventArgs e)
		{
			EditInfoForm editForm = new EditInfoForm();
			editForm.ShowDialog();
		}
	}
}
