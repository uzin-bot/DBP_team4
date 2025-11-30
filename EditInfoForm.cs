using DBP_WinformChat;
using System.Data;

namespace leehaeun
{
    public partial class EditInfoForm : Form
    {
        private DataRow CurrProfile { get; set; } = UserInfo.Profile.Rows[0];   // 기본 프로필

        private DataTable MulProfileMember { get; set; }

        // 디자인 폼에서 비밀번호 확인 삭제

        public EditInfoForm()
        {
            InitializeComponent();
            LoadUserInfo();
            LoadProfileInfo();
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
        private void LoadProfileInfo()
        {
            NicknameBox.Text = CurrProfile["Nickname"].ToString();
            NicknameBox.Tag = CurrProfile["ProfileId"].ToString();
            DeptLabel.Text = UserInfo.User["DeptName"].ToString();
            StatusBox.Text = CurrProfile["StatusMessage"].ToString();

            string? base64String = CurrProfile["ProfileImage"].ToString();
            if (!string.IsNullOrEmpty(base64String))
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                ProfileImagePBox.Image = Image.FromStream(ms);
                ProfileImagePBox.Tag = base64String;
            }
            else
            {
                ProfileImagePBox.Image = DBP_WinformChat.Properties.Resources._default;
                ProfileImagePBox.Tag = null;
            }

            bool isDefault = Convert.ToBoolean(CurrProfile["IsDefault"]);
            if (isDefault)
            {
                // 멀티 프로필 탭 기본 프로필
                NicknameLabel.Text = UserInfo.Profile.Rows[0]["Nickname"].ToString();
                string? dbase64String = UserInfo.Profile.Rows[0]["ProfileImage"].ToString();
                if (!string.IsNullOrEmpty(dbase64String))
                {
                    byte[] imageBytes = Convert.FromBase64String(dbase64String);
                    using var ms = new MemoryStream(imageBytes);
                    ProfileImageMBox.Image = Image.FromStream(ms);
                }
                else
                {
                    ProfileImageMBox.Image = DBP_WinformChat.Properties.Resources._default;
                }

                // 멀티프로필 멤버 관리 창 끄기
                label2.Visible = false;
                EditMButton.Visible = false;
                MemberFLP.Visible = false;
            }
            else
            {
                // 멀티프로필 멤버 관리 창 켜기
                label2.Visible = true;
                EditMButton.Visible = true;
                MemberFLP.Visible = true;

                // 멀티 프로필 멤버 리스트 로딩
                LoadMulProfileMemberList();
            }

            tabControl.SelectedIndex = 0;
        }

        // 멀티 프로필 리스트 로딩
        private void LoadMulProfileList()
        {
            // 기본 프로필 제외
            for (int i = 1; i < UserInfo.Profile.Rows.Count; i++)
            {
                DataRow row = UserInfo.Profile.Rows[i];
                AddNewProfile(row);
            }
        }

        // 새 멀티프로필 패널 추가
        private void AddNewProfile(DataRow row)
        {
            Panel newPanel = new Panel();
            newPanel.Size = new Size(216, 35);
            newPanel.Location = new Point(2, 41);

            PictureBox profileImage = new PictureBox();
            profileImage.Location = new Point(0, 3);
            profileImage.Size = new Size(30, 29);
            profileImage.SizeMode = PictureBoxSizeMode.StretchImage;
            profileImage.Image = DBP_WinformChat.Properties.Resources._default;

            Label nicknameLabel = new Label();
            nicknameLabel.Location = new Point(35, 10);
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
                CurrProfile = row;
                LoadProfileInfo();
            };

