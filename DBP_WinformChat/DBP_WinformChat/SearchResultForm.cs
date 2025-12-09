using DBP_WinformChat;
using kyg;
using leehaeun;
using MySqlConnector;
using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;
using 남예솔;
using DBP_Chat;

namespace DBP_Chat
{
	public partial class SearchResultForm : Form
	{
		string id, name, dept;
		int currentUserId;
		Dept parentForm;

		public SearchResultForm(string id, string name, string dept, int userId, Dept parent)
		{
			InitializeComponent();

			this.id = id;
			this.name = name;
			this.dept = dept;
			this.currentUserId = userId;
			this.parentForm = parent;

			// 테마 적용
			ApplyTheme(ThemeManager.IsDarkMode);
			ThemeManager.Subscribe(this, ApplyTheme);

			LoadResult();
		}

		private void ApplyTheme(bool isDarkMode)
		{
			SearchResultUIHelper.Apply(this, isDarkMode);
		}

		// ================= 직원 검색 결과 로드 =================
		private void LoadResult()
		{
			string sql = @"
                SELECT 
                    u.UserId,
                    u.LoginId,
                    u.Name, 
                    p.DeptName AS DeptName,      -- 상위 부서
                    d.DeptName AS TeamName       -- 팀(하위 부서)
                FROM User u
                JOIN Department d ON u.DeptId = d.DeptId
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE 1=1 ";

			if (!string.IsNullOrEmpty(id))
				sql += $" AND u.LoginId LIKE '%{id}%' ";

			if (!string.IsNullOrEmpty(name))
				sql += $" AND u.Name LIKE '%{name}%' ";

			if (!string.IsNullOrEmpty(dept))
				sql += $" AND p.DeptName = '{dept}' ";

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

			// 테마 재적용 (항목 추가 후)
			ApplyTheme(ThemeManager.IsDarkMode);
		}
	}
}
