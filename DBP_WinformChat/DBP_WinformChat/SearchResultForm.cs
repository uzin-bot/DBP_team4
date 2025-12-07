using DBP_WinformChat;
using kyg;
using MySqlConnector;
using System;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using 남예솔;

namespace DBP_Chat
{
    public partial class SearchResultForm : Form
    {
        string id, name, dept;
        int currentUserId;
        Dept parentForm;

        public SearchResultForm(string id, string name, string dept, int userId, Dept parent)
        {

            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.UserPaint, true);

            InitializeComponent();

            DBP_WinformChat.SearchResultUIHelper.Apply(this);

            this.id = id;
            this.name = name;
            this.dept = dept;
            this.currentUserId = userId;
            this.parentForm = parent;

            //셀 클릭 시 자동 체크되도록 이벤트 연결
            lvResult.ItemSelectionChanged += lvResult_ItemSelectionChanged;

            LoadResult();
        }

        //셀 클릭 시 자동 체크되도록 설정
        //셀 클릭하면 자동으로 체크표시 되도록 변경했습니다!
        private void lvResult_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            e.Item.Checked = true;
        }

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

        private void lvResult_DoubleClick(object sender, EventArgs e)
        {
            if (lvResult.SelectedItems.Count == 0) return;

            int targetUserId = Convert.ToInt32(lvResult.SelectedItems[0].Text);
            new ChatForm(currentUserId, targetUserId).Show();
        }

        private void btnAddFavorite_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvResult.Items)
            {
                if (item.Checked)
                {
                    int targetUserId = Convert.ToInt32(item.Text);

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
    }
}