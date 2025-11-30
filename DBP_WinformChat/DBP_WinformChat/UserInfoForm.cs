using DBP_WinformChat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace leehaeun
{
    public partial class UserInfoForm : Form
    {
        public UserInfoForm(int id)
        {
            InitializeComponent();
            targetUserId = id;
            GetUserProfileInfo();
        }

        private int targetUserId;

        private void GetUserProfileInfo()
        {
            string query = $"SELECT * FROM UserProfileMap WHERE OwnerUserId = {targetUserId} AND TargetUserId = {LoginForm.UserId};";
            DataTable dt = DBconnector.GetInstance().Query(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                int profileId = Convert.ToInt32(dt.Rows[0]["ProfileId"]);
                string pquery = $"SELECT * FROM Profile WHERE ProfileId = {profileId}";
                DataTable pdt = DBconnector.GetInstance().Query(pquery);
                DataRow row = pdt.Rows[0];
                LoadUserProfile(row);
            }
            else
            {
                string pquery = $"SELECT * FROM Profile WHERE UserId = {targetUserId} AND IsDefault = 1;";
                DataTable pdt = DBconnector.GetInstance().Query(pquery);
                DataRow row = pdt.Rows[0];
                LoadUserProfile(row);
            }
        }

        private void LoadUserProfile(DataRow row)
        {
            NicknameBox.Text = row["Nickname"].ToString();
            StatusBox.Text = row["StatusMessage"].ToString();
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
