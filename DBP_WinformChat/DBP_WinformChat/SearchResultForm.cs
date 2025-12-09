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
            // [수정] SELECT 절에 u.LoginId 추가 (리스트뷰 표시용)
            string sql = $@"
        SELECT 
            u.UserId, 
            u.LoginId,                   -- [추가] 리스트뷰에 표시할 로그인 ID
            u.Name, 
            p.DeptName AS DeptName,      -- 상위 부서명
            d.DeptName AS TeamName       -- 팀명(자식 부서)
        FROM User u 
        JOIN Department d ON u.DeptId = d.DeptId
        LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
        WHERE 1=1 
        
        -- [필터 1] 관리자가 설정한 '안 보이게 할 사용자' 제외
        AND u.UserId NOT IN (
            SELECT VisibleUserId 
            FROM UserVisibleUser 
            WHERE OwnerUserId = {this.currentUserId}
        )

        -- [필터 2] 관리자가 설정한 '안 보이게 할 부서' 제외
        AND u.DeptId NOT IN (
            SELECT DeptId 
            FROM UserVisibleDept 
            WHERE OwnerUserId = {this.currentUserId}
        )
        -- (2) 상위 부서가 제한된 경우
        AND (d.ParentDeptId IS NULL OR d.ParentDeptId NOT IN (
            SELECT DeptId 
            FROM UserVisibleDept 
            WHERE OwnerUserId = {this.currentUserId}
        ))";

            // 검색 조건: LoginId로 검색
            if (!string.IsNullOrEmpty(id))
                sql += $" AND u.LoginId LIKE '%{id}%' ";

            if (!string.IsNullOrEmpty(name))
                sql += $" AND u.Name LIKE '%{name}%' ";

            if (!string.IsNullOrEmpty(dept))
                sql += $" AND p.DeptName = '{dept}' ";

            DataTable dt = DBconnector.GetInstance().Query(sql);

            lvResult.Items.Clear();

            // [수정] 보내주신 코드대로 LoginId를 첫 번째 컬럼에 표시
            foreach (DataRow row in dt.Rows)
            {
                // DB에서 가져온 LoginId 컬럼 사용
                ListViewItem item = new ListViewItem(row["LoginId"].ToString());
                item.SubItems.Add(row["Name"].ToString());

                // 상위 부서가 없는 경우 처리
                string parentDeptName = row["DeptName"] == DBNull.Value ? "-" : row["DeptName"].ToString();
                item.SubItems.Add(parentDeptName);

                item.SubItems.Add(row["TeamName"].ToString());
                lvResult.Items.Add(item);
            }
        }
    }
}
