using DBP_WinformChat;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using kyg;
using DBP_Chat;
using leehaeun;

namespace 남예솔
{
    public partial class chatlist : Form
    {
        // 현재 로그인한 사용자 정보
        // UserInfo의 DataTable User 정보 불러오는걸로 수정
        // UserInfo.User에서 바로 받아오셔도 됩니다!

        private int currentUserId = Convert.ToInt32(UserInfo.User.Rows[0]["UserId"]);
        private string currentUserName = UserInfo.User.Rows[0]["Name"].ToString();
        private string currentUserNickname = UserInfo.Profile.Rows[0]["Nickname"].ToString();

        /*
        private int currentUserId;
        private string currentUserName;
        private string currentUserNickname;
        */

        public chatlist()
        {
            InitializeComponent();
            /*
            this.currentUserId = userId;
            this.currentUserName = name;
            this.currentUserNickname = nickname;
            */
        }

        private void chatlist_Load(object sender, EventArgs e)
        {
            LoadRecentChat();
        }

        // 디비커넥터 수정
        //RecentChat + pinned_chat 반영
        private void LoadRecentChat()
        {
            lvlist.Items.Clear();

            // RecentChat 테이블에서 최근 대화 목록 가져오기
            string sql = $@"
                SELECT 
                    rc.PartnerUserId,
                    u.Name,
                    u.Nickname,
                    d.DeptName,
                    rc.LastMessageAt,
                    rc.is_pinned
                FROM RecentChat rc
                JOIN User u ON rc.PartnerUserId = u.UserId
                JOIN Department d ON u.DeptId = d.DeptId
                WHERE rc.UserId = {currentUserId}
                ORDER BY rc.is_pinned DESC, rc.LastMessageAt DESC";


            // DBconnector 사용
            DataTable dt = DBconnector.GetInstance().Query(sql);

            foreach (DataRow row in dt.Rows)
            {
                bool isPinned = Convert.ToInt32(row["is_pinned"]) == 1;
                ListViewItem item = new ListViewItem();
                item.ImageIndex = isPinned ? 0 : -1;
                item.SubItems.Add(row["PartnerUserId"].ToString());
                item.SubItems.Add(row["Name"].ToString());
                item.SubItems.Add(row["DeptName"].ToString());
                item.SubItems.Add(row["LastMessageAt"].ToString());
                lvlist.Items.Add(item);
            }

        }

        //우클릭 항목 자동 선택
        private void lvlist_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewItem item = lvlist.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }

        //더블 클릭  → 채팅창 열기
        private void lvlist_DoubleClick(object sender, EventArgs e)
        {
            if (lvlist.SelectedItems.Count == 0) return;

            int targetUserId = Convert.ToInt32(lvlist.SelectedItems[0].SubItems[1].Text);

            // ChatForm에 currentUserId와 targetUserId 전달
            new ChatForm(currentUserId, targetUserId).Show();
        }

        //디비커넥터 수정
        //채팅방 고정 추가
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


        // 친구 목록가기
        private void button1_Click(object sender, EventArgs e)
        {
            // Dept 폼 열기
            Dept deptForm = new Dept(currentUserId, currentUserName, currentUserNickname);
            deptForm.Show();
        }

        // 로그아웃 
        private void button2_Click(object sender, EventArgs e)
        {
            // 폼 닫으면 바로 열리게 수정
            // 현재 폼 닫기
            this.Close();
        }
    }
}
