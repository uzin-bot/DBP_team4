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
            pnlLeft = new Panel();
            lblLogo = new Label();
            btnDashboard = new Button();
            btnDepartment = new Button();
            btnUsers = new Button();
            btnChatSearch = new Button();
            btnLoginLog = new Button();
            btnPermission = new Button();
            pnlContent = new Panel();
            btnLogout = new Button();
            pnlLeft.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.BackColor = Color.FromArgb(119, 136, 115);
            pnlLeft.Controls.Add(btnLogout);
            pnlLeft.Controls.Add(lblLogo);
            pnlLeft.Controls.Add(btnDashboard);
            pnlLeft.Controls.Add(btnDepartment);
            pnlLeft.Controls.Add(btnUsers);
            pnlLeft.Controls.Add(btnChatSearch);
            pnlLeft.Controls.Add(btnLoginLog);
            pnlLeft.Controls.Add(btnPermission);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Margin = new Padding(4, 5, 4, 5);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(283, 759);
            pnlLeft.TabIndex = 0;
            // 
            // lblLogo
            // 
            lblLogo.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(0, 33);
            lblLogo.Margin = new Padding(4, 0, 4, 0);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(283, 83);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "관리자 패널";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.FromArgb(161, 188, 152);
            btnDashboard.Cursor = Cursors.Hand;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Location = new Point(13, 150);
            btnDashboard.Margin = new Padding(4, 5, 4, 5);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Padding = new Padding(19, 0, 0, 0);
            btnDashboard.Size = new Size(257, 70);
            btnDashboard.TabIndex = 1;
            btnDashboard.Text = "대시보드";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.UseVisualStyleBackColor = false;
            // 
            // btnDepartment
            // 
            btnDepartment.BackColor = Color.FromArgb(119, 136, 115);
            btnDepartment.Cursor = Cursors.Hand;
            btnDepartment.FlatAppearance.BorderSize = 0;
            btnDepartment.FlatStyle = FlatStyle.Flat;
            btnDepartment.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnDepartment.ForeColor = Color.FromArgb(210, 220, 182);
            btnDepartment.Location = new Point(13, 233);
            btnDepartment.Margin = new Padding(4, 5, 4, 5);
            btnDepartment.Name = "btnDepartment";
            btnDepartment.Padding = new Padding(19, 0, 0, 0);
            btnDepartment.Size = new Size(257, 70);
            btnDepartment.TabIndex = 2;
            btnDepartment.Text = "부서관리";
            btnDepartment.TextAlign = ContentAlignment.MiddleLeft;
            btnDepartment.UseVisualStyleBackColor = false;
            // 
            // btnUsers
            // 
            btnUsers.BackColor = Color.FromArgb(119, 136, 115);
            btnUsers.Cursor = Cursors.Hand;
            btnUsers.FlatAppearance.BorderSize = 0;
            btnUsers.FlatStyle = FlatStyle.Flat;
            btnUsers.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnUsers.ForeColor = Color.FromArgb(210, 220, 182);
            btnUsers.Location = new Point(13, 317);
            btnUsers.Margin = new Padding(4, 5, 4, 5);
            btnUsers.Name = "btnUsers";
            btnUsers.Padding = new Padding(19, 0, 0, 0);
            btnUsers.Size = new Size(257, 70);
            btnUsers.TabIndex = 3;
            btnUsers.Text = "사용자관리";
            btnUsers.TextAlign = ContentAlignment.MiddleLeft;
            btnUsers.UseVisualStyleBackColor = false;
            // 
            // btnChatSearch
            // 
            btnChatSearch.BackColor = Color.FromArgb(119, 136, 115);
            btnChatSearch.Cursor = Cursors.Hand;
            btnChatSearch.FlatAppearance.BorderSize = 0;
            btnChatSearch.FlatStyle = FlatStyle.Flat;
            btnChatSearch.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnChatSearch.ForeColor = Color.FromArgb(210, 220, 182);
            btnChatSearch.Location = new Point(13, 400);
            btnChatSearch.Margin = new Padding(4, 5, 4, 5);
            btnChatSearch.Name = "btnChatSearch";
            btnChatSearch.Padding = new Padding(19, 0, 0, 0);
            btnChatSearch.Size = new Size(257, 70);
            btnChatSearch.TabIndex = 4;
            btnChatSearch.Text = "대화내용검색";
            btnChatSearch.TextAlign = ContentAlignment.MiddleLeft;
            btnChatSearch.UseVisualStyleBackColor = false;
            // 
            // btnLoginLog
            // 
            btnLoginLog.BackColor = Color.FromArgb(119, 136, 115);
            btnLoginLog.Cursor = Cursors.Hand;
            btnLoginLog.FlatAppearance.BorderSize = 0;
            btnLoginLog.FlatStyle = FlatStyle.Flat;
            btnLoginLog.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnLoginLog.ForeColor = Color.FromArgb(210, 220, 182);
            btnLoginLog.Location = new Point(13, 483);
            btnLoginLog.Margin = new Padding(4, 5, 4, 5);
            btnLoginLog.Name = "btnLoginLog";
            btnLoginLog.Padding = new Padding(19, 0, 0, 0);
            btnLoginLog.Size = new Size(257, 70);
            btnLoginLog.TabIndex = 5;
            btnLoginLog.Text = "로그인기록";
            btnLoginLog.TextAlign = ContentAlignment.MiddleLeft;
            btnLoginLog.UseVisualStyleBackColor = false;
            // 
            // btnPermission
            // 
            btnPermission.BackColor = Color.FromArgb(119, 136, 115);
            btnPermission.Cursor = Cursors.Hand;
            btnPermission.FlatAppearance.BorderSize = 0;
            btnPermission.FlatStyle = FlatStyle.Flat;
            btnPermission.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnPermission.ForeColor = Color.FromArgb(210, 220, 182);
            btnPermission.Location = new Point(13, 567);
            btnPermission.Margin = new Padding(4, 5, 4, 5);
            btnPermission.Name = "btnPermission";
            btnPermission.Padding = new Padding(19, 0, 0, 0);
            btnPermission.Size = new Size(257, 70);
            btnPermission.TabIndex = 6;
            btnPermission.Text = "권한관리";
            btnPermission.TextAlign = ContentAlignment.MiddleLeft;
            btnPermission.UseVisualStyleBackColor = false;
            // 
            // pnlContent
            // 
            pnlContent.AutoScroll = true;
            pnlContent.BackColor = Color.FromArgb(241, 243, 224);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(283, 0);
            pnlContent.Margin = new Padding(4, 5, 4, 5);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(26, 33, 26, 33);
            pnlContent.Size = new Size(1173, 759);
            pnlContent.TabIndex = 1;
            // 
            // btnLogout
            // 
            btnLogout.Location = new Point(26, 699);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(94, 29);
            btnLogout.TabIndex = 7;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // AdminMainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1456, 759);
            Controls.Add(pnlContent);
            Controls.Add(pnlLeft);
            Margin = new Padding(4, 5, 4, 5);
            Name = "AdminMainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "관리자 패널";
            pnlLeft.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Button btnLogout;
    }
}