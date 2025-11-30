using DBP_WinformChat;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using 남예솔;
using kyg;

namespace DBP_Chat
{
	public partial class SearchResultForm : Form
	{

		string id, name, dept;
        int currentUserId;  // 로그인한 사용자 ID
        Dept parentForm;


        // 생성자에 currentUserId 추가
        public SearchResultForm(string id, string name, string dept, int userId, Dept parent)
		{
			InitializeComponent();

			this.id = id;
			this.name = name;
			this.dept = dept;
            this.currentUserId = userId;
            this.parentForm = parent;

			LoadResult();
		}

		// 디비커넥터 수정
		//검색 결과 로딩
		private void LoadResult()
		{
            string sql = @"
				SELECT u.UserId, u.Name, d.DeptName, u.Nickname
				FROM User u 
				JOIN Department d ON u.DeptId = d.DeptId
				WHERE 1=1 ";

            // 검색 조건 추가
            if (!string.IsNullOrEmpty(id))
                sql += $"AND u.UserId = {id} ";

            if (!string.IsNullOrEmpty(name))
                sql += $"AND u.Name LIKE '%{name}%' ";

            if (!string.IsNullOrEmpty(dept))
                sql += $"AND d.DeptName = '{dept}' ";

            DataTable dt = DBconnector.GetInstance().Query(sql);

            lvResult.Items.Clear();

            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["UserId"].ToString());
                item.SubItems.Add(row["Name"].ToString());
                item.SubItems.Add(row["DeptName"].ToString());
                item.SubItems.Add(row["Nickname"].ToString()); // 상태 대신에 nickname
                lvResult.Items.Add(item);
            }
        }

		//더블클릭 → 채팅하기
		private void lvResult_DoubleClick(object sender, EventArgs e)
		{
			if (lvResult.SelectedItems.Count == 0) return;

            int targetUserId = Convert.ToInt32(lvResult.SelectedItems[0].Text);

            // ChatForm에 currentUserId와 targetUserId 전달
            new ChatForm(currentUserId, targetUserId).Show();
        }

		// 디비커넥터 수정
		//즐겨찾기 추가
		private void btnAddFavorite_Click(object sender, EventArgs e)
		{
            foreach (ListViewItem item in lvResult.Items)
            {
                if (item.Checked)
                {
                    int targetUserId = Convert.ToInt32(item.Text);

                    // 중복 체크
                    string sql = $@"
						SELECT COUNT(*) 
						FROM Favorite 
						WHERE UserId = {currentUserId} AND FavortieUserId = {targetUserId}"; ;
                    DataTable dt = DBconnector.GetInstance().Query(sql);

                    if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0)
                        continue;

                    // 즐겨찾기 추가
                    string insertSql = $@"
						INSERT INTO Favorite (UserId, FavortieUserId)
						VALUES ({currentUserId}, {targetUserId})";

                    DBconnector.GetInstance().NonQuery(insertSql);
                }
            }

            MessageBox.Show("즐겨찾기에 추가되었습니다!");

			//DeptForm 즐겨찾기 자동 새로고침
			if (parentForm != null)
				parentForm.RefreshFavorites();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
