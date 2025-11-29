using System;
using System.Drawing;
using System.Windows.Forms;

namespace MessengerApp
{
    public partial class AdminMainForm : Form
    {
        public AdminMainForm()
        {
            // Designer.cs 파일의 InitializeComponent()를 호출하여 UI를 생성합니다.
            InitializeComponent();

            // 폼 초기 설정 및 이벤트 연결
            InitializeFormSettings();
            AttachMenuEvents();

            // 시작 시 대시보드 화면을 로드
            ShowDashboard();
        }

        private void InitializeFormSettings()
        {
            this.Text = "관리자 패널";
        }

        private void AttachMenuEvents()
        {
            // 모든 메뉴 버튼에 동일한 클릭 이벤트 핸들러 연결
            btnDashboard.Click += MenuButton_Click;
            btnDepartment.Click += MenuButton_Click;
            btnUsers.Click += MenuButton_Click;
            btnChatSearch.Click += MenuButton_Click;
            btnLoginLog.Click += MenuButton_Click;
            btnPermission.Click += MenuButton_Click;

            // 초기 활성 버튼 설정
            SetActiveButton(btnDashboard);
        }

        /// <summary>
        /// 메뉴 버튼 클릭 시 화면 전환 및 스타일 변경 처리
        /// </summary>
        private void MenuButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            SetActiveButton(clickedButton);

            // 클릭된 버튼의 Text를 기반으로 화면 전환
            string screenText = clickedButton.Text.Replace(" ", "").Replace("📊", "").Replace("🏢", "").Replace("👥", "").Replace("💬", "").Replace("🕐", "").Replace("🔒", "");

            // 화면을 지우고 선택된 화면을 로드
            ClearContent();

            switch (screenText)
            {
                case "대시보드":
                    ShowDashboard();
                    break;
                case "부서관리":
                    ShowDepartmentManage();
                    break;
                case "사용자관리":
                    ShowUserManage();
                    break;
                case "대화내용검색":
                    ShowChatSearch();
                    break;
                case "로그인기록":
                    ShowLoginLog();
                    break;
                case "권한관리":
                    ShowPermissionManage();
                    break;
            }
        }

