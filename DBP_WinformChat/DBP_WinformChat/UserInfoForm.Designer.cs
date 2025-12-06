namespace leehaeun
{
    partial class UserInfoForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            ProfileImageBox = new PictureBox();
            NicknameLabel = new Label();
            StatusMessageLabel = new Label();
            DeptLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox).BeginInit();
            SuspendLayout();
            // 
            // ProfileImageBox
            // 
            ProfileImageBox.Image = DBP_WinformChat.Properties.Resources._default;
            ProfileImageBox.Location = new Point(30, 30);
            ProfileImageBox.Name = "ProfileImageBox";
            ProfileImageBox.Size = new Size(80, 80);
            ProfileImageBox.SizeMode = PictureBoxSizeMode.Zoom;
            ProfileImageBox.TabIndex = 0;
            ProfileImageBox.TabStop = false;
            // 
            // NicknameLabel
            // 
            NicknameLabel.AutoSize = true;
            NicknameLabel.Location = new Point(130, 40);
            NicknameLabel.Name = "NicknameLabel";
            NicknameLabel.Size = new Size(43, 15);
            NicknameLabel.TabIndex = 1;
            NicknameLabel.Text = "닉네임";
            // 
            // StatusMessageLabel
            // 
            StatusMessageLabel.AutoSize = true;
            StatusMessageLabel.Location = new Point(30, 130);
            StatusMessageLabel.Name = "StatusMessageLabel";
            StatusMessageLabel.Size = new Size(71, 15);
            StatusMessageLabel.TabIndex = 3;
            StatusMessageLabel.Text = "상태 메시지";
            // 
            // DeptLabel
            // 
            DeptLabel.AutoSize = true;
            DeptLabel.Location = new Point(130, 70);
            DeptLabel.Name = "DeptLabel";
            DeptLabel.Size = new Size(59, 15);
            DeptLabel.TabIndex = 2;
            DeptLabel.Text = "소속 부서";
            // 
            // UserInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(350, 254);
            Controls.Add(StatusMessageLabel);
            Controls.Add(DeptLabel);
            Controls.Add(NicknameLabel);
            Controls.Add(ProfileImageBox);
            Name = "UserInfoForm";
            Text = "UserInfoForm";
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox ProfileImageBox;
        private Label NicknameLabel;
        private Label StatusMessageLabel;
        private Label DeptLabel;
    }
}
