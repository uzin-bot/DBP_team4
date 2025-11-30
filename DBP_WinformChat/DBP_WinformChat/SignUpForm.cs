using DBP_WinformChat;
using System.Data;

namespace leehaeun
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
            LoadDepartments();
        }
        // 디자인 폼 비밀번호 확인 삭제

        // 부서아이디 로드 해오기
        private void LoadDepartments()
        {
            string query = "SELECT DeptId, DeptName FROM Department";
            DataTable dt = DBconnector.GetInstance().Query(query);

            DeptBox.DataSource = dt;
            DeptBox.DisplayMember = "DeptName";  // 화면에 보이는 것
            DeptBox.ValueMember = "DeptId";      // 실제 값
        }

        // 아이디 중복 검사
        private void CheckIDButton_Click(object sender, EventArgs e)
        {
            string id = IdBox.Text;

            // User 테이블에서 LoginId 중복 체크
            string query = $"SELECT COUNT(*) FROM User WHERE LoginId = '{id}';";

            DataTable dt = DBconnector.GetInstance().Query(query);
            bool result = dt != null && dt.Rows.Count > 0;

            if (result)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
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
            int deptId = Convert.ToInt32(DeptBox.SelectedValue); // 실제 DeptId 가져오기!
            string? profileImage = ProfileImageBox.Tag?.ToString();

            // 필수 항목 체크
            if (string.IsNullOrWhiteSpace(id) ||
                string.IsNullOrWhiteSpace(pw) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(nickname) ||
                string.IsNullOrWhiteSpace(address) ||
                string.IsNullOrWhiteSpace(zipCode) ||
                deptId < 0)
            {
                MessageBox.Show("모든 항목을 입력해주세요.");
                return; // 이거 실행하고 확인해보기
            }
            else if (IsDuplicate.Text == "이미 사용 중인 아이디입니다.")
            {
                // 직접 쓴 문구 -> IsDuplicate.Text
                MessageBox.Show(IsDuplicate.Text);
            }
            else
            {
                // 각 쿼리/결과 변수 이름 변경
                // User 테이블에 정보 저장
                string pwHash = Sha256.Instance.HashSHA256(pw);
                string uquery = $@"INSERT INTO
                    User(LoginId, PasswordHash, Name, Nickname, Address, ZipCode, DeptId, Role)
                    VALUES('{id}', '{pwHash}', '{name}', '{nickname}', '{address}', {zipCode}, {deptId}, 'user');";
                int uaff = DBconnector.GetInstance().NonQuery(uquery);
                if (uaff <= 0) MessageBox.Show("회원가입 실패");

                // 방금 가입한 계정의 UserId
                string iquery = $"SELECT UserId FROM User WHERE LoginId = '{id}';";
                DataTable dt = DBconnector.GetInstance().Query(iquery);
                int UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);

                // 새 프로필 생성
                string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string pquery = $@"INSERT INTO
                    Profile(UserId, NickName, IsDefault, CreatedAt)
                    VALUES({UserId}, '{nickname}', 1, '{now}');";
                int paff = DBconnector.GetInstance().NonQuery(pquery);
                if (paff <= 0) MessageBox.Show("프로필 생성 실패");

                // 프로필 이미지 있으면 업데이트
                if (!string.IsNullOrEmpty(profileImage))
                {
                    string mquery = $"UPDATE Profile SET ProfileImage = '{profileImage}' WHERE UserId = {UserId} AND IsDefault = 1;";
                    int maff = DBconnector.GetInstance().NonQuery(mquery);
                    if (maff <= 0) MessageBox.Show("프로필 이미지 업로드 실패");
                }

                MessageBox.Show("회원가입 완료");
                this.Close();
            }
        }

        // 프로필 이미지 변경
        private void ChangeProfileButton_Click(object sender, EventArgs e)
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

        // 주소 검색
        private void SearchAddressButton_Click(object sender, EventArgs e)
        {
            var searchAddress = new SearchAddressForm();
            if (searchAddress.ShowDialog() == DialogResult.OK)
            {
                AddressBox.Text = searchAddress.selectedAddress;
                ZipCodeBox.Text = searchAddress.selectedZipCode;
            }
        }
    }
}