            deleteButton.Click += (s, args) =>
            {
                DialogResult result = MessageBox.Show("삭제 하시겠습니까?",
                "확인",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question
                );
                if (result == DialogResult.OK)
                {
                    // DB에서 해당 프로필, 해당 프로필과 연결된 맵 삭제
                    int profileId = Convert.ToInt32(row["ProfileID"]);
                    string query = $"DELETE FROM Profile WHERE ProfileId = {profileId};";
                    DBconnector.GetInstance().NonQuery(query);
                    string mquery = $"DELETE FROM UserProfileMap WHERE OwnerUserId = {LoginForm.UserId} AND ProfileId = {profileId}";
                    DBconnector.GetInstance().NonQuery(mquery);
                    ProfileFLP.Controls.Remove(newPanel);
                    newPanel.Dispose();
                    UserInfo.GetProfileInfo();
                }
            };
        }

        // 멀티 프로필 매핑 정보
        private void GetMulProfileMember()
        {
            // 현재 프로필의 멀티 프로필 멤버 정보
            string query = $@"
                SELECT
                    u.UserId,
                    u.Name,
                    p.ProfileId,
                    p.Nickname,
                    p.ProfileImage,
                    p.StatusMessage,
                    d.DeptName
                FROM (
                    SELECT 
                        o.TargetUserId,
                        COALESCE(t.ProfileId, o.ProfileId) AS FinalProfileId
                    FROM UserProfileMap o
                    LEFT JOIN UserProfileMap t
                        ON t.OwnerUserId = o.TargetUserId
                       AND t.TargetUserId = {LoginForm.UserId}
                    WHERE o.OwnerUserId = {LoginForm.UserId}
                      AND o.ProfileId = {CurrProfile["ProfileId"]}
                ) AS map
                JOIN User u
                    ON u.UserId = map.TargetUserId
                JOIN Profile p
                    ON p.ProfileId = map.FinalProfileId
                LEFT JOIN Department d
                    ON u.DeptId = d.DeptId;";
            // DataTable에 저장
            MulProfileMember = DBconnector.GetInstance().Query(query);
        }

        private void LoadMulProfileMemberList()
        {
            GetMulProfileMember();
            // 멤버 관리 창 초기화
            MemberFLP.Controls.Clear();

            // 멀티 프로필 멤버 추가
            foreach (DataRow row in MulProfileMember.Rows)
            {
                AddNewMember(row);
            }
        }

        // 멀티 프로필 멤버 관리
        private void AddNewMember(DataRow row)
        {
            Panel newPanel = new Panel();
            newPanel.Size = new Size(216, 35);
            newPanel.Location = new Point(2, 2);

            PictureBox profileImage = new PictureBox();
            profileImage.Location = new Point(3, 3);
            profileImage.Size = new Size(30, 29);
            profileImage.SizeMode = PictureBoxSizeMode.StretchImage;
            profileImage.Image = DBP_WinformChat.Properties.Resources._default;

            Label nicknameLabel = new Label();
            nicknameLabel.Location = new Point(38, 10);
            nicknameLabel.AutoSize = true;
            nicknameLabel.Text = row["Nickname"].ToString();

            Button deleteButton = new Button();
            deleteButton.Text = "삭제";
            deleteButton.Size = new Size(39, 22);
            deleteButton.Location = new Point(168, 6);

            newPanel.Controls.Add(profileImage);
            newPanel.Controls.Add(nicknameLabel);
            newPanel.Controls.Add(deleteButton);

            MemberFLP.Controls.Add(newPanel);

            // 부서이름 이름(닉네임) 형식
            string name = row["DeptName"].ToString();
            name += " ";
            name += row["Name"].ToString();
            name += "(";
            name += row["Nickname"].ToString();
            name += ")";
            nicknameLabel.Text = name;

            string? base64String = row["ProfileImage"].ToString();
            if (!string.IsNullOrEmpty(base64String))
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                profileImage.Image = Image.FromStream(ms);
            }

