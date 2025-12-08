using DBP_WinformChat;
using leehaeun.UIHelpers;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace leehaeun
{
	public partial class UserInfoForm : Form
	{
		private int targetUserId;

		public UserInfoForm(int id)
		{
			InitializeComponent();
			UserInfoFormUIHelper.ApplyStyles(this);
			targetUserId = id;
			GetUserProfileInfo();
		}

		private void GetUserProfileInfo()
		{
			string deptQuery = $@"
                SELECT d.DeptName 
                FROM User u
                LEFT JOIN Department d ON u.DeptId = d.DeptId
                WHERE u.UserId = {targetUserId};";

			DataTable deptDt = DBconnector.GetInstance().Query(deptQuery);

			string deptName = "";
			if (deptDt != null && deptDt.Rows.Count > 0)
			{
				deptName = deptDt.Rows[0]["DeptName"]?.ToString() ?? "";
			}

			string mapQuery = $@"
                SELECT * 
                FROM UserProfileMap 
                WHERE OwnerUserId = {targetUserId}
                  AND TargetUserId = {LoginForm.UserId};";

			DataTable mapDt = DBconnector.GetInstance().Query(mapQuery);

			DataTable profileDt = null;

			if (mapDt != null && mapDt.Rows.Count > 0)
			{
				// 상대방이 나에게 설정한 프로필
				int profileId = Convert.ToInt32(mapDt.Rows[0]["ProfileId"]);
				string pquery = $"SELECT * FROM Profile WHERE ProfileId = {profileId};";
				profileDt = DBconnector.GetInstance().Query(pquery);
			}
			else
			{
				// 기본 프로필
				string pquery = $"SELECT * FROM Profile WHERE UserId = {targetUserId} AND IsDefault = 1;";
				profileDt = DBconnector.GetInstance().Query(pquery);
			}

			//수정-  Rows[0] 접근 전 체크
			if (profileDt != null && profileDt.Rows.Count > 0)
			{
				LoadUserProfile(profileDt.Rows[0], deptName);
			}
		}

		private void LoadUserProfile(DataRow row, string deptName)
		{
			NicknameLabel.Text = row["Nickname"]?.ToString() ?? "";
			StatusMessageLabel.Text = row["StatusMessage"]?.ToString() ?? "";
			DeptLabel.Text = deptName;

			//수정 - ProfileImage DBNull 안전 처리
			if (row["ProfileImage"] != DBNull.Value)
			{
				string base64String = row["ProfileImage"].ToString();
				if (!string.IsNullOrEmpty(base64String))
				{
					byte[] imageBytes = Convert.FromBase64String(base64String);
					using var ms = new MemoryStream(imageBytes);
					ProfileImageBox.Image = Image.FromStream(ms);
				}
			}
			else
			{
				// 이미지 없을 경우 기본 이미지 (선택)
				ProfileImageBox.Image = DBP_WinformChat.Properties.Resources._default;
			}
		}
	}
}
