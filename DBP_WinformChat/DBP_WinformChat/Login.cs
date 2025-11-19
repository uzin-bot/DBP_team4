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
            InitServerInfo();
        }

        private void InitServerInfo()
        {
            try
            {
                DBManager.Instance.InitServer();
                bool connectionResult = false;
                connectionResult = DBManager.Instance.Connection();

                if (!connectionResult)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB 연결 실패");
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string id = IdBox.Text;
            string pw = PwBox.Text;

            string pwHash = Sha256.Instance.HashSHA256(pw);

            string query = $"SELECT EXISTS(SELECT 1 FROM DbTest WHERE LoginId = '{id}' AND PasswordHash = '{pwHash}');";
            var result = DBManager.Instance.Query(query);
            if (result == true)
            {
                this.Hide();
                var Dept = new namyesol.Dept();
                Dept.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("로그인 실패");
            }
        }

        private void SignUpLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var signUp = new SignUp();
            signUp.ShowDialog();
            this.Show();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            CheckBox[] checkBoxes = { RememberMe, SaveInfo };
            TextBox[] loginInfo = { IdBox, PwBox };

            bool login = LoginConfig.loadConfig(checkBoxes, loginInfo);
            if (login) LoginButton_Click(sender, e);
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckBox[] checkBoxes = { RememberMe, SaveInfo };
            TextBox[] loginInfo = { IdBox, PwBox };

            LoginConfig.saveConfig(checkBoxes, loginInfo);
        }
    }
}