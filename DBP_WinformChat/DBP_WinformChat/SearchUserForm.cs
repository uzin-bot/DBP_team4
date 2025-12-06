using DBP_WinformChat;
using leehaeun.UIHelpers;
using MySqlConnector;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace leehaeun
{
    public partial class SearchUserForm : Form
    {
        public List<int> selectedUserIds = new List<int>();

        public SearchUserForm()
        {
            InitializeComponent();
            SearchUserFormUIHelper.ApplyStyles(this);
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

            // 데이터를 리스트로 변환
            List<(int, string, string, string)> users = new List<(int, string, string, string)>();

            foreach (DataRow row in dt.Rows)
            {
                int userId = int.Parse(row["UserId"].ToString());
                string name = row["Name"].ToString();
                string nickname = row["Nickname"].ToString();
                string deptName = row["DeptName"].ToString();

                users.Add((userId, name, nickname, deptName));
            }

            // 커스텀 패널에 로드
            SearchUserFormUIHelper.LoadUsers(this, users);
        }

        // 선택된 유저의 UserId 리스트 생성
        private void AddButton_Click(object sender, EventArgs e)
        {
            // 체크된 사용자 가져오기
            Panel scrollPanel = null;

            foreach (Control control in this.Controls)
            {
                if (control is Panel wrapper && wrapper.Tag != null && wrapper.Tag.ToString() == "UserScrollPanelWrapper")
                {
                    foreach (Control child in wrapper.Controls)
                    {
                        if (child is Panel panel && panel.Name == "UserScrollPanel")
                        {
                            scrollPanel = panel;
                            break;
                        }
                    }
                    break;
                }
            }

            if (scrollPanel != null)
            {
                foreach (Control item in scrollPanel.Controls)
                {
                    if (item is Panel itemPanel)
                    {
                        foreach (Control itemControl in itemPanel.Controls)
                        {
                            if (itemControl is PictureBox checkBox && checkBox.Tag is object tagObj)
                            {
                                var tagType = tagObj.GetType();
                                var userIdProp = tagType.GetProperty("UserId");
                                var checkedProp = tagType.GetProperty("Checked");

                                if (userIdProp != null && checkedProp != null)
                                {
                                    bool isChecked = (bool)checkedProp.GetValue(tagObj);
                                    if (isChecked)
                                    {
                                        int userId = (int)userIdProp.GetValue(tagObj);
                                        selectedUserIds.Add(userId);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            if (selectedUserIds.Count == 0)
            {
                MessageBox.Show("선택된 사용자가 없습니다.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
