namespace DBPAdmin
{
    partial class AdminMainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnDepartment;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnChatSearch;
        private System.Windows.Forms.Button btnLoginLog;
        private System.Windows.Forms.Button btnPermission;
        private System.Windows.Forms.Label lblLogo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnDepartment = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.btnChatSearch = new System.Windows.Forms.Button();
            this.btnLoginLog = new System.Windows.Forms.Button();
            this.btnPermission = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlLeft.SuspendLayout();
            this.SuspendLayout();

            // ========== pnlLeft (사이드바) ==========
            this.pnlLeft.BackColor = System.Drawing.ColorTranslator.FromHtml("#778873");
            this.pnlLeft.Controls.Add(this.lblLogo);
            this.pnlLeft.Controls.Add(this.btnDashboard);
            this.pnlLeft.Controls.Add(this.btnDepartment);
            this.pnlLeft.Controls.Add(this.btnUsers);
            this.pnlLeft.Controls.Add(this.btnChatSearch);
            this.pnlLeft.Controls.Add(this.btnLoginLog);
            this.pnlLeft.Controls.Add(this.btnPermission);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(220, 661);
            this.pnlLeft.TabIndex = 0;

            // ========== lblLogo ==========
            this.lblLogo.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(0, 20);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(220, 50);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "관리자 패널";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ========== btnDashboard ==========
            this.btnDashboard.BackColor = System.Drawing.ColorTranslator.FromHtml("#A1BC98");
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(10, 90);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnDashboard.Size = new System.Drawing.Size(200, 42);
            this.btnDashboard.TabIndex = 1;
            this.btnDashboard.Text = "대시보드";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;

            // ========== btnDepartment ==========
            this.btnDepartment.BackColor = System.Drawing.ColorTranslator.FromHtml("#778873");
            this.btnDepartment.FlatAppearance.BorderSize = 0;
            this.btnDepartment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDepartment.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnDepartment.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D2DCB6");
            this.btnDepartment.Location = new System.Drawing.Point(10, 140);
            this.btnDepartment.Name = "btnDepartment";
            this.btnDepartment.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnDepartment.Size = new System.Drawing.Size(200, 42);
            this.btnDepartment.TabIndex = 2;
            this.btnDepartment.Text = "부서관리";
            this.btnDepartment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDepartment.UseVisualStyleBackColor = false;
            this.btnDepartment.Cursor = System.Windows.Forms.Cursors.Hand;

            // ========== btnUsers ==========
            this.btnUsers.BackColor = System.Drawing.ColorTranslator.FromHtml("#778873");
            this.btnUsers.FlatAppearance.BorderSize = 0;
            this.btnUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsers.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnUsers.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D2DCB6");
            this.btnUsers.Location = new System.Drawing.Point(10, 190);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnUsers.Size = new System.Drawing.Size(200, 42);
            this.btnUsers.TabIndex = 3;
            this.btnUsers.Text = "사용자관리";
            this.btnUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsers.UseVisualStyleBackColor = false;
            this.btnUsers.Cursor = System.Windows.Forms.Cursors.Hand;

            // ========== btnChatSearch ==========
            this.btnChatSearch.BackColor = System.Drawing.ColorTranslator.FromHtml("#778873");
            this.btnChatSearch.FlatAppearance.BorderSize = 0;
            this.btnChatSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChatSearch.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnChatSearch.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D2DCB6");
            this.btnChatSearch.Location = new System.Drawing.Point(10, 240);
            this.btnChatSearch.Name = "btnChatSearch";
            this.btnChatSearch.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnChatSearch.Size = new System.Drawing.Size(200, 42);
            this.btnChatSearch.TabIndex = 4;
            this.btnChatSearch.Text = "대화내용검색";
            this.btnChatSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChatSearch.UseVisualStyleBackColor = false;
            this.btnChatSearch.Cursor = System.Windows.Forms.Cursors.Hand;

            // ========== btnLoginLog ==========
            this.btnLoginLog.BackColor = System.Drawing.ColorTranslator.FromHtml("#778873");
            this.btnLoginLog.FlatAppearance.BorderSize = 0;
            this.btnLoginLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoginLog.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnLoginLog.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D2DCB6");
            this.btnLoginLog.Location = new System.Drawing.Point(10, 290);
            this.btnLoginLog.Name = "btnLoginLog";
            this.btnLoginLog.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnLoginLog.Size = new System.Drawing.Size(200, 42);
            this.btnLoginLog.TabIndex = 5;
            this.btnLoginLog.Text = "로그인기록";
            this.btnLoginLog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoginLog.UseVisualStyleBackColor = false;
            this.btnLoginLog.Cursor = System.Windows.Forms.Cursors.Hand;

            // ========== btnPermission ==========
            this.btnPermission.BackColor = System.Drawing.ColorTranslator.FromHtml("#778873");
            this.btnPermission.FlatAppearance.BorderSize = 0;
            this.btnPermission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPermission.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnPermission.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D2DCB6");
            this.btnPermission.Location = new System.Drawing.Point(10, 340);
            this.btnPermission.Name = "btnPermission";
            this.btnPermission.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnPermission.Size = new System.Drawing.Size(200, 42);
            this.btnPermission.TabIndex = 6;
            this.btnPermission.Text = "권한관리";
            this.btnPermission.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPermission.UseVisualStyleBackColor = false;
            this.btnPermission.Cursor = System.Windows.Forms.Cursors.Hand;

            // ========== pnlContent (메인 영역) ==========
            this.pnlContent.BackColor = System.Drawing.ColorTranslator.FromHtml("#F1F3E0");
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(220, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(20);
            this.pnlContent.Size = new System.Drawing.Size(980, 661);
            this.pnlContent.TabIndex = 1;
            this.pnlContent.AutoScroll = true;

            // ========== AdminMainForm ==========
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 661);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlLeft);
            this.Name = "AdminMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "관리자 패널";
            this.pnlLeft.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}