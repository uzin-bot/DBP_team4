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

            string query = $"SELECT EXISTS(SELECT 1 FROM DbTest WHERE LoginId = '{id}' AND PasswordHash = '{pwHash}');";
            // DBconnector의 Query는 DataTable을 반환 (수정)
            DataTable dt = DBconnector.GetInstance().Query(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                // 로그인 성공
                MessageBox.Show("로그인 성공");
                this.Hide();
                // 이거 리스트로 바로 가도록 수정 예정
                var mainForm = new Form1();
                mainForm.ShowDialog();
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