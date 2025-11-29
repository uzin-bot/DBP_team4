using DBP_Chat; // Dept
using DBP_WinformChat;
using kyg;
using leehaeun;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;

namespace 남예솔
{
	public partial class chatlist : Form
	{
		//현재 로그인한 사용자 정보(UserInfo에서 가져옴)
		private int currentUserId = Convert.ToInt32(UserInfo.User.Rows[0]["UserId"]);
		private string currentUserName = UserInfo.User.Rows[0]["Name"].ToString();
		private string currentUserNickname = UserInfo.Profile.Rows[0]["Nickname"].ToString();

		public chatlist()
		{
			InitializeComponent();

			btndept.Click += btndept_Click; //클릭시 DeptForm으로 이동 
		}

		private void chatlist_Load(object sender, EventArgs e)
		{
			LoadRecentChat();
		}

		//RecentChat + 고정정렬
		private void LoadRecentChat()
		{
			lvlist.Items.Clear();

			string sql = $@"
                SELECT 
                    rc.PartnerUserId,
                    u.Name,
                    u.Nickname,
                    d.DeptName,
                    rc.LastMessage,        
                    rc.LastMessageAt,
                    rc.is_pinned
                FROM RecentChat rc
                JOIN User u ON rc.PartnerUserId = u.UserId
                JOIN Department d ON u.DeptId = d.DeptId
                WHERE rc.UserId = {currentUserId}
                ORDER BY rc.is_pinned DESC, rc.LastMessageAt DESC";

			DataTable dt = DBconnector.GetInstance().Query(sql);

			foreach (DataRow row in dt.Rows)
			{
				bool isPinned = Convert.ToInt32(row["is_pinned"]) == 1;

				ListViewItem item = new ListViewItem();
				item.ImageIndex = isPinned ? 0 : -1;

				item.SubItems.Add(row["PartnerUserId"].ToString());
				item.SubItems.Add(row["Name"].ToString());
				item.SubItems.Add(row["DeptName"].ToString());

				//최근 메시지 길면 ...으로 잘림 (20제한 >> UI 변경시 늘리거나 해도 O)
				string msg = row["LastMessage"].ToString();
				if (msg.Length > 20) 
					msg = msg.Substring(0, 20) + "…";
				item.SubItems.Add(msg);

				item.SubItems.Add(row["LastMessageAt"].ToString());

				lvlist.Items.Add(item);
			}
		}

		//우클릭 자동 선택
		private void lvlist_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ListViewItem item = lvlist.GetItemAt(e.X, e.Y);
				if (item != null)
					item.Selected = true;
			}
		}

		//더블클릭 → 채팅창 열기
		private void lvlist_DoubleClick(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

			int targetUserId = Convert.ToInt32(lvlist.SelectedItems[0].SubItems[1].Text);

			new ChatForm(currentUserId, targetUserId).Show();
		}

		//고정하기
		private void PinChat(int partnerUserId)
		{
			string sql = $@"
                UPDATE RecentChat 
                SET is_pinned = 1
                WHERE UserId = {currentUserId} AND PartnerUserId = {partnerUserId}";

			DBconnector.GetInstance().NonQuery(sql);
		}

		//고정 해제
		private void UnpinChat(int partnerUserId)
		{
			string sql = $@"
                UPDATE RecentChat 
                SET is_pinned = 0 
                WHERE UserId = {currentUserId} AND PartnerUserId = {partnerUserId}";

			DBconnector.GetInstance().NonQuery(sql);
		}

		//우클릭 메뉴 → 고정하기
		private void addpin_Click(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

			int partnerUserId = Convert.ToInt32(lvlist.SelectedItems[0].SubItems[1].Text);
			PinChat(partnerUserId);
			LoadRecentChat();
		}

		//우클릭 메뉴 → 고정 해제
		private void deletepin_Click(object sender, EventArgs e)
		{
			if (lvlist.SelectedItems.Count == 0) return;

			int partnerUserId = Convert.ToInt32(lvlist.SelectedItems[0].SubItems[1].Text);
			UnpinChat(partnerUserId);
			LoadRecentChat();
		}

		//btndept → 친구 목록(DeptForm)으로 이동
		private void btndept_Click(object sender, EventArgs e)
		{
			Dept deptForm = new Dept(currentUserId, currentUserName, currentUserNickname);
			deptForm.Show();
		}
	}
}
