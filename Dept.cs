using DBP_WinformChat;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using kyg;

namespace DBP_Chat
{
	public partial class Dept : Form
	{
        // 현재 로그인한 사용자 정보
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

		// 디비커넥터 수정
		// 회사 → 부서 → 직원 로드
		private void LoadTreeView()
		{
			tvdept.Nodes.Clear();
			TreeNode companyNode = new TreeNode("회사");
			tvdept.Nodes.Add(companyNode);

            // 부서 목록 가져오기
            string sqlDept = "SELECT DeptId, DeptName FROM Department";
            DataTable dtDept = DBconnector.GetInstance().Query(sqlDept);

            foreach (DataRow dept in dtDept.Rows)
			{
				TreeNode deptNode = new TreeNode(dept["DeptName"].ToString());
				deptNode.Tag = dept["DeptId"];
				companyNode.Nodes.Add(deptNode);

                // 각 부서의 직원 가져오기
                string sql = $"SELECT UserId, Name, Nickname FROM User WHERE DeptId = {dept["DeptId"]}";
                DataTable dtUser = DBconnector.GetInstance().Query(sql);

                foreach (DataRow user in dtUser.Rows)
                {
                    // ID + 이름 + 닉네임을 TreeView에 표시
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

		//직원 더블클릭 → 채팅창 열기
		private void tvdept_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Level != 2) return;

            int targetUserId = Convert.ToInt32(e.Node.Tag);

            // ChatForm에 현재 사용자와 대화 상대 전달
            new ChatForm(currentUserId, targetUserId).Show();
        }

		//검색 버튼 클릭시 검색창 열기
		private void btnsearch_Click(object sender, EventArgs e)
		{
			string id = txtID.Text.Trim();
			string name = txtname.Text.Trim();
			string dept = txtdept.Text.Trim();

			SearchResultForm s = new SearchResultForm(id, name, dept, currentUserId, this);
			s.Show();
		}


		// 디비커넥터 수정
		//즐겨찾기 리스트 로딩
		private void LoadFavoriteList()
		{
			lBlist.Items.Clear();

            // Favorite 테이블에서 즐겨찾기 목록 가져오기
            string sql = $@"
				SELECT u.UserId, u.Name, u.Nickname
				FROM Favorite f
				JOIN User u ON f.FavortieUserId = u.UserId
				WHERE f.UserId = {currentUserId}";

            DataTable dt = DBconnector.GetInstance().Query(sql);

            foreach (DataRow row in dt.Rows)
            {
				// status가 왜들어가는거죠?
                //lBlist.Items.Add($"{row["emp_id"]} - {row["emp_name"]} ({row["status"]})");
                lBlist.Items.Add($"{row["UserId"]} - {row["Name"]} ({row["Nickname"]})");
            }
        }

		// 디비커넥터 수정
		// 즐겨찾기 추가
		private void btnadd_Click(object sender, EventArgs e)
		{
			string userId = txtID.Text.Trim();

			if (userId == "")
			{
				MessageBox.Show("직원을 선택하세요!");
				return;
			}

			int targetUserId = int.Parse(userId);

            // 중복 체크 (수정 완료)
            string checkSql = $@"
				SELECT COUNT(*) 
				FROM Favorite 
				WHERE UserId = {currentUserId} AND FavortieUserId = {targetUserId}";
            DataTable dt = DBconnector.GetInstance().Query(checkSql);

            if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                MessageBox.Show("이미 즐겨찾기에 등록되어 있습니다!");
                return;
            }

            // 즐겨찾기 추가
            string sql = $"INSERT INTO Favorite (UserId, FavortieUserId) VALUES ({currentUserId}, {targetUserId})";
            DBconnector.GetInstance().NonQuery(sql);


            MessageBox.Show("즐겨찾기에 추가되었습니다!");
			LoadFavoriteList();
		}

		// 디비커넥터 수정 
		// 즐겨찾기 삭제
		private void btndelete_Click(object sender, EventArgs e)
		{
			if (lBlist.SelectedItem == null)
			{
				MessageBox.Show("삭제할 대상을 선택하세요!");
				return;
			}

            string userIdText = lBlist.SelectedItem.ToString().Split('-')[0].Trim();
            int targetUserId = Convert.ToInt32(userIdText);

            string sql = $"DELETE FROM Favorite " +
				$"WHERE UserId = {currentUserId} AND FavortieUserId = {targetUserId}";
            DBconnector.GetInstance().NonQuery(sql);

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

            string userIdText = lBlist.SelectedItem.ToString().Split('-')[0].Trim();
            int targetUserId = Convert.ToInt32(userIdText);

            // ChatForm에 현재 사용자와 대화 상대 전달 
            new ChatForm(currentUserId, targetUserId).Show();
        }
	}
}
