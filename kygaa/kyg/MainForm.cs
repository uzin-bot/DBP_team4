using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;


namespace DBP_finalproject_chatting
{
    // MainForm은 TreeView (tvDepartmentEmployees)와 ListView (lvRecentChats)
    public partial class MainForm : Form
    {
        private DBHelper dbHelper = new DBHelper();
        // 로그인에 성공한 사용자 ID는 전역적으로 접근 가능하다고 가정합니다.
        // 실제로는 로그인 폼에서 전달받아 클래스 멤버 변수에 저장해야 합니다.
        private string loggedInUserId; // 예시 ID 사용

        public MainForm()
        {
            InitializeComponent();

            // 폼 로드 시 대화상대 및 대화목록을 로드
            LoadDepartmentEmployees(); // 1주차 기능
            LoadRecentChats();         // 2주차 기능 (6-A 검정)

            // TreeView 더블클릭 이벤트 연결 (Designer에서 설정 가능)
            this.tvDepartmentEmployees.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tvDepartmentEmployees_NodeMouseDoubleClick);
            // ListView 더블클릭 이벤트 연결 (Designer에서 설정 가능)
            this.lvRecentChats.DoubleClick += new EventHandler(lvRecentChats_DoubleClick);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loggedInUserId = textBox1.Text;
        }

        // 1. 대화상대 목록 로드 (1주차 기능)
        public void LoadDepartmentEmployees()
        {
            tvDepartmentEmployees.Nodes.Clear();

            // 부서 및 직원 정보 조회 쿼리 (1주차에 제시된 쿼리 사용)
            string query = @"
            SELECT 
                D.DeptID, D.DeptName, 
                U.UserID, U.UserName, U.Nickname 
            FROM Department D 
            LEFT JOIN User U ON D.DeptID = U.DeptID 
            ORDER BY D.DeptName, U.UserName";

            DataTable dt = dbHelper.ExecuteSelect(query);

            TreeNode companyNode = new TreeNode("우리 회사") { Tag = "Company" };
            tvDepartmentEmployees.Nodes.Add(companyNode);

            string currentDeptName = string.Empty;
            TreeNode currentDeptNode = null;

            foreach (DataRow row in dt.Rows)
            {
                string deptName = row["DeptName"].ToString();
                string deptId = row["DeptID"].ToString();

                if (deptName != currentDeptName)
                {
                    currentDeptName = deptName;
                    currentDeptNode = new TreeNode(deptName) { Tag = $"Dept|{deptId}" };
                    companyNode.Nodes.Add(currentDeptNode);
                }

                if (row["UserID"] != DBNull.Value)
                {
                    string userId = row["UserID"].ToString();
                    string userName = row["UserName"].ToString();
                    string nickname = row["Nickname"].ToString();

                    TreeNode employeeNode = new TreeNode($"{userName} ({nickname})")
                    {
                        Tag = $"User|{userId}", // Tag에 사용자 ID 저장
                    };
                    currentDeptNode.Nodes.Add(employeeNode);
                }
            }
            companyNode.Expand();
        }

        // 2. TreeView 직원 더블클릭 시 채팅창 열기 (2주차 기능)
        private void tvDepartmentEmployees_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Tag가 "User|"로 시작하는지 확인 (직원 노드인지 확인)
            if (e.Node.Tag is string tag && tag.StartsWith("User|"))
            {
                string partnerId = tag.Split('|')[1];
                string partnerName = e.Node.Text;

                OpenChatForm(partnerId, partnerName);
            }
        }

        // 3. 대화목록 로드 (2주차 6-A 검정)
        public void LoadRecentChats()
        {
            lvRecentChats.Items.Clear();

            string query = @"
            SELECT 
                R.PartnerID,
                U.UserName,
                R.LastMessageTime
            FROM RecentChat R
            JOIN User U ON R.PartnerID = U.UserID
            WHERE R.UserID = @myId
            ORDER BY R.LastMessageTime DESC";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
            new MySqlParameter("@myId", loggedInUserId)
            };

            DataTable dt = dbHelper.ExecuteSelect(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                string partnerId = row["PartnerID"].ToString();
                string partnerName = row["UserName"].ToString();
                // 시간 형식을 원하는 대로 포맷팅할 수 있습니다.
                string lastTime = row["LastMessageTime"].ToString();

                ListViewItem item = new ListViewItem(partnerId);
                item.SubItems.Add(partnerName);
                item.SubItems.Add(lastTime);
                item.Tag = partnerId; // Tag에 PartnerID 저장

                lvRecentChats.Items.Add(item);
            }
        }

        // 4. ListView 대화목록 더블클릭 시 채팅창 열기 (2주차 기능)
        private void lvRecentChats_DoubleClick(object sender, EventArgs e)
        {
            if (lvRecentChats.SelectedItems.Count > 0)
            {
                // 선택된 항목의 Tag에서 PartnerID 가져오기
                string partnerId = lvRecentChats.SelectedItems[0].Tag.ToString();
                string partnerName = lvRecentChats.SelectedItems[0].SubItems[1].Text;

                OpenChatForm(partnerId, partnerName);
            }
        }

        // 5. 채팅창 열기 공통 메서드
        private void OpenChatForm(string partnerId, string partnerName)
        {
            // 새 ChatForm 인스턴스를 생성하고 ID 정보 전달
            ChatForm chatForm = new ChatForm(this.loggedInUserId, partnerId, partnerName);
            chatForm.Show();
        }

        // 기타: MyPage, 관리자 버튼 등 다른 팀원과 연계되는 기능
    }
}
