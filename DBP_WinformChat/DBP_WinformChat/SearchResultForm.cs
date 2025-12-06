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
			InitializeComponent();

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

		private void LoadResult()
		{
            // 로그인 추가 관리자 제외
			// 닉네임관련 쿼리 수정
            string sql = @"
			SELECT u.UserId, u.LoginId, u.Name, d.DeptName, p.Nickname
			FROM User u 
			JOIN Profile p ON u.UserId = p.UserId AND p.IsDefault = 1
			JOIN Department d ON u.DeptId = d.DeptId
			WHERE u.Role != 'admin' ";

            if (!string.IsNullOrEmpty(id))
				sql += $"AND u.LoginId LIKE '%{id}%'";

			if (!string.IsNullOrEmpty(name))
				sql += $"AND u.Name LIKE '%{name}%' ";

			if (!string.IsNullOrEmpty(dept))
				// 부분 검색 바꿀까요?
				sql += $"AND d.DeptName = '{dept}' ";

			DataTable dt = DBconnector.GetInstance().Query(sql);

			lvResult.Items.Clear();

			foreach (DataRow row in dt.Rows)
			{
				ListViewItem item = new ListViewItem(row["LoginId"].ToString());
				item.SubItems.Add(row["Name"].ToString());
				item.SubItems.Add(row["DeptName"].ToString());
				item.SubItems.Add(row["Nickname"].ToString());

                // Tag에 실제 UserId 저장
                item.Tag = row["UserId"].ToString();

                lvResult.Items.Add(item);
			}
		}

		private void lvResult_DoubleClick(object sender, EventArgs e)
		{
			if (lvResult.SelectedItems.Count == 0) return;

            // Tag에서 UserId 가져오기
            int targetUserId = Convert.ToInt32(lvResult.SelectedItems[0].Tag);

          
			new ChatForm(currentUserId, targetUserId).Show();
		}

		private void btnAddFavorite_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lvResult.Items)
			{
				if (item.Checked)
				{

                    // Tag에서 UserId 가져오기
                    int targetUserId = Convert.ToInt32(item.Tag);

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
	}
}