            // DB에서 프로필 매핑 정보 및 패널 삭제
            deleteButton.Click += (s, args) =>
            {
                int targetUserId = Convert.ToInt32(row["UserId"]);
                string query = $"DELETE FROM UserProfileMap WHERE OwnerUserId = {LoginForm.UserId} AND TargetUserId = {targetUserId};";
                DBconnector.GetInstance().NonQuery(query);
                ProfileFLP.Controls.Remove(newPanel);
                newPanel.Dispose();
            };
        }

        // 기본 프로필 탭 컨트롤
        // 프로필 이미지 변경
        private void ChangeProfileImageButton_Click(object sender, EventArgs e)
        {
            var FD = new OpenFileDialog();

            if (FD.ShowDialog() == DialogResult.OK)
            {
                using var img = Image.FromFile(FD.FileName);

                int maxSize = 100;
                double ratio = Math.Min((double)maxSize / img.Width, (double)maxSize / img.Height);
                using var resized = new Bitmap(img, new Size((int)(img.Width * ratio), (int)(img.Height * ratio)));

                ProfileImagePBox.Image = (Image)resized.Clone();

                using var ms = new MemoryStream();
                resized.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ProfileImagePBox.Tag = Convert.ToBase64String(ms.ToArray());
            }

            FD.Dispose();
        }

        // 저장 버튼
        private void SavePButton_Click(object sender, EventArgs e)
        {
            SaveProfileInfo();
        }

        // 프로필 저장
        private void SaveProfileInfo()
        {
            // 닉네임 변경
            if (NicknameBox.Text != CurrProfile["Nickname"].ToString())
            {
                string query = $"UPDATE Profile SET Name = '{NicknameBox.Text}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("닉네임 변경 실패");
            }

            // 상태메시지 변경
            if (StatusBox.Text != CurrProfile["StatusMessage"].ToString())
            {
                string query = $"UPDATE Profile SET Address = '{StatusBox.Text}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("상태메시지 변경 실패");
            }

            // 프로필 이미지 변경
            if (ProfileImagePBox.Tag?.ToString() != CurrProfile["ProfileImage"].ToString() &&
                string.IsNullOrEmpty(ProfileImagePBox.Tag.ToString()))
            {
                string query = $"UPDATE Profile SET ProfileImage = '{ProfileImagePBox.Tag?.ToString()}' WHERE UserId = '{LoginForm.UserId}';";
                int affected = DBconnector.GetInstance().NonQuery(query);
                if (affected <= 0) MessageBox.Show("프로필 이미지 변경 실패");
            }

            MessageBox.Show("프로필 저장 완료");
            UserInfo.GetProfileInfo();
            LoadProfileInfo();
        }

        // 멤버 추가 버튼
        private void EditMButton_Click(object sender, EventArgs e)
        {
            var s = new SearchUserForm();
            if (s.ShowDialog() == DialogResult.OK)
            {
                foreach (int targetId in s.selectedUserIds)
                {
                    // 매핑 정보 추가
                    string query = $"INSERT INTO UserProfileMap(OwnerUserId, TargetUserId, ProfileId) VALUES({LoginForm.UserId}, {targetId}, {CurrProfile["ProfileId"]});";
                    DBconnector.GetInstance().NonQuery(query);
                }
            }

            LoadMulProfileMemberList();
        }

        // 취소 버튼
        private void CancelPButton_Click(object sender, EventArgs e)
        {
            CancelCheck();
        }

        // 멀티 프로필 탭 컨트롤
        // 기본 프로필 관리 버튼
        private void EditButton_Click(object sender, EventArgs e)
        {
            // 현재 프로필을 기본 프로필로 변경
            CurrProfile = UserInfo.Profile.Rows[0];
            LoadProfileInfo();
        }

        // 새 멀티프로필 추가
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

        // 사용자 정보 저장 버튼
        private void SaveIButton_Click(object sender, EventArgs e)
        {
            SaveUserInfo();
        }

        // 취소 버튼
        private void CancleIButton_Click(object sender, EventArgs e)
        {
            CancelCheck();
        }

        // 사용자 정보 저장
        private void SaveUserInfo()
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

        private void CancelCheck()
        {
            DialogResult result = MessageBox.Show("변경 내용을 취소하시겠습니까?",
                "확인",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question
                );
            if (result == DialogResult.OK)
            {
                // 수정 전 정보
                LoadUserInfo();
                LoadProfileInfo();
            }
        }
    }
}
