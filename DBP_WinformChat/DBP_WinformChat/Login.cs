using DBP_WinformChat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 남예솔;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace leehaeun
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // 디비 연결 확인하는 부분 뺐습니다

        // 로그인 버튼
        private void LoginButton_Click(object sender, EventArgs e)
        {
            string id = IdBox.Text;
            string pw = PwBox.Text;
            string pwHash = Sha256.Instance.HashSHA256(pw);

            // UserId와 사용자 정보 가져오기
            string query = $"SELECT UserId, Name, Nickname FROM User WHERE LoginId = '{id}' AND PasswordHash = '{pwHash}';";

            // DBconnector의 Query는 DataTable을 반환 (수정)
            DataTable dt = DBconnector.GetInstance().Query(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                // 로그인 성공
                int userId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                string name = dt.Rows[0]["Name"].ToString();
                string nickname = dt.Rows[0]["Nickname"].ToString();

                MessageBox.Show("로그인 성공");
                this.Hide();

                // 채팅 리스트 폼으로 바로가기
                // 생성자로 UserId, Name, Nickname 전달
                var chatListForm = new chatlist(userId, name, nickname);
                chatListForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("로그인 실패");
            }
        }

        // 회원가입버튼
        private void SignUpLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var signUp = new SignUp();
            signUp.ShowDialog();
            this.Show();
        }


        // 로그인폼 로드
        private void Login_Load(object sender, EventArgs e)
        {
            CheckBox[] checkBoxes = { RememberMe, SaveInfo };
            TextBox[] loginInfo = { IdBox, PwBox };

            bool login = LoginConfig.loadConfig(checkBoxes, loginInfo);
            if (login) LoginButton_Click(sender, e);
        }


        // 로그인폼 닫힐 때
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckBox[] checkBoxes = { RememberMe, SaveInfo };
            TextBox[] loginInfo = { IdBox, PwBox };

            LoginConfig.saveConfig(checkBoxes, loginInfo);
        }
    }
}