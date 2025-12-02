using DBP_WinformChat;
using kyg;
using MySql.Data.MySqlClient;
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
		int currentLoginId;
		Dept parentForm;

		public SearchResultForm(string id, string name, string dept, int LoginId, Dept parent)
		{
			InitializeComponent();

			this.id = id;
			this.name = name;
			this.dept = dept;
			this.currentLoginId = LoginId;
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
			//검색 결과에서도 관리자 제외 >>> 12월 2일 수정
			string sql = @"
                SELECT u.LoginId, u.Name, d.DeptName, u.Nickname
                FROM User u
				JOIN users us ON u.LoginId = us.user_id        
                JOIN Department d ON u.DeptId = d.DeptId
                WHERE us.is_admin = 0";                        

			if (!string.IsNullOrEmpty(id))
				sql += $" AND u.LoginId = {id} ";

			if (!string.IsNullOrEmpty(name))
				sql += $" AND u.Name LIKE '%{name}%' ";

			if (!string.IsNullOrEmpty(dept))
				sql += $" AND d.DeptName = '{dept}' ";

			DataTable dt = DBconnector.GetInstance().Query(sql);

			lvResult.Items.Clear();

			foreach (DataRow row in dt.Rows)
			{
				ListViewItem item = new ListViewItem(row["LoginId"].ToString());
				item.SubItems.Add(row["Name"].ToString());
				item.SubItems.Add(row["DeptName"].ToString());
				item.SubItems.Add(row["Nickname"].ToString());
				lvResult.Items.Add(item);
			}
		}

		private void lvResult_DoubleClick(object sender, EventArgs e)
		{
			if (lvResult.SelectedItems.Count == 0) return;

			int targetLoginId = Convert.ToInt32(lvResult.SelectedItems[0].Text);
			new ChatForm(currentLoginId, targetLoginId).Show();
		}

		private void btnAddFavorite_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lvResult.Items)
			{
				if (item.Checked)
				{
					int targetLoginId = Convert.ToInt32(item.Text);

					string sql = $@"
                        SELECT COUNT(*) 
                        FROM Favorite 
                        WHERE UserId = {currentLoginId} AND FavoriteUserId = {targetLoginId}";   

					DataTable dt = DBconnector.GetInstance().Query(sql);

					if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0)
						continue;

					string insertSql = $@"
                        INSERT INTO Favorite (UserId, FavoriteUserId)
                        VALUES ({currentLoginId}, {targetLoginId})";  

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
