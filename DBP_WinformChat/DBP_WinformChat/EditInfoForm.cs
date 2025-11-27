using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace leehaeun
{
    public partial class EditInfoForm : Form
    {
        public EditInfoForm()
        {
            InitializeComponent();
            LoadDefaultProfileInfo();
        }

        private void LoadDefaultProfileInfo()
        {
            NicknameBox.Text = UserInfo.Profile.Rows[0]["Nickname"].ToString();
            NicknameBox.Tag = UserInfo.Profile.Rows[0]["ProfileId"].ToString();
            DeptLabel.Text = UserInfo.User.Rows[0]["DeptName"].ToString();
            StatusBox.Text = UserInfo.Profile.Rows[0]["StatusMessage"].ToString();
            NicknameLabel.Text = UserInfo.Profile.Rows[0]["Nickname"].ToString();

            string? base64String = UserInfo.Profile.Rows[0]["ProfileImage"].ToString();
            if (!string.IsNullOrEmpty(base64String))
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                ProfileImageBox0.Image = Image.FromStream(ms);
                ProfileImageBox1.Image = Image.FromStream(ms);
                ProfileImageBox0.Tag = base64String;
            }

            EditMemberLabel.Text = "기본 프로필";
        }

        private void LoadMulProfileList()
        {
            // 멀티 프로필 리스트
        }

        private void LoadMulProfileInfo()
        {
            // 멀티 프로필 상세 정보 로딩(탭1)
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {


            UserInfo.GetInfo();
        }

        private void CancelButton0_Click(object sender, EventArgs e)
        {
            CancelCheck();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            LoadDefaultProfileInfo();
        }

        private void CancelCheck()
        {
            // 저장 안된 정보 있는지 확인
        }

        private void AddMulProfileButton_Click(object sender, EventArgs e)
        {
            // 버튼 추가
            // 버튼 이벤트 추가
            // DB에 기본 정보로 프로필 생성 쿼리 보내기
        }

        /* 
        private void LoadUserInfo()
        {
            UserInfo.GetUserInfo();

            IdBox.Text = UserInfo.User.Rows[0]["LoginId"].ToString();
            NameBox.Text = UserInfo.User.Rows[0]["Name"].ToString();
            ZipCodeBox.Text = UserInfo.User.Rows[0]["ZipCode"].ToString();
            AddressBox.Text = UserInfo.User.Rows[0]["address"].ToString();
            DeptBox.Text = UserInfo.User.Rows[0]["DeptName"].ToString();
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


        // 임시

        private void LoadProfileInfo()
        {
            UserInfo.GetUserInfo();
            UserInfo.GetProfileInfo();

            NicknameBox.Text = UserInfo.Profile.Rows[0]["Nickname"].ToString();
            Deptlabel.Text = UserInfo.User.Rows[0]["DeptId"].ToString();
            // DeptLabel.Text = UserInfo.User.Rows[0]["DeptName"].ToString();
            StatusBox.Text = UserInfo.Profile.Rows[0]["StatusMessage"].ToString();

            // 멀티프로필창 기본프로필 별명 설정
            NicknameLabel.Text = UserInfo.Profile.Rows[0]["Nickname"].ToString();

            string? base64String = UserInfo.Profile.Rows[0]["ProfileImage"].ToString();
            if (!string.IsNullOrEmpty(base64String))
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                ProfileImageBox.Image = Image.FromStream(ms);
                ProfileImageBox.Tag = base64String;

                // 멀티프로필창 기본프로필 별명 설정
                ProfileImage.Image = Image.FromStream(ms);
            }
        }

        private void LoadMulProfileInfo()
        {
            UserInfo.GetMulProfileInfo();

            foreach (DataRow row in UserInfo.MulProfile.Rows)
            {
                Panel newPanel = new Panel();
                newPanel.Size = new Size(272, 35);
                newPanel.Margin = new Padding(0, 0, 0, 6);

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
                editButton.Text = "����";
                editButton.Size = new Size(52, 22);
                editButton.Location = new Point(159, 6);

                Button deleteButton = new Button();
                deleteButton.Text = "����";
                deleteButton.Size = new Size(52, 22);
                deleteButton.Location = new Point(215, 6);

                newPanel.Controls.Add(profileImage);
                newPanel.Controls.Add(nicknameLabel);
                newPanel.Controls.Add(editButton);
                newPanel.Controls.Add(deleteButton);

                flowProfiles.Controls.Add(newPanel);

                string? base64String = row["ProfileImage"].ToString();
                if (!string.IsNullOrEmpty(base64String))
                {
                    byte[] imageBytes = Convert.FromBase64String(base64String);
                    using var ms = new MemoryStream(imageBytes);
                    profileImage.Image = Image.FromStream(ms);
                }

                nicknameLabel.Text = UserInfo.MulProfile.Rows[0]["Nickname"].ToString();
                nicknameLabel.Tag = UserInfo.MulProfile.Rows[0]["ProfileId"].ToString();

                deleteButton.Click += (s, args) =>
                {
                    flowProfiles.Controls.Remove(newPanel);
                    newPanel.Dispose();

                    // DB에서도 삭제
                    string delQuery = $"DELETE FROM Profile WHERE ProfileId = {row["ProfileId"]};";
                    DBconnector.GetInstance().NonQuery(delQuery);
                };

                editButton.Click += (s, args) =>
                {
                    NicknameBox.Text = row["Nickname"].ToString();
                    StatusBox.Text = row["StatusMessage"].ToString();

                    string? imgStr = row["ProfileImage"].ToString();
                    if (!string.IsNullOrEmpty(imgStr))
                    {
                        byte[] bytes = Convert.FromBase64String(imgStr);
                        using var ms2 = new MemoryStream(bytes);
                        ProfileImageBox.Image = Image.FromStream(ms2);
                        ProfileImageBox.Tag = imgStr;
                    }
                    else
                    {
                        ProfileImageBox.Image = DBP_WinformChat.Properties.Resources._default;
                        ProfileImageBox.Tag = null;
                    }

                    tabControl1.SelectedIndex = 0;
                };
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
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
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
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
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // 기본 프로필 수정
            LoadProfileInfo();
            tabControl1.SelectedIndex = 0;
        }

        private void ChangeProfileImageButton_Click(object sendor, EventArgs e)
        {
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
        }

        private void AddMulProfileButton_Click(object sender, EventArgs e)
        {
            // DB에 새 Row 추가

            Panel newPanel = new Panel();
            newPanel.Size = new Size(272, 35);
            newPanel.Margin = new Padding(0, 0, 0, 6);

            PictureBox profileImage = new PictureBox();
            profileImage.Location = new Point(3, 3);
            profileImage.Size = new Size(30, 29);
            profileImage.SizeMode = PictureBoxSizeMode.Zoom;
            profileImage.Image = DBP_WinformChat.Properties.Resources._default;

            Label nicknameLabel = new Label();
            nicknameLabel.Location = new Point(38, 10);
            nicknameLabel.AutoSize = true;
            nicknameLabel.Text = "�� ������";

            Button editButton = new Button();
            editButton.Text = "����";
            editButton.Size = new Size(52, 22);
            editButton.Location = new Point(159, 6);

            editButton.Click += (s, args) =>
            {
                // 데이터 반영 후
                // 탭 이동
            };

            Button deleteButton = new Button();
            deleteButton.Text = "����";
            deleteButton.Size = new Size(52, 22);
            deleteButton.Location = new Point(215, 6);

            deleteButton.Click += (s, args) =>
            {
                flowProfiles.Controls.Remove(newPanel);
                newPanel.Dispose();
            };

            newPanel.Controls.Add(profileImage);
            newPanel.Controls.Add(nicknameLabel);
            newPanel.Controls.Add(editButton);
            newPanel.Controls.Add(deleteButton);

            flowProfiles.Controls.Add(newPanel);
        }
        */
    }
}
