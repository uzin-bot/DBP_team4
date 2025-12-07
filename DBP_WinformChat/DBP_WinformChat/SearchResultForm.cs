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

namespace DBP_Chat
{
    public partial class SearchResultForm : Form
    {
        string id, name, dept;
        int currentUserId;
        Dept parentForm;

        // 다크모드 색상 정의
        private Color _darkBack = Color.FromArgb(32, 32, 32);
        private Color _darkPanel = Color.FromArgb(45, 45, 45);
        private Color _darkHeader = Color.FromArgb(64, 64, 64);
        private Color _darkButton = Color.FromArgb(80, 100, 90);
        private Color _darkText = Color.White;

        public SearchResultForm(string id, string name, string dept, int userId, Dept parent)
        {

            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.UserPaint, true);

            InitializeComponent();

            this.id = id;
            this.name = name;
            this.dept = dept;
            this.currentUserId = userId;
            this.parentForm = parent;

            //셀 클릭 시 자동 체크되도록 이벤트 연결
            lvResult.ItemSelectionChanged += lvResult_ItemSelectionChanged;

            // 전역 테마 변경 이벤트 구독
            ThemeManager.ThemeChanged += mode => this.OnThemeChanged(mode);

            // 현재 테마 상태에 따라 초기 스타일 적용
            if (ThemeManager.CurrentMode == ThemeMode.Dark)
            {
                this.ApplyTheme(true);
            }
            else
            {
                this.ApplyLightTheme();
            }

            LoadResult();
        }

        //셀 클릭 시 자동 체크되도록 설정
        //셀 클릭하면 자동으로 체크표시 되도록 변경했습니다!
        private void lvResult_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // e.Item.Checked = true;
        }

        /*
        // 팀 컬럼(DeptPath의 마지막 세그먼트) 추가 조회 및 표시
        private void LoadResult()
        {
            string sql = @"
                SELECT 
                    u.UserId, 
                    u.Name, 
                    p.DeptName AS DeptName,      -- 상위 부서명
                    d.DeptName AS TeamName,      -- 팀명(자식 부서 DeptName)
                    u.Nickname
                FROM User u 
                JOIN Department d ON u.DeptId = d.DeptId
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE 1=1 ";

            if (!string.IsNullOrEmpty(id))
                sql += $"AND u.UserId = {id} ";

            if (!string.IsNullOrEmpty(name))
                sql += $"AND u.Name LIKE '%{name}%' ";

            if (!string.IsNullOrEmpty(dept))
                sql += $"AND p.DeptName = '{dept}' ";

            DataTable dt = DBconnector.GetInstance().Query(sql);

            lvResult.Items.Clear();

            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["UserId"].ToString());
                item.SubItems.Add(row["Name"].ToString());
                item.SubItems.Add(row["DeptName"].ToString()); // 상위 부서
                item.SubItems.Add(row["TeamName"].ToString()); // 팀(자식 부서)
                item.SubItems.Add(row["Nickname"].ToString());
                lvResult.Items.Add(item);
            }
        }
        */

        private void LoadResult()
        {
            string sql = $@"
        SELECT 
            u.UserId,
            u.LoginId,
            u.Name, 
            p.DeptName AS DeptName,      -- 상위 부서명
            d.DeptName AS TeamName      -- 팀명(자식 부서 DeptName)
        FROM User u 
        JOIN Department d ON u.DeptId = d.DeptId
        LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
        WHERE 1=1 
        AND u.UserId NOT IN (
            SELECT VisibleUserId 
            FROM UserVisibleUser 
            WHERE OwnerUserId = {this.currentUserId}
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

            // 검색 조건 추가
            if (!string.IsNullOrEmpty(id))
                sql += $" AND u.LoginId LIKE '%{id}%' ";

            if (!string.IsNullOrEmpty(name))
                sql += $" AND u.Name LIKE '%{name}%' ";

            // Dept.cs에서 '전체'를 선택하면 dept가 ""(빈값)으로 넘어오므로 이 조건문은 건너뛰게 됩니다.
            if (!string.IsNullOrEmpty(dept))
                sql += $" AND p.DeptName = '{dept}' ";

            DataTable dt = DBconnector.GetInstance().Query(sql);

            lvResult.Items.Clear();

            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["LoginId"].ToString());
                item.SubItems.Add(row["Name"].ToString());
                item.SubItems.Add(row["DeptName"].ToString()); // 상위 부서
                item.SubItems.Add(row["TeamName"].ToString()); // 팀(자식 부서)
                //item.SubItems.Add(row["Nickname"].ToString());
                lvResult.Items.Add(item);
            }
        }

        private void lvResult_DoubleClick(object sender, EventArgs e)
        {
            /*
            if (lvResult.SelectedItems.Count == 0) return;

            int targetUserId = Convert.ToInt32(lvResult.SelectedItems[0].Text);
            new ChatForm(currentUserId, targetUserId).Show();
            */
        }

        private void btnAddFavorite_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvResult.Items)
            {
                if (item.Checked)
                {
                    int targetLoginId = Convert.ToInt32(item.Text);
                    // LoginId로 UserId 찾기
                    string findUserSql = $"SELECT UserId FROM User WHERE LoginId = '{targetLoginId}'";
                    DataTable userDt = DBconnector.GetInstance().Query(findUserSql);
                    int targetUserId = Convert.ToInt32(userDt.Rows[0]["UserId"]);

                    string sql = $@"
                        SELECT COUNT(*) 
                        FROM Favorite 
                        WHERE UserId = {currentUserId} AND FavoriteUserId = {targetUserId}";

                    DataTable dt = DBconnector.GetInstance().Query(sql);

                    if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0)
                        continue;

                    string insertSql = $@"
                        INSERT INTO Favorite (UserId, FavoriteUserId)
                        VALUES ({currentUserId}, {targetUserId})";

                    DBconnector.GetInstance().NonQuery(insertSql);
                }
            }

            MessageBox.Show("즐겨찾기에 추가되었습니다!");

            if (parentForm != null)
                parentForm.RefreshFavorites();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lvResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 선택된 항목이 변경될 때 실행할 코드 작성
            // 예시: 아무 동작도 하지 않음
        }

        // ThemeManager.ThemeChanged에서 호출되는 핸들러
        private void OnThemeChanged(ThemeMode mode)
        {
            if (mode == ThemeMode.Dark)
            {
                this.ApplyTheme(true);
            }
            else
            {
                this.ApplyLightTheme();
            }
        }

        // 다크 모드 적용
        private void ApplyTheme(bool isDark)
        {
            if (!isDark)
            {
                this.ApplyLightTheme();
                return;
            }

            // 폼 배경
            this.BackColor = _darkBack;

            // 헤더 패널
            this.panel1.BackColor = _darkHeader;
            this.label1.ForeColor = Color.White;
            this.label1.BackColor = Color.Transparent;

            // ListView 다크 테마
            this.lvResult.BackColor = Color.FromArgb(30, 30, 30);
            this.lvResult.ForeColor = _darkText;

            // 버튼들 다크 테마
            //this.StyleButton(this.btnAddFavorite, _darkButton);
            //this.StyleButton(this.btnClose, _darkButton);
        }

        // 라이트 모드 적용
        private void ApplyLightTheme()
        {
            // SearchResultUIHelper 사용하여 라이트 테마 적용
            DBP_WinformChat.SearchResultUIHelper.Apply(this, false);
        }

        // 버튼 스타일 적용
        private void StyleButton(Button b, Color back)
        {
            b.BackColor = back;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
        }
    }
}