using DBP_WinformChat;
using System.Data;
using System.Windows.Forms;           
using MySqlConnector;

namespace leehaeun
{
    public partial class SearchUserForm : Form
    {
        public List<int> selectedUserIds = new List<int>();

        public SearchUserForm()
        {
            InitializeComponent();
            SearchUser();
        }

        // 멀티프로필 매핑 안된 유저 검색(쿼리 수정)
        private void SearchUser()
        {
            string query = $@"
                SELECT 
                    u.UserId, 
                    u.Name, 
                    p.Nickname, 
                    p.ProfileId, 
                    d.DeptName
                FROM User u
                LEFT JOIN UserProfileMap upm
                    ON upm.OwnerUserId = u.UserId
                    AND upm.TargetUserId = {LoginForm.UserId}
                LEFT JOIN Profile p
                    ON p.ProfileId = upm.ProfileId
                LEFT JOIN Department d
                    ON d.DeptId = u.DeptId
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM UserProfileMap already
                    WHERE already.OwnerUserId = {LoginForm.UserId}
                    AND already.TargetUserId = u.UserId
                );";

            DataTable dt = DBconnector.GetInstance().Query(query);

            UserListView.Columns.Clear();
            UserListView.Columns.Add("UserId", 100);
            UserListView.Columns.Add("Name", 150);
            UserListView.Columns.Add("Nickname", 150);
            UserListView.Columns.Add("DeptName", 150);

            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["UserId"].ToString());
                item.SubItems.Add(row["Name"].ToString());
                item.SubItems.Add(row["Nickname"].ToString());
                item.SubItems.Add(row["DeptName"].ToString());

                UserListView.Items.Add(item);
            }
        }

        // 선택된 유저의 UserId 리스트 생성
        private void AddButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in UserListView.CheckedItems)
            {
                selectedUserIds.Add(int.Parse(item.SubItems[0].Text));
            }

            int[] selectedUserIdArray = selectedUserIds.ToArray();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}