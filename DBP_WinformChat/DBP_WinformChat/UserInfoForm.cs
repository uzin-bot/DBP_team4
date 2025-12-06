using DBP_WinformChat;
using leehaeun.UIHelpers;
using System.Data;

namespace leehaeun
{
    public partial class UserInfoForm : Form
    {
        public UserInfoForm(int id)
        {
            InitializeComponent();
            UserInfoFormUIHelper.ApplyStyles(this);
            targetUserId = id;
            GetUserProfileInfo();
        }

        private int targetUserId;

        private void GetUserProfileInfo()
        {
            // Department 정보 가져오기
            string deptQuery = $@"
                SELECT d.DeptName 
                FROM User u
                LEFT JOIN Department d ON u.DeptId = d.DeptId
                WHERE u.UserId = {targetUserId};";

            DataTable deptDt = DBconnector.GetInstance().Query(deptQuery);
            string deptName = "";
            if (deptDt != null && deptDt.Rows.Count > 0)
            {
                deptName = deptDt.Rows[0]["DeptName"].ToString();
            }

            // Profile 정보 가져오기
            string query = $"SELECT * FROM UserProfileMap WHERE OwnerUserId = {targetUserId} AND TargetUserId = {LoginForm.UserId};";
            DataTable dt = DBconnector.GetInstance().Query(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                int profileId = Convert.ToInt32(dt.Rows[0]["ProfileId"]);
                string pquery = $"SELECT * FROM Profile WHERE ProfileId = {profileId}";
                DataTable pdt = DBconnector.GetInstance().Query(pquery);
                DataRow row = pdt.Rows[0];
                LoadUserProfile(row, deptName);
            }
            else
            {
                string pquery = $"SELECT * FROM Profile WHERE UserId = {targetUserId} AND IsDefault = 1;";
                DataTable pdt = DBconnector.GetInstance().Query(pquery);
                DataRow row = pdt.Rows[0];
                LoadUserProfile(row, deptName);
            }
        }

        private void LoadUserProfile(DataRow row, string deptName)
        {
            NicknameLabel.Text = row["Nickname"].ToString();
            StatusMessageLabel.Text = row["StatusMessage"].ToString();
            DeptLabel.Text = deptName;  // DeptLabel에 표시

            string? base64String = row["ProfileImage"].ToString();
            if (!string.IsNullOrEmpty(base64String))
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                ProfileImageBox.Image = Image.FromStream(ms);
            }
        }
    }
}
