using DBP_WinformChat;
using System.Data;

namespace leehaeun
{
    public partial class EditInfoForm : Form
    {
        public EditInfoForm()
        {
            InitializeComponent();
            LoadUserInfo();
            LoadProfileInfo(UserInfo.Profile.Rows[0]);  // 기본 프로필
            LoadMulProfileList();
        }

        // 사용자 정보 로딩
        private void LoadUserInfo()
        {
            IdBox.Text = UserInfo.User["LoginId"].ToString();
            NameBox.Text = UserInfo.User["Name"].ToString();
            ZipCodeBox.Text = UserInfo.User["ZipCode"].ToString();
            AddressBox.Text = UserInfo.User["address"].ToString();
            DeptBox.Text = UserInfo.User["DeptName"].ToString();
        }

        // 프로필 정보 로딩
        private void LoadProfileInfo(DataRow row)
        {
            NicknameBox.Text = row["Nickname"].ToString();
            NicknameBox.Tag = row["ProfileId"].ToString();
            DeptLabel.Text = UserInfo.User["DeptName"].ToString();
            StatusBox.Text = row["StatusMessage"].ToString();
            NicknameLabel.Text = row["Nickname"].ToString();

            string? base64String = row["ProfileImage"].ToString();
            if (!string.IsNullOrEmpty(base64String))
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                ProfileImagePBox.Image = Image.FromStream(ms);
                ProfileImageMBox.Image = Image.FromStream(ms);
                ProfileImagePBox.Tag = base64String;
            }
            else
            {
                ProfileImagePBox.Image = DBP_WinformChat.Properties.Resources._default;
                ProfileImagePBox.Tag = null;
            }

            bool result = Convert.ToBoolean(row["IsDefault"]);
            if (result) EditMemberLabel.Text = "기본 프로필";
            else EditMemberLabel.Text = "멤버 관리";

            tabControl.SelectedIndex = 0;
        }

        private void LoadMulProfileList()
        {
            for (int i = 1; i < UserInfo.Profile.Rows.Count; i++)
            {
                DataRow row = UserInfo.Profile.Rows[i];
                AddNewProfile(row);
            }
        }

        private void ChangeProfileImageButton_Click(object sender, EventArgs e)
        {
            /*
            var FD = new OpenFileDialog();

            if (FD.ShowDialog() == DialogResult.OK)
            {
                using var img = Image.FromFile(FD.FileName);

                int maxSize = 100;
                double ratio = Math.Min((double)maxSize / img.Width, (double)maxSize / img.Height);
                using var resized = new Bitmap(img, new Size((int)(img.Width * ratio), (int)(img.Height * ratio)));

                ProfileImageBox.Image = (Image)resized.Clone();

                using var ms = new MemoryStream();
                resized.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ProfileImageBox.Tag = Convert.ToBase64String(ms.ToArray());
            }

            FD.Dispose();
            */
        }

        // 기본 프로필 탭 컨트롤
        private void SavePButton_Click(object sender, EventArgs e)
        {
            /*
            // UserId, ProfileId 둘 다 비교
            if (NicknameBox.Text != UserInfo.Profile.Rows[0]["Nickame"].ToString())
            {
                string query = $"UPDATE Profile SET Name = '{NicknameBox.Text}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("���� ����");
            }
            if (StatusBox.Text != UserInfo.Profile.Rows[0]["StatusMessage"].ToString())
            {
                string query = $"UPDATE Profile SET Address = '{StatusBox.Text}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("���� ����");
            }
            if (ProfileImageBox.Tag?.ToString() != UserInfo.Profile.Rows[0]["ProfileImage"].ToString())
            {
                if (ProfileImageBox.Tag == null)
                {
                    string query = $"UPDATE Profile SET ProfileImage = null WHERE UserId = '{LoginForm.UserId}';";
                    int affected = DBconnector.GetInstance().NonQuery(query);
                    if (affected <= 0) MessageBox.Show("���� ����");
                }
                else
                {
                    string query = $"UPDATE Profile SET ProfileImage = '{ProfileImageBox.Tag?.ToString()}' WHERE UserId = '{LoginForm.UserId}';";
                    int affected = DBconnector.GetInstance().NonQuery(query);
                    if (affected <= 0) MessageBox.Show("���� ����");
                }
            }

            MessageBox.Show("���� �Ϸ�");
            LoadProfileInfo();
            */
            UserInfo.GetProfileInfo();
        }

        private void CancelPButton_Click(object sender, EventArgs e)
        {
            CancelCheck();
        }

        // 멀티 프로필 탭 컨트롤
        private void EditButton_Click(object sender, EventArgs e)
        {
            LoadProfileInfo(UserInfo.Profile.Rows[0]);
        }