        private void SetActiveButton(Button activeBtn)
        {
            Color defaultBg = Color.FromArgb(45, 55, 72);
            Color defaultFg = Color.FromArgb(203, 213, 224);
            Color activeBg = Color.FromArgb(49, 130, 206);
            Color activeFg = Color.White;

            foreach (Control ctrl in pnlLeft.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = defaultBg;
                    btn.ForeColor = defaultFg;
                }
            }
            activeBtn.BackColor = activeBg;
            activeBtn.ForeColor = activeFg;
        }

        private void ClearContent()
        {
            pnlContent.Controls.Clear();
        }

        // --- 화면별 UI 로드 (형식만 유지, 데이터 하드코딩 없음) ---
        // UI 컴포넌트 선언 및 초기화는 Designer.cs에 있습니다.

        private void ShowDashboard()
        {
            // pnlContent의 크기에 맞춰 카드 크기 및 위치 조정이 필요할 경우, 
            // 여기에 컨트롤 생성 및 추가 로직을 작성합니다.

            // 기존 코드의 CreateCard/CreateTitle 로직을 사용하여 UI를 동적으로 생성
            pnlContent.Controls.Add(CreateTitle("대시보드"));

            int cardW = 300;
            int cardH = 120;
            int cardSpacing = 30;

            // 카드 1
            Panel card1 = CreateCard(0, 70, cardW, cardH);
            card1.Controls.Add(new Label { Text = "전체 사용자", Font = new Font("맑은 고딕", 9F), ForeColor = Color.Gray, Location = new Point(20, 20), AutoSize = true });
            card1.Controls.Add(new Label { Text = "???명", Font = new Font("맑은 고딕", 28F, FontStyle.Bold), ForeColor = Color.FromArgb(45, 55, 72), Location = new Point(20, 50), AutoSize = true });
            pnlContent.Controls.Add(card1);

            // 카드 2
            Panel card2 = CreateCard(cardW + cardSpacing, 70, cardW, cardH);
            card2.Controls.Add(new Label { Text = "전체 부서", Font = new Font("맑은 고딕", 9F), ForeColor = Color.Gray, Location = new Point(20, 20), AutoSize = true });
            card2.Controls.Add(new Label { Text = "??개", Font = new Font("맑은 고딕", 28F, FontStyle.Bold), ForeColor = Color.FromArgb(45, 55, 72), Location = new Point(20, 50), AutoSize = true });
            pnlContent.Controls.Add(card2);

            // 카드 3
            Panel card3 = CreateCard(2 * (cardW + cardSpacing), 70, cardW, cardH);
            card3.Controls.Add(new Label { Text = "오늘 접속자", Font = new Font("맑은 고딕", 9F), ForeColor = Color.Gray, Location = new Point(20, 20), AutoSize = true });
            card3.Controls.Add(new Label { Text = "???명", Font = new Font("맑은 고딕", 28F, FontStyle.Bold), ForeColor = Color.FromArgb(45, 55, 72), Location = new Point(20, 50), AutoSize = true });
            pnlContent.Controls.Add(card3);
        }

        private void ShowDepartmentManage()
        {
            pnlContent.Controls.Add(CreateTitle("부서 관리"));

            // 버튼 추가 (오른쪽 상단에 배치)
            Button btnAdd = CreateBlueButton("+ 부서 추가", new Point(850, 5), new Size(120, 40));
            pnlContent.Controls.Add(btnAdd);

            // 검색 카드 및 DataGridView 추가
            Panel searchCard = CreateCard(0, 70, 970, 60);
            TextBox txtSearch = new TextBox { Location = new Point(15, 18), Size = new Size(940, 25), Font = new Font("맑은 고딕", 10F), PlaceholderText = "부서명 검색..." };
            searchCard.Controls.Add(txtSearch);
            pnlContent.Controls.Add(searchCard);

            DataGridView dgv = CreateDGV(new Point(0, 145), new Size(970, 450));
            dgv.Columns.Add("Name", "부서명");
            dgv.Columns.Add("Count", "인원수");
            dgv.Columns.Add(new DataGridViewButtonColumn { Text = "수정", UseColumnTextForButtonValue = true, Width = 100 });
            dgv.Columns.Add(new DataGridViewButtonColumn { Text = "삭제", UseColumnTextForButtonValue = true, Width = 100 });
            pnlContent.Controls.Add(dgv);
        }

        private void ShowUserManage()
        {
            pnlContent.Controls.Add(CreateTitle("사용자 관리"));

            Panel searchCard = CreateCard(0, 70, 970, 60);
            TextBox txtSearch = new TextBox { Location = new Point(15, 18), Size = new Size(600, 25), Font = new Font("맑은 고딕", 10F), PlaceholderText = "이름 검색..." };

            ComboBox cboDept = new ComboBox { Location = new Point(630, 18), Size = new Size(200, 25), Font = new Font("맑은 고딕", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            cboDept.Items.AddRange(new object[] { "전체 부서", "개발팀", "디자인팀", "기획팀" }); // 항목은 하드코딩해도 됨
            cboDept.SelectedIndex = 0;

            searchCard.Controls.Add(txtSearch);
            searchCard.Controls.Add(cboDept);
            pnlContent.Controls.Add(searchCard);

            DataGridView dgv = CreateDGV(new Point(0, 145), new Size(970, 450));
            dgv.Columns.Add("Name", "이름");
            dgv.Columns.Add("Dept", "부서");
            dgv.Columns.Add("Email", "이메일");
            dgv.Columns.Add(new DataGridViewButtonColumn { Text = "부서 변경", UseColumnTextForButtonValue = true, Width = 120 });
            dgv.Columns.Add(new DataGridViewButtonColumn { Text = "삭제", UseColumnTextForButtonValue = true, Width = 100 });

            pnlContent.Controls.Add(dgv);
        }

        private void ShowChatSearch()
        {
            pnlContent.Controls.Add(CreateTitle("전체 대화 내용 검색"));

            Panel searchCard = CreateCard(0, 70, 970, 220);

            // 사용자 선택
            searchCard.Controls.Add(new Label { Text = "사용자 선택", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(25, 25), AutoSize = true });
            ComboBox cboUser = new ComboBox { Location = new Point(25, 50), Size = new Size(430, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboUser.Items.AddRange(new object[] { "전체", "사용자 1", "사용자 2" });
            cboUser.SelectedIndex = 0;
            searchCard.Controls.Add(cboUser);

            // 검색 키워드
            searchCard.Controls.Add(new Label { Text = "검색 키워드", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(490, 25), AutoSize = true });
            searchCard.Controls.Add(new TextBox { Location = new Point(490, 50), Size = new Size(430, 25) });

            // 시작일/종료일
            searchCard.Controls.Add(new Label { Text = "시작일", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(25, 100), AutoSize = true });
            searchCard.Controls.Add(new DateTimePicker { Location = new Point(25, 125), Size = new Size(430, 25) });
            searchCard.Controls.Add(new Label { Text = "종료일", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(490, 100), AutoSize = true });
            searchCard.Controls.Add(new DateTimePicker { Location = new Point(490, 125), Size = new Size(430, 25) });

            // 검색 버튼
            Button btnSearch = CreateBlueButton("검색", new Point(25, 170), new Size(895, 35));
            btnSearch.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            searchCard.Controls.Add(btnSearch);
            pnlContent.Controls.Add(searchCard);

            // 검색 결과 카드
            Panel resultCard = CreateCard(0, 305, 970, 290);
            Label lblResult = new Label { Text = "검색 조건을 입력하고 검색 버튼을 눌러주세요", Font = new Font("맑은 고딕", 11F), ForeColor = Color.Gray, Location = new Point(0, 120), Size = new Size(970, 30), TextAlign = ContentAlignment.MiddleCenter };
            resultCard.Controls.Add(lblResult);
            pnlContent.Controls.Add(resultCard);
        }

        private void ShowLoginLog()
        {
            pnlContent.Controls.Add(CreateTitle("로그인/로그아웃 기록"));

            Panel searchCard = CreateCard(0, 70, 970, 110);

            // 사용자
            searchCard.Controls.Add(new Label { Text = "사용자", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(25, 20), AutoSize = true });
            ComboBox cboUser = new ComboBox { Location = new Point(25, 45), Size = new Size(280, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboUser.Items.AddRange(new object[] { "전체", "사용자 1", "사용자 2" });
            cboUser.SelectedIndex = 0;
            searchCard.Controls.Add(cboUser);

            // 시작일/종료일
            searchCard.Controls.Add(new Label { Text = "시작일", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(330, 20), AutoSize = true });
            searchCard.Controls.Add(new DateTimePicker { Location = new Point(330, 45), Size = new Size(280, 25) });
            searchCard.Controls.Add(new Label { Text = "종료일", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(635, 20), AutoSize = true });
            searchCard.Controls.Add(new DateTimePicker { Location = new Point(635, 45), Size = new Size(280, 25) });

            // 검색 버튼
            Button btnSearch = CreateBlueButton("검색", new Point(800, 45), new Size(120, 25));
            btnSearch.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            searchCard.Controls.Add(btnSearch);
            pnlContent.Controls.Add(searchCard);

            // DataGridView
            DataGridView dgv = CreateDGV(new Point(0, 195), new Size(970, 400));
            dgv.Columns.Add("User", "사용자");
            dgv.Columns.Add("Action", "활동");
            dgv.Columns.Add("Time", "시간");
            pnlContent.Controls.Add(dgv);
        }

        private void ShowPermissionManage()
        {
            pnlContent.Controls.Add(CreateTitle("권한 관리"));

            // 직원 보안 권한 (7-G)
            Panel card1 = CreateCard(0, 70, 475, 400);
            card1.Controls.Add(new Label { Text = "직원 보안 권한 설정", Font = new Font("맑은 고딕", 11F, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true });

            // 사용자 선택
            card1.Controls.Add(new Label { Text = "사용자 선택", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(20, 60), AutoSize = true });
            ComboBox cboUser1 = new ComboBox { Location = new Point(20, 85), Size = new Size(425, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboUser1.Items.AddRange(new object[] { "사용자 1", "사용자 2" });
            card1.Controls.Add(cboUser1);

            // 보이는 부서 범위
            card1.Controls.Add(new Label { Text = "보이는 부서 범위", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(20, 135), AutoSize = true });
            card1.Controls.Add(new CheckBox { Text = "개발팀", Location = new Point(20, 165), Checked = true, AutoSize = true });
            card1.Controls.Add(new CheckBox { Text = "디자인팀", Location = new Point(20, 195), Checked = true, AutoSize = true });
            card1.Controls.Add(new CheckBox { Text = "기획팀", Location = new Point(20, 225), AutoSize = true });

            // 저장 버튼
            Button btnSave1 = CreateBlueButton("저장", new Point(20, 330), new Size(425, 40));
            btnSave1.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            card1.Controls.Add(btnSave1);
            pnlContent.Controls.Add(card1);

            // 대화 허용/차단 (7-H)
            Panel card2 = CreateCard(495, 70, 475, 400);
            card2.Controls.Add(new Label { Text = "대화 허용/차단 관리", Font = new Font("맑은 고딕", 11F, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true });

            // 사용자 A
            card2.Controls.Add(new Label { Text = "사용자 A", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(20, 60), AutoSize = true });
            ComboBox cboUserA = new ComboBox { Location = new Point(20, 85), Size = new Size(425, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboUserA.Items.AddRange(new object[] { "사용자 A-1", "사용자 A-2" });
            card2.Controls.Add(cboUserA);

            // 사용자 B
            card2.Controls.Add(new Label { Text = "사용자 B", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(20, 135), AutoSize = true });
            ComboBox cboUserB = new ComboBox { Location = new Point(20, 160), Size = new Size(425, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboUserB.Items.AddRange(new object[] { "사용자 B-1", "사용자 B-2" });
            card2.Controls.Add(cboUserB);

            // 대화 설정
            card2.Controls.Add(new Label { Text = "대화 설정", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(20, 210), AutoSize = true });
            ComboBox cboSetting = new ComboBox { Location = new Point(20, 235), Size = new Size(425, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboSetting.Items.AddRange(new object[] { "허용", "차단" });
            cboSetting.SelectedIndex = 0;
            card2.Controls.Add(cboSetting);

            // 적용 버튼
            Button btnSave2 = CreateBlueButton("적용", new Point(20, 330), new Size(425, 40));
            btnSave2.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            card2.Controls.Add(btnSave2);
            pnlContent.Controls.Add(card2);
        }

        // --- 공통 UI 생성 도우미 메서드 (Designer.cs에 넣기 어려움) ---
        // 동적으로 생성하는 컨트롤의 스타일을 정의합니다.

        private Label CreateTitle(string text)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(45, 55, 72);
            lbl.Location = new Point(0, 0);
            lbl.AutoSize = true;
            return lbl;
        }

        private Panel CreateCard(int x, int y, int width, int height)
        {
            Panel card = new Panel();
            card.Location = new Point(x, y);
            card.Size = new Size(width, height);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Padding = new Padding(15);
            return card;
        }

        private Button CreateBlueButton(string text, Point location, Size size)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = location;
            btn.Size = size;
            btn.BackColor = Color.FromArgb(49, 130, 206);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private DataGridView CreateDGV(Point location, Size size)
        {
            DataGridView dgv = new DataGridView();
            dgv.Location = location;
            dgv.Size = size;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersHeight = 40;
            dgv.RowTemplate.Height = 40;
            // 데이터를 추가하는 부분은 없습니다.
            return dgv;
        }

    }
}