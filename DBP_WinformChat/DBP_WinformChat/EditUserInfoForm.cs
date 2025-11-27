using DBP_WinformChat;
using Microsoft.VisualBasic.Logging;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace leehaeun
{
    public partial class EditUserInfoForm : Form
    {
        public EditUserInfoForm()
        {
            InitializeComponent();
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            UserInfo.GetUserInfo();

            IdBox.Text = UserInfo.User.Rows[0]["LoginId"].ToString();
            PwBox.Text = "";
            NameBox.Text = UserInfo.User.Rows[0]["Name"].ToString();
            ZipCodeBox.Text = UserInfo.User.Rows[0]["ZipCode"].ToString();
            AddressBox.Text = UserInfo.User.Rows[0]["address"].ToString();
            DeptBox.Text = UserInfo.User.Rows[0]["DeptId"].ToString();
            // DeptBox.Text = User.Rows[0]["DeptName"].ToString();
            // DB 데이터 추가 후 아래로 변경
        }

        // 회원가입 폼 내부 메서드와 동일
        private void SearchAddressButton_Click(object sender, EventArgs e)
        {
            var searchAddress = new SearchAddressForm();
            if (searchAddress.ShowDialog() == DialogResult.OK)
            {
                AddressBox.Text = searchAddress.selectedAddress;
                ZipCodeBox.Text = searchAddress.selectedZipCode;
            }
        }

        private void SaveInfoButton_Click(object sender, EventArgs e)
        {
            // 이름 변경
            if (NameBox.Text != UserInfo.User.Rows[0]["Name"].ToString())
            {
                string query = $"UPDATE User SET Name = '{NameBox.Text}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("변경 실패");
            }

            // 비밀번호 변경
            string pwHash = Sha256.Instance.HashSHA256(PwBox.Text);
            if (pwHash != UserInfo.User.Rows[0]["PasswordHash"].ToString())
            {
                string query = $"UPDATE User SET PasswordHash = '{pwHash}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("변경 실패");
            }

            // 주소 변경
            if (AddressBox.Text != UserInfo.User.Rows[0]["Address"].ToString() ||
                ZipCodeBox.Text != UserInfo.User.Rows[0]["ZipCode"].ToString())
            {
                string query = $"UPDATE User SET Address = '{AddressBox.Text}', ZipCode = '{ZipCodeBox.Text}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("변경 실패");
            }

            MessageBox.Show("변경 완료");

            LoadUserInfo();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (NameBox.Text != UserInfo.User.Rows[0]["Name"].ToString() ||
                !string.IsNullOrEmpty(PwBox.Text) ||
                AddressBox.Text != UserInfo.User.Rows[0]["Address"].ToString() ||
                ZipCodeBox.Text != UserInfo.User.Rows[0]["ZipCode"].ToString())
            {
                DialogResult result = MessageBox.Show("변경 내용을 취소하시겠습니까?",
                    "확인",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.OK) this.Close();
            }
            else this.Close();
        }
    }
}