        private void AddMulProfileButton_Click(object sender, EventArgs e)
        {
            // 기본 정보로 새로운 프로필 생성
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string iquery = $@"INSERT INTO
                    Profile(UserId, NickName, IsDefault, CreatedAt)
                    VALUES({LoginForm.UserId}, '새 프로필', 0, '{now}');";
            DBconnector.GetInstance().NonQuery(iquery);

            // 프로필 아이디 가져와서 새 프로필 패널 생성
            string squery = $"SELECT * FROM Profile WHERE UserId = {LoginForm.UserId} ORDER BY ProfileId DESC LIMIT 1;";
            DataTable dt = DBconnector.GetInstance().Query(squery);
            DataRow row = dt.Rows[0];
            AddNewProfile(row);
        }

        private void AddNewProfile(DataRow row)
        {
            Panel newPanel = new Panel();
            newPanel.Size = new Size(216, 35);
            newPanel.Location = new Point(2, 41);

            PictureBox profileImage = new PictureBox();
            profileImage.Location = new Point(3, 3);
            profileImage.Size = new Size(30, 29);
            profileImage.SizeMode = PictureBoxSizeMode.Zoom;
            profileImage.Image = DBP_WinformChat.Properties.Resources._default;

            Label nicknameLabel = new Label();
            nicknameLabel.Location = new Point(38, 10);
            nicknameLabel.AutoSize = true;
            nicknameLabel.Text = row["Nickname"].ToString();

            Button editButton = new Button();
            editButton.Text = "관리";
            editButton.Size = new Size(39, 22);
            editButton.Location = new Point(125, 6);

            Button deleteButton = new Button();
            deleteButton.Text = "삭제";
            deleteButton.Size = new Size(39, 22);
            deleteButton.Location = new Point(168, 6);

            newPanel.Controls.Add(profileImage);
            newPanel.Controls.Add(nicknameLabel);
            newPanel.Controls.Add(editButton);
            newPanel.Controls.Add(deleteButton);

            ProfileFLP.Controls.Add(newPanel);

            string? base64String = row["ProfileImage"].ToString();
            if (!string.IsNullOrEmpty(base64String))
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                profileImage.Image = Image.FromStream(ms);
            }

            nicknameLabel.Text = row["Nickname"].ToString();
            nicknameLabel.Tag = row["ProfileId"].ToString();

            editButton.Click += (s, args) =>
            {
                LoadProfileInfo(row);
            };

            deleteButton.Click += (s, args) =>
            {
                // 진짜 삭제할건지 묻기

                ProfileFLP.Controls.Remove(newPanel);
                newPanel.Dispose();

                // DB에서도 삭제
                string delQuery = $"DELETE FROM Profile WHERE ProfileId = {row["ProfileId"]};";
                DBconnector.GetInstance().NonQuery(delQuery);
                UserInfo.GetProfileInfo();
            };
        }

        // 사용자 정보 탭

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

        private void SaveIButton_Click(object sender, EventArgs e)
        {
            // 이름 변경
            if (NameBox.Text != UserInfo.User["Name"].ToString())
            {
                string query = $"UPDATE User SET Name = '{NameBox.Text}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("변경 실패");
            }

            // 비밀번호 변경
            string pwHash = Sha256.Instance.HashSHA256(PwBox.Text);
            if (pwHash != UserInfo.User["PasswordHash"].ToString())
            {
                string query = $"UPDATE User SET PasswordHash = '{pwHash}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("변경 실패");
            }

            // 주소 변경
            if (AddressBox.Text != UserInfo.User["Address"].ToString() ||
            ZipCodeBox.Text != UserInfo.User["ZipCode"].ToString())
            {
                string query = $"UPDATE User SET Address = '{AddressBox.Text}', ZipCode = '{ZipCodeBox.Text}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("변경 실패");
            }

            MessageBox.Show("변경 완료");

            UserInfo.GetUserInfo();
            LoadUserInfo();
        }

        private void CancleIButton_Click(object sender, EventArgs e)
        {

        }

        private void CancelCheck()
        {
            // 저장 안된 정보 있는지 확인
            // 선택된 탭이 0일때, 2일때
            // this.Close();
            /*
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
            */

            /*
                if (NicknameBox.Text != UserInfo.Profile.Rows[0]["Nickame"].ToString() ||
                StatusBox.Text != UserInfo.Profile.Rows[0]["StatusMessage"].ToString() ||
                ProfileImageBox.Tag?.ToString() != UserInfo.Profile.Rows[0]["ProfileImage"].ToString())
                {
                DialogResult result = MessageBox.Show("���� ������ ����Ͻðڽ��ϱ�?",
                "Ȯ��",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question
                );

                if (result == DialogResult.OK) this.Close();
                }

                this.Close();
            */
        }
    }
}
