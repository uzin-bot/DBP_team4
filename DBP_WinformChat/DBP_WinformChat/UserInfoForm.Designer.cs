namespace leehaeun
{
    partial class UserInfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            label1 = new Label();
            StatusBox = new TextBox();
            NicknameBox = new TextBox();
            ProfileImageBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 74);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(67, 15);
            label1.TabIndex = 27;
            label1.Text = "상태메시지";
            // 
            // StatusBox
            // 
            StatusBox.Location = new Point(11, 91);
            StatusBox.Margin = new Padding(2);
            StatusBox.Multiline = true;
            StatusBox.Name = "StatusBox";
            StatusBox.Size = new Size(219, 51);
            StatusBox.TabIndex = 29;
            // 
            // NicknameBox
            // 
            NicknameBox.Location = new Point(65, 11);
            NicknameBox.Margin = new Padding(2);
            NicknameBox.Name = "NicknameBox";
            NicknameBox.Size = new Size(165, 23);
            NicknameBox.TabIndex = 28;
            // 
            // ProfileImageBox
            // 
            ProfileImageBox.Image = DBP_WinformChat.Properties.Resources._default;
            ProfileImageBox.Location = new Point(11, 11);
            ProfileImageBox.Margin = new Padding(2);
            ProfileImageBox.Name = "ProfileImageBox";
            ProfileImageBox.Size = new Size(50, 48);
            ProfileImageBox.SizeMode = PictureBoxSizeMode.Zoom;
            ProfileImageBox.TabIndex = 24;
            ProfileImageBox.TabStop = false;
            ProfileImageBox.Tag = "";
            // 
            // UserInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(245, 151);
            Controls.Add(label1);
            Controls.Add(StatusBox);
            Controls.Add(NicknameBox);
            Controls.Add(ProfileImageBox);
            Name = "UserInfoForm";
            Text = "UserInfoForm";
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox StatusBox;
        private TextBox NicknameBox;
        private PictureBox ProfileImageBox;
    }
}