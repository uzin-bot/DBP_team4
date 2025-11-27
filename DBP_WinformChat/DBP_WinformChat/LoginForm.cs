using DBP_WinformChat;
using System.Data;

namespace leehaeun
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            if (LoadConfig()) Login();
        }

        // UserId 저장
        public static int UserId { get; private set; } = 0;

        // 로그아웃 확인
        public static bool Logout { private get; set; } = false;

        // 로그인 버튼
        private void LoginButton_Click(object sender, EventArgs e)
        {
            Login();
        }

        // 로그인
        private void Login()
        {
            string id = IdBox.Text;
            string pw = PwBox.Text;
            string pwHash = Sha256.Instance.HashSHA256(pw);

            string query = $"SELECT UserId FROM User WHERE LoginId = '{id}' AND PasswordHash = '{pwHash}';";
            DataTable dt = DBconnector.GetInstance().Query(query);
            bool result = dt != null && dt.Rows.Count > 0;

            if (result)
            {
                // 로그인 성공
                UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                UserInfo.GetUserInfo();
                UserInfo.GetProfileInfo();
                UserInfo.GetMulProfileInfo();

                // 채팅 목록 폼
                FormHide();
                var chatListForm = new 남예솔.chatlist();
                chatListForm.ShowDialog();

                // 로그아웃이면 폼 열기
                if (Logout) FormShow();
                else this.Close();
            }
            else
            {
                MessageBox.Show("로그인 실패");
            }
        }

        // 회원가입버튼
        private void SignUpLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormHide();
            var signUp = new SignUpForm();
            signUp.ShowDialog();
            FormShow();
        }

        // 폼 열릴 때
        private void FormShow()
        {
            UserId = 0;
            UserInfo.ResetDataTable();
            LoadConfig();
            this.Show();
        }

        // 폼 닫힐 때
        private void FormHide()
        {
            SaveConfig();
            IdBox.Text = "";
            PwBox.Text = "";
            Logout = false;
            this.Hide();
        }

        // 로그인 설정 저장
        public void SaveConfig()
        {
            DBP_WinformChat.Properties.Settings.Default.RememberMe = RememberMe.Checked;
            DBP_WinformChat.Properties.Settings.Default.SaveInfo = SaveInfo.Checked;
            if (RememberMe.Checked || SaveInfo.Checked)
            {
                DBP_WinformChat.Properties.Settings.Default.SaveId = IdBox.Text;
                DBP_WinformChat.Properties.Settings.Default.SavePw = PwBox.Text;
            }
            DBP_WinformChat.Properties.Settings.Default.Save();
        }

        // 로그인 설정 불러오기
        public bool LoadConfig()
        {
            RememberMe.Checked = DBP_WinformChat.Properties.Settings.Default.RememberMe;
            SaveInfo.Checked = DBP_WinformChat.Properties.Settings.Default.SaveInfo;
            if (RememberMe.Checked || SaveInfo.Checked)
            {
                IdBox.Text = DBP_WinformChat.Properties.Settings.Default.SaveId;
                PwBox.Text = DBP_WinformChat.Properties.Settings.Default.SavePw;
            }

            if (RememberMe.Checked) return true;
            else return false;
        }
    }
}