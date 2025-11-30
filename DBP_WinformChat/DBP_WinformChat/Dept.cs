using DBP_WinformChat;
using kyg;
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
		private int currentUserId;
		private string currentUserName;
		private string currentUserNickname;

		public Dept(int userId, string name, string nickname)
		{
			InitializeComponent();

			this.currentUserId = userId;
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

				string sql = $"SELECT UserId, Name, Nickname FROM User WHERE DeptId = {dept["DeptId"]}";
				DataTable dtUser = DBconnector.GetInstance().Query(sql);

				foreach (DataRow user in dtUser.Rows)
				{
					string text = $"({user["UserId"]}) {user["Name"]} ({user["Nickname"]})";

					TreeNode userNode = new TreeNode(text);
					userNode.Tag = user["UserId"];
					deptNode.Nodes.Add(userNode);
				}
			}

			tvdept.ExpandAll();
		}

		private void tvdept_AfterSelect(object sender, TreeViewEventArgs e)
		{

		}

		//현재 >> 직원 더블클릭 → "프로필 폼 예정입니다" 메시지 뜸 (채팅 X)
		//메시지 -> 폼 연결
		private void tvdept_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Level != 2) return;

			int targetUserId = Convert.ToInt32(e.Node.Tag);

			var f = new leehaeun.UserInfoForm(targetUserId);
			f.Show();
		}

		//검색창 열기(SearchResultForm)
		private void btnsearch_Click(object sender, EventArgs e)
		{
			string id = txtID.Text.Trim();
			string name = txtname.Text.Trim();
			string dept = txtdept.Text.Trim();

			SearchResultForm s = new SearchResultForm(id, name, dept, currentUserId, this);
			s.Show();
		}

		//즐겨찾기 로딩
		private void LoadFavoriteList()
		{
			lBlist.Items.Clear();

			string sql = $@"
                SELECT u.UserId, u.Name, u.Nickname
                FROM Favorite f
                JOIN User u ON f.FavortieUserId = u.UserId
                WHERE f.UserId = {currentUserId}";

			DataTable dt = DBconnector.GetInstance().Query(sql);

			foreach (DataRow row in dt.Rows)
			{
				lBlist.Items.Add($"{row["UserId"]} - {row["Name"]} ({row["Nickname"]})");
			}
		}

		//즐겨찾기 추가
		private void btnadd_Click(object sender, EventArgs e)
		{
			string userId = txtID.Text.Trim();
			if (userId == "")
			{
				MessageBox.Show("직원을 선택하세요!");
				return;
			}

			int targetUserId = int.Parse(userId);

			string checkSql = $@"
                SELECT COUNT(*) 
                FROM Favorite 
                WHERE UserId = {currentUserId} AND FavortieUserId = {targetUserId}";

			DataTable dt = DBconnector.GetInstance().Query(checkSql);

			if (Convert.ToInt32(dt.Rows[0][0]) > 0)
			{
				MessageBox.Show("이미 즐겨찾기에 등록되어 있습니다!");
				return;
			}

			string sql =
				$"INSERT INTO Favorite (UserId, FavortieUserId) VALUES ({currentUserId}, {targetUserId})";

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

			string userIdText = lBlist.SelectedItem.ToString().Split('-')[0].Trim();
			int targetUserId = Convert.ToInt32(userIdText);

			string sql =
				$"DELETE FROM Favorite WHERE UserId = {currentUserId} AND FavortieUserId = {targetUserId}";

			DBconnector.GetInstance().NonQuery(sql);

			MessageBox.Show("삭제되었습니다!");
			LoadFavoriteList();
		}

		//즐겨찾기 OR TreeView 직원 선택 → 채팅하기
		private void btnChat_Click(object sender, EventArgs e)
		{
			//즐겨찾기 선택
			if (lBlist.SelectedItem != null)
			{
				string userIdText = lBlist.SelectedItem.ToString().Split('-')[0].Trim();
				int targetUserId = Convert.ToInt32(userIdText);

				new ChatForm(currentUserId, targetUserId).Show();
				return;
			}

			//TreeView에서 직원 선택한 경우
			if (tvdept.SelectedNode != null && tvdept.SelectedNode.Level == 2)
			{
				int targetUserId = Convert.ToInt32(tvdept.SelectedNode.Tag);
				new ChatForm(currentUserId, targetUserId).Show();
				return;
			}

			MessageBox.Show("대화할 직원을 선택하세요!");
		}


		//TreeView에서 직원 선택 시 즐겨찾기에서는 선택 해제
		private void tvdept_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			lBlist.ClearSelected();
		}

		//즐겨찾기에서 직원 선택 시 TreeView 에서는 선택 해제
		private void lBlist_SelectedIndexChanged(object sender, EventArgs e)
		{
			tvdept.SelectedNode = null;
		}
	}
}
