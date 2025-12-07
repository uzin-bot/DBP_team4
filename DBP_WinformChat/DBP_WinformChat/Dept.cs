using DBP_WinformChat;
using kyg;
using leehaeun;
using MySqlConnector;
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
        private PermissionManager permissionManager; // 어드민 추가
        public Dept(int userId, string name, string nickname)
        {
            InitializeComponent();

            this.currentUserId = userId;
            this.currentUserName = name;
            this.currentUserNickname = nickname;
            this.permissionManager = new PermissionManager(); // 어드민 추가
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

        // 어드민 수정
        //회사 → 부서 → 직원 TreeView 로딩
        private void LoadTreeView()
        {

            tvdept.Nodes.Clear();
            TreeNode companyNode = new TreeNode("회사");
            tvdept.Nodes.Add(companyNode);

            // 1. 현재 사용자가 볼 수 있는 부서 목록 가져오기
            DataTable visibleDepts = permissionManager.GetVisibleDepartments(currentUserId);

            // 권한 있는 부서가 없으면 기본 메시지 표시
            if (visibleDepts == null || visibleDepts.Rows.Count == 0)
            {
                TreeNode noDeptNode = new TreeNode("(볼 수 있는 부서가 없습니다)");
                companyNode.Nodes.Add(noDeptNode);
                tvdept.ExpandAll();
                return;
            }

            foreach (DataRow dept in visibleDepts.Rows)
            {
                // DeptPath 사용 (상위부서 > 하위부서 형태)
                string deptDisplayName = dept["DeptPath"] != DBNull.Value
                    ? dept["DeptPath"].ToString()
                    : dept["DeptName"].ToString();

                TreeNode deptNode = new TreeNode($"{deptDisplayName} ({dept["UserCount"]}명)");
                deptNode.Tag = dept["DeptId"];
                companyNode.Nodes.Add(deptNode);

                // 2. 해당 부서의 사용자 중 볼 수 있는 사용자만 표시
                DataTable deptUsers = permissionManager.GetUsersByDepartment(Convert.ToInt32(dept["DeptId"]));

                foreach (DataRow user in deptUsers.Rows)
                {
                    int userId = Convert.ToInt32(user["UserId"]);

                    // 본인은 항상 표시, 다른 사용자는 권한 체크
                    if (userId != currentUserId && !permissionManager.CanViewUser(currentUserId, userId))
                        continue;

                    string text = $"({user["LoginId"]}) {user["Name"]} ({user["Nickname"]})";

                    if (userId == currentUserId)
                        text += " (나)";

                    // 대화 차단된 사용자 표시
                    if (userId != currentUserId && !permissionManager.CanChat(currentUserId, userId))
                        text += " 🚫";

                    TreeNode userNode = new TreeNode(text);
                    userNode.Tag = user["UserId"]; // UserId로 통일

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


        // 어드민 수정
        //즐겨찾기 로딩
        private void LoadFavoriteList()
        {
            lBlist.Items.Clear();

            string sql = $@"
                SELECT u.UserId, u.LoginId, u.Name, p.Nickname
                FROM Favorite f
                JOIN User u ON f.FavoriteUserId = u.UserId
                JOIN Profile p ON u.UserId = p.UserId AND p.IsDefault = 1
                WHERE f.UserId = {currentUserId}";
            DataTable dt = DBconnector.GetInstance().Query(sql);

            foreach (DataRow row in dt.Rows)
            {
                int userId = Convert.ToInt32(row["UserId"]);

                // 권한 있는 사용자만 표시
                if (!permissionManager.CanViewUser(currentUserId, userId))
                    continue;

                string displayText = $"{row["UserId"]} - {row["Name"]} ({row["Nickname"]})";

                // 대화 차단 표시
                if (!permissionManager.CanChat(currentUserId, userId))
                    displayText += " 🚫";

                lBlist.Items.Add(displayText);
            }
        }

        // 어드민 수정
        //즐겨찾기 추가
        private void btnadd_Click(object sender, EventArgs e)
        {
            string userIdText = txtID.Text.Trim();
            if (userIdText == "")
            {
                MessageBox.Show("직원을 선택하세요!");
                return;
            }

            int targetUserId = int.Parse(userIdText);

            // 권한 체크 추가
            if (!permissionManager.CanViewUser(currentUserId, targetUserId))
            {
                MessageBox.Show("해당 사용자를 볼 수 있는 권한이 없습니다.");
                return;
            }

            string checkSql = $@"
                SELECT COUNT(*)
                FROM Favorite
                WHERE UserId = {currentUserId} AND FavoriteUserId = {targetUserId}";

            DataTable dt = DBconnector.GetInstance().Query(checkSql);

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                MessageBox.Show("이미 즐겨찾기에 등록되어 있습니다!");
                return;
            }

            string sql =
                $"INSERT INTO Favorite (UserId, FavoriteUserId) VALUES ({currentUserId}, {targetUserId})";

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
                $"DELETE FROM Favorite WHERE UserId = {currentUserId} AND FavoriteUserId = {targetUserId}";

            DBconnector.GetInstance().NonQuery(sql);

            MessageBox.Show("삭제되었습니다!");
            LoadFavoriteList();
        }


        // 어드민 수정
        //즐겨찾기 OR TreeView 직원 선택 → 채팅하기
        private void btnChat_Click(object sender, EventArgs e)
        {
            int targetUserId = -1;

            if (lBlist.SelectedItem != null)
            {
                string userIdText = lBlist.SelectedItem.ToString().Split('-')[0].Trim();
                targetUserId = Convert.ToInt32(userIdText);
            }
            else if (tvdept.SelectedNode != null && tvdept.SelectedNode.Level == 2)
            {
                targetUserId = Convert.ToInt32(tvdept.SelectedNode.Tag);
            }
            else
            {
                MessageBox.Show("대화할 직원을 선택하세요!");
                return;
            }

            // 어드민 권한 체크 추가
            var result = permissionManager.CanSendMessage(currentUserId, targetUserId);
            if (!result.CanSend)
            {
                MessageBox.Show(result.Reason, "채팅 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Close();
            new ChatForm(currentUserId, targetUserId).Show();
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
