using System.Windows.Forms;

namespace leehaeun
{
    partial class LoginForm
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
            PwBox = new TextBox();
            LoginButton = new Button();
            RememberMeCheckBox = new CheckBox();
            SaveInfoCheckBox = new CheckBox();
            IdBox = new TextBox();
            SignUpLInkLabel = new LinkLabel();
            SuspendLayout();
            // 
            // PwBox
            // 
            PwBox.Location = new Point(54, 208);
            PwBox.Name = "PwBox";
            PwBox.PasswordChar = '*';
            PwBox.Size = new Size(179, 23);
            PwBox.TabIndex = 3;
            // 
            // LoginButton
            // 
            LoginButton.Location = new Point(54, 309);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(179, 23);
            LoginButton.TabIndex = 4;
            LoginButton.Text = "로그인";
            LoginButton.UseVisualStyleBackColor = true;
            LoginButton.Click += LoginButton_Click;
            // 
            // RememberMeCheckBox
            // 
            RememberMeCheckBox.AutoSize = true;
            RememberMeCheckBox.Location = new Point(54, 237);
            RememberMeCheckBox.Name = "RememberMeCheckBox";
            RememberMeCheckBox.Size = new Size(90, 19);
            RememberMeCheckBox.TabIndex = 5;
            RememberMeCheckBox.Text = "자동 로그인";
            RememberMeCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveInfoCheckBox
            // 
            SaveInfoCheckBox.AutoSize = true;
            SaveInfoCheckBox.Location = new Point(54, 262);
            SaveInfoCheckBox.Name = "SaveInfoCheckBox";
            SaveInfoCheckBox.Size = new Size(118, 19);
            SaveInfoCheckBox.TabIndex = 6;
            SaveInfoCheckBox.Text = "로그인 정보 저장";
            SaveInfoCheckBox.UseVisualStyleBackColor = true;
            // 
            // IdBox
            // 
            IdBox.Location = new Point(54, 137);
            IdBox.Name = "IdBox";
            IdBox.Size = new Size(179, 23);
            IdBox.TabIndex = 7;
            // 
            // SignUpLInkLabel
            // 
            SignUpLInkLabel.AutoSize = true;
            SignUpLInkLabel.LinkColor = Color.Gray;
            SignUpLInkLabel.Location = new Point(117, 338);
            SignUpLInkLabel.Name = "SignUpLInkLabel";
            SignUpLInkLabel.Size = new Size(55, 15);
            SignUpLInkLabel.TabIndex = 8;
            SignUpLInkLabel.TabStop = true;
            SignUpLInkLabel.Text = "회원가입";
            SignUpLInkLabel.LinkClicked += SignUpLabel_LinkClicked;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 450);
            Controls.Add(SignUpLInkLabel);
            Controls.Add(IdBox);
            Controls.Add(SaveInfoCheckBox);
            Controls.Add(RememberMeCheckBox);
            Controls.Add(LoginButton);
            Controls.Add(PwBox);
            Name = "LoginForm";
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox PwBox;
        private Button LoginButton;
        private CheckBox RememberMeCheckBox;
        private CheckBox SaveInfoCheckBox;
        private TextBox IdBox;
        private LinkLabel SignUpLInkLabel;
    }
}