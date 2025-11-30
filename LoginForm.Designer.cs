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
            label1 = new Label();
            label2 = new Label();
            PwBox = new TextBox();
            LoginButton = new Button();
            RememberMe = new CheckBox();
            SaveInfo = new CheckBox();
            IdBox = new TextBox();
            SignUpLabel = new LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(54, 119);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 0;
            label1.Text = "아이디";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(54, 190);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 1;
            label2.Text = "비밀번호";
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
            // RememberMe
            // 
            RememberMe.AutoSize = true;
            RememberMe.Location = new Point(54, 237);
            RememberMe.Name = "RememberMe";
            RememberMe.Size = new Size(90, 19);
            RememberMe.TabIndex = 5;
            RememberMe.Text = "자동 로그인";
            RememberMe.UseVisualStyleBackColor = true;
            // 
            // SaveInfo
            // 
            SaveInfo.AutoSize = true;
            SaveInfo.Location = new Point(54, 262);
            SaveInfo.Name = "SaveInfo";
            SaveInfo.Size = new Size(118, 19);
            SaveInfo.TabIndex = 6;
            SaveInfo.Text = "로그인 정보 저장";
            SaveInfo.UseVisualStyleBackColor = true;
            // 
            // IdBox
            // 
            IdBox.Location = new Point(54, 137);
            IdBox.Name = "IdBox";
            IdBox.Size = new Size(179, 23);
            IdBox.TabIndex = 7;
            // 
            // SignUpLabel
            // 
            SignUpLabel.AutoSize = true;
            SignUpLabel.LinkColor = Color.Gray;
            SignUpLabel.Location = new Point(117, 338);
            SignUpLabel.Name = "SignUpLabel";
            SignUpLabel.Size = new Size(55, 15);
            SignUpLabel.TabIndex = 8;
            SignUpLabel.TabStop = true;
            SignUpLabel.Text = "회원가입";
            SignUpLabel.LinkClicked += SignUpLabel_LinkClicked;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 450);
            Controls.Add(SignUpLabel);
            Controls.Add(IdBox);
            Controls.Add(SaveInfo);
            Controls.Add(RememberMe);
            Controls.Add(LoginButton);
            Controls.Add(PwBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "LoginForm";
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox PwBox;
        private Button LoginButton;
        private CheckBox RememberMe;
        private CheckBox SaveInfo;
        private TextBox IdBox;
        private LinkLabel SignUpLabel;
    }
}