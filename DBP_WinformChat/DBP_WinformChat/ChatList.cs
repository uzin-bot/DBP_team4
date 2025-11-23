using DBP_WinformChat;
using kyg;
using MySql.Data.MySqlClient;
using namyesol;
using System;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace namyesol
{
    public partial class ChatList : Form
    {
        private readonly string myId; // 로그인한 사용자 ID 저장용

        // 추가
        // 생성자에서 로그인 ID를 받아오기
        public ChatList(string loginUserId)
        {
            InitializeComponent();
            myId = loginUserId; // 로그인 폼에서 넘겨받음
            LoadRecentChat();
        }


        public ChatList() // 기본 생성자
        {
            InitializeComponent();
            LoadRecentChat();
        }


        // RecentChat 테이블에서 현재 채팅중 리스트 불러오기
        // 디비커넥터로 수정
        private void LoadRecentChat()
        {
            lvlist.Items.Clear();

           
            string sql = @"
                SELECT r.emp_id, e.emp_name, d.dept_name, r.time
                FROM RecentChat r
                JOIN employee e ON r.emp_id = e.emp_id
                JOIN department d ON e.dept_id = d.dept_id
                ORDER BY r.time DESC";

            DataTable dt = DBconnector.GetInstance().Query(sql);
            if (dt == null) return;


            foreach (DataRow row in dt.Rows)
            {
                string empId = row["emp_id"].ToString();
                string empName = row["emp_name"].ToString();
                string deptName = row["dept_name"].ToString();
                string timeStr = row["time"].ToString();

                var item = new ListViewItem(empId);
                item.SubItems.Add(empName);
                item.SubItems.Add(deptName);
                item.SubItems.Add(timeStr);

                // Tag에 emp_name 저장 → 더블클릭 시 파라미터로 전달
                item.Tag = empName;
                lvlist.Items.Add(item);
            }

           
        }


        // 더블클릭 → 채팅창 열기
        // 수정
        private void lvlist_DoubleClick(object sender, EventArgs e)
        {
            if (lvlist.SelectedItems.Count == 0) return; 

            var selected = lvlist.SelectedItems[0]; // ListView에서 선택된 첫 번째 아이템
            string partnerId = selected.SubItems[0].Text; // emp_id
            string partnerName = selected.Tag.ToString(); // emp_name


            // 수정전
            // ChatForm chat = new ChatForm(targetID);

            // 수정후
            var chat = new ChatForm(myId, partnerId, partnerName);
            chat.Show();
        }
    }
}