using System.Drawing;
using System.Windows.Forms;
using System;

namespace MessengerApp
{
    partial class AdminMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // --- 컨트롤 선언 필드 (전역 변수) ---
        private Panel pnlLeft;
        private Panel pnlLeftHeader;
        private Panel pnlLeftFooter;
        private Button btnDashboard;
        private Button btnDepartment;
        private Button btnUsers;
        private Button btnChatSearch;
        private Button btnLoginLog;
        private Button btnPermission;
        private Panel pnlRight;
        private Panel pnlContent;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblAdmin;
        private Label lblEmail;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 컨트롤 인스턴스 생성
            pnlLeft = new Panel();
            pnlLeftHeader = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            pnlLeftFooter = new Panel();
            lblAdmin = new Label();
            lblEmail = new Label();

            // 메뉴 버튼들
            btnDashboard = new Button();
            btnDepartment = new Button();
            btnUsers = new Button();
            btnChatSearch = new Button();
            btnLoginLog = new Button();
            btnPermission = new Button();

            pnlRight = new Panel();
            pnlContent = new Panel();

            // Layout 시작
            pnlLeft.SuspendLayout();
            pnlLeftHeader.SuspendLayout();
            pnlLeftFooter.SuspendLayout();
            pnlRight.SuspendLayout();
            this.SuspendLayout();

            // 
            // AdminMainForm (폼 자체)
            // 
            this.AutoScaleDimensions = new SizeF(8F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1300, 800); // 폼 폭 조정
            this.BackColor = Color.FromArgb(247, 250, 252); // 배경색 밝게 변경
            this.Font = new Font("맑은 고딕", 9F);
            this.Controls.Add(pnlRight);
            this.Controls.Add(pnlLeft);
            this.Name = "AdminMainForm";
            this.Text = "관리자 패널";

            // 
            // pnlLeft (왼쪽 사이드바)
            // 
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Width = 260; // 폭 조정
            pnlLeft.BackColor = Color.FromArgb(27, 37, 56); // 배경색 진하게 변경
            pnlLeft.Controls.Add(btnPermission);
            pnlLeft.Controls.Add(btnLoginLog);
            pnlLeft.Controls.Add(btnChatSearch);
            pnlLeft.Controls.Add(btnUsers);
            pnlLeft.Controls.Add(btnDepartment);
            pnlLeft.Controls.Add(btnDashboard);
            pnlLeft.Controls.Add(pnlLeftFooter);
            pnlLeft.Controls.Add(pnlLeftHeader);
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.TabIndex = 0;

            // 
            // pnlLeftHeader (왼쪽 상단 제목)
            // 
            pnlLeftHeader.Dock = DockStyle.Top;
            pnlLeftHeader.Height = 100;
            pnlLeftHeader.BackColor = Color.FromArgb(27, 37, 56); // 배경색 일치
            pnlLeftHeader.Controls.Add(lblTitle);
            pnlLeftHeader.Controls.Add(lblSubtitle);
            pnlLeftHeader.Location = new Point(0, 0);
            pnlLeftHeader.Name = "pnlLeftHeader";
            pnlLeftHeader.TabIndex = 0;

            // 
            // lblTitle (관리자 메뉴)
            // 
            lblTitle.Text = "관리자 메뉴";
            lblTitle.Font = new Font("맑은 고딕", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 30);
            lblTitle.AutoSize = true;
            lblTitle.Name = "lblTitle";

            // 
            // lblSubtitle (Admin Panel)
            // 
            lblSubtitle.Text = "Admin Panel";
            lblSubtitle.Font = new Font("맑은 고딕", 9F);
            lblSubtitle.ForeColor = Color.FromArgb(160, 174, 192);
            lblSubtitle.Location = new Point(20, 62);
            lblSubtitle.AutoSize = true;
            lblSubtitle.Name = "lblSubtitle";

            // --- 메뉴 버튼 공통 속성 설정 ---
            Action<Button, string, int, string> setupButton = (btn, text, y, name) =>
            {
                btn.Text = text;
                btn.Location = new Point(10, y);
                btn.Size = new Size(240, 45);
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font("맑은 고딕", 10F);
                btn.ForeColor = Color.FromArgb(203, 213, 224);
                btn.BackColor = Color.FromArgb(27, 37, 56); // 배경색 일치
                btn.TextAlign = ContentAlignment.MiddleLeft;
                btn.Padding = new Padding(15, 0, 0, 0);
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 55, 72); // 마우스 오버 색상 조정
                btn.Cursor = Cursors.Hand;
                btn.UseVisualStyleBackColor = false;
                btn.Name = name; // Name 속성을 명확히 지정
                btn.TabIndex = y;
            };

            // 
            // 메뉴 버튼들
            // 
            setupButton(btnDashboard, "📊 대시보드", 120, "btnDashboard");
            setupButton(btnDepartment, "🏢 부서 관리", 175, "btnDepartment");
            setupButton(btnUsers, "👥 사용자 관리", 230, "btnUsers");
            setupButton(btnChatSearch, "💬 대화 내용 검색", 285, "btnChatSearch");
            setupButton(btnLoginLog, "🕐 로그인 기록", 340, "btnLoginLog");
            setupButton(btnPermission, "🔒 권한 관리", 395, "btnPermission");

            // 
            // pnlLeftFooter (왼쪽 하단 정보)
            // 
            pnlLeftFooter.Dock = DockStyle.Bottom;
            pnlLeftFooter.Height = 90;
            pnlLeftFooter.BackColor = Color.FromArgb(36, 44, 58);
            pnlLeftFooter.Controls.Add(lblAdmin);
            pnlLeftFooter.Controls.Add(lblEmail);
            pnlLeftFooter.Location = new Point(0, 710);
            pnlLeftFooter.Name = "pnlLeftFooter";
            pnlLeftFooter.TabIndex = 1;

            // 
            // lblAdmin (관리자: 홍길동)
            // 
            lblAdmin.Text = "관리자: 홍길동";
            lblAdmin.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblAdmin.ForeColor = Color.White;
            lblAdmin.Location = new Point(20, 25);
            lblAdmin.AutoSize = true;
            lblAdmin.Name = "lblAdmin";

            // 
            // lblEmail (admin@company.com)
            // 
            lblEmail.Text = "admin@company.com";
            lblEmail.Font = new Font("맑은 고딕", 8F);
            lblEmail.ForeColor = Color.FromArgb(160, 174, 192);
            lblEmail.Location = new Point(20, 50);
            lblEmail.AutoSize = true;
            lblEmail.Name = "lblEmail";

            // 
            // pnlRight (오른쪽 컨테이너)
            // 
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.BackColor = Color.FromArgb(247, 250, 252); // 배경색 일치
            pnlRight.Padding = new Padding(30); // 여백 조정
            pnlRight.Controls.Add(pnlContent);
            pnlRight.Location = new Point(260, 0); // 시작 위치 조정
            pnlRight.Name = "pnlRight";
            pnlRight.TabIndex = 1;

            // 
            // pnlContent (실제 내용이 들어갈 패널)
            // 
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.BackColor = Color.FromArgb(247, 250, 252); // 배경색 일치
            pnlContent.AutoScroll = true;
            pnlContent.Location = new Point(30, 30); // pnlRight Padding에 의해 위치 결정
            pnlContent.Name = "pnlContent";
            pnlContent.TabIndex = 0;

            // Layout 종료
            pnlLeft.ResumeLayout(false);
            pnlLeftHeader.ResumeLayout(false);
            pnlLeftHeader.PerformLayout();
            pnlLeftFooter.ResumeLayout(false);
            pnlLeftFooter.PerformLayout();
            pnlRight.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}