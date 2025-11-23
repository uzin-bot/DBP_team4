using DBP_WinformChat;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace leehaeun
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void CheckIDButton_Click(object sender, EventArgs e)
        {
            string id = IdBox.Text;

            string query = $"SELECT EXISTS(SELECT 1 FROM DbTest WHERE LoginId = '{id}');";

            // 디비커넥터로 수정
            DataTable dt = DBconnector.GetInstance().Query(query);
            bool exists = dt != null && dt.Rows.Count > 0;

            if (exists)
            {
                IsDuplicate.Text = "이미 사용 중인 아이디입니다.";
                IsDuplicate.ForeColor = Color.Red;
            }
            else
            {
                IsDuplicate.Text = "사용 가능한 아이디입니다.";
                IsDuplicate.ForeColor = Color.Blue;
            }
        }

        // 회원가입 버튼
        private void SignUpButton_Click(object sender, EventArgs e)
        {
            string id = IdBox.Text;
            string pw = PwBox.Text;
            string name = NameBox.Text;
            string nickname = NickNameBox.Text;
            string address = AddressBox.Text;
            string zipCode = ZipCodeBox.Text;
            int team = TeamBox.SelectedIndex;
            string filePath = null;
            if (!string.IsNullOrEmpty(filePath))
            {
                filePath = FilePath.Text;
            }

            if (string.IsNullOrWhiteSpace(id) ||
                string.IsNullOrWhiteSpace(pw) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(nickname) ||
                string.IsNullOrWhiteSpace(address) ||
                team < 0)
            {
                MessageBox.Show("모든 항목을 입력해주세요.");
            }
            else if (IsDuplicate.Text == "이미 사용 중인 아이디입니다.")
            {
                MessageBox.Show("이미 사용 중인 아이디입니다.");
            }
            else
            {
                string pwHash = Sha256.Instance.HashSHA256(pw);
                string query = $@"INSERT INTO
                    DbTest(LoginId, PasswordHash, Name, Nickname, Address, ZipCode, DeptId, ProfileImage, Role)
                    VALUES('{id}', '{pwHash}', '{name}', '{nickname}', '{address}', {zipCode}, {team}, '{filePath}', null);";
                
                // 디비커넥터로 수정
                int affected = DBconnector.GetInstance().NonQuery(query);

                if (affected > 0)
                {
                    MessageBox.Show("회원가입 완료");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("회원가입 실패");
                }
            }
        }

        private void ChangeProfileButton_Click(object sender, EventArgs e)
        {
            var FD = new OpenFileDialog();

            if (FD.ShowDialog() == DialogResult.OK)
            {
                FilePath.Text = FD.FileName;
                ProfileImageBox.Image = Image.FromFile(FD.FileName);
            }
        }
    }
}
