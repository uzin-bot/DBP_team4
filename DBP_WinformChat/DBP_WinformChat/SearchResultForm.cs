using DBP_WinformChat;
using kyg;
using leehaeun;
using MySqlConnector;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using 남예솔;

namespace DBP_Chat
{
	public partial class SearchResultForm : Form
	{
		string id, name, dept;
		int currentUserId;
		Dept parentForm;

		// ===== 다크모드 색상 =====
		private Color _darkBack = Color.FromArgb(32, 32, 32);
		private Color _darkHeader = Color.FromArgb(64, 64, 64);
		private Color _darkText = Color.White;
		private Color _darkButton = Color.FromArgb(80, 100, 90);

		public SearchResultForm(string id, string name, string dept, int userId, Dept parent)
		{
			InitializeComponent();   


			btnAddFavorite.Click += btnAddFavorite_Click;
			btnClose.Click += btnClose_Click;

			this.AutoScaleMode = AutoScaleMode.None;
			this.DoubleBuffered = true;

			this.id = id;
			this.name = name;
			this.dept = dept;
			this.currentUserId = userId;
			this.parentForm = parent;

			// 테마 이벤트
			ThemeManager.ThemeChanged += mode => OnThemeChanged(mode);

			// 초기 테마 적용
			if (ThemeManager.CurrentMode == ThemeMode.Dark)
				ApplyTheme(true);
			else
				ApplyLightTheme();

			LoadResult();
		}

		// ================= 직원 검색 결과 로드 =================
		private void LoadResult()
		{
			string sql = $@"
SELECT 
	u.UserId,
	u.LoginId,
	u.Name,
	p.DeptName AS DeptName,
	d.DeptName AS TeamName
FROM User u
JOIN Department d ON u.DeptId = d.DeptId
LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
WHERE 1=1
AND u.UserId NOT IN (
	SELECT VisibleUserId 
	FROM UserVisibleUser 
	WHERE OwnerUserId = {currentUserId}
)
AND d.DeptId NOT IN (
	SELECT DeptId
	FROM UserVisibleDept
	WHERE OwnerUserId = {LoginForm.UserId}
)
AND (p.DeptId IS NULL OR p.DeptId NOT IN (
	SELECT DeptId
	FROM UserVisibleDept
	WHERE OwnerUserId = {LoginForm.UserId}
))";

			// LoginId 기준 검색
			if (!string.IsNullOrEmpty(id))
				sql += $" AND u.LoginId LIKE '%{id}%'";

			if (!string.IsNullOrEmpty(name))
				sql += $" AND u.Name LIKE '%{name}%'";

			if (!string.IsNullOrEmpty(dept))
				sql += $" AND p.DeptName = '{dept}'";

			DataTable dt = DBconnector.GetInstance().Query(sql);

			lvResult.Items.Clear();

			foreach (DataRow row in dt.Rows)
			{
				ListViewItem item = new ListViewItem(row["LoginId"].ToString());
				item.SubItems.Add(row["Name"].ToString());
				item.SubItems.Add(row["DeptName"].ToString());
				item.SubItems.Add(row["TeamName"].ToString());
				lvResult.Items.Add(item);
			}
		}

		// ================= 즐겨찾기 추가 =================
		private void btnAddFavorite_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lvResult.Items)
			{
				if (!item.Checked) continue;

				string loginId = item.Text;

				// LoginId → UserId 변환
				string findUserSql = $"SELECT UserId FROM User WHERE LoginId = '{loginId}'";
				DataTable userDt = DBconnector.GetInstance().Query(findUserSql);
				if (userDt.Rows.Count == 0) continue;

				int targetUserId = Convert.ToInt32(userDt.Rows[0]["UserId"]);

				// 중복 체크
				string checkSql = $@"
SELECT COUNT(*)
FROM Favorite
WHERE UserId = {currentUserId}
AND FavoriteUserId = {targetUserId}";

				DataTable checkDt = DBconnector.GetInstance().Query(checkSql);
				if (Convert.ToInt32(checkDt.Rows[0][0]) > 0)
					continue;

				// 즐겨찾기 추가
				string insertSql = $@"
INSERT INTO Favorite (UserId, FavoriteUserId)
VALUES ({currentUserId}, {targetUserId})";

				DBconnector.GetInstance().NonQuery(insertSql);
			}

			MessageBox.Show("즐겨찾기에 추가되었습니다!");
			parentForm?.RefreshFavorites();
		}

		// ================= 닫기 =================
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		// ================= 테마 처리 =================
		private void OnThemeChanged(ThemeMode mode)
		{
			if (mode == ThemeMode.Dark)
				ApplyTheme(true);
			else
				ApplyLightTheme();
		}

		private void ApplyTheme(bool isDark)
		{
			if (!isDark)
			{
				ApplyLightTheme();
				return;
			}

			//폼 / 헤더
			this.BackColor = _darkBack;
			this.panel1.BackColor = _darkHeader;
			this.label1.ForeColor = Color.White;

			//리스트뷰
			lvResult.BackColor = Color.FromArgb(30, 30, 30);
			lvResult.ForeColor = _darkText;

			//버튼
			StyleButton(btnAddFavorite, _darkButton);
			StyleButton(btnClose, _darkButton);
		}

		private void ApplyLightTheme()
		{
			SearchResultUIHelper.Apply(this, false);

			btnAddFavorite.FlatStyle = FlatStyle.Standard;
			btnClose.FlatStyle = FlatStyle.Standard;
		}

		// ================= 버튼 스타일 헬퍼 =================
		private void StyleButton(Button b, Color back)
		{
			b.BackColor = back;
			b.ForeColor = Color.White;
			b.FlatStyle = FlatStyle.Flat;
		}
	}
}
