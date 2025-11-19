using System.Windows.Forms;

namespace leehaeun
{
    partial class SignUp
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
            label3 = new Label();
            label4 = new Label();
            label6 = new Label();
            label7 = new Label();
            IdBox = new TextBox();
            TeamBox = new ComboBox();
            PwBox = new TextBox();
            NameBox = new TextBox();
            AddressBox = new TextBox();
            ZipCodeBox = new TextBox();
            CheckIDButton = new Button();
            SearchAddressButton = new Button();
            ProfileImageBox = new PictureBox();
            ChangeProfileButton = new Button();
            NickNameBox = new TextBox();
            SignUpButton = new Button();
            IsDuplicate = new Label();
            openFileDialog = new OpenFileDialog();
            FilePath = new Label();
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(365, 75);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 0;
            label1.Text = "아이디";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(365, 126);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 1;
            label2.Text = "비밀번호";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(108, 268);
            label3.Name = "label3";
            label3.Size = new Size(31, 15);
            label3.TabIndex = 3;
            label3.Text = "별명";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(365, 179);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 2;
            label4.Text = "이름";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(365, 320);
            label6.Name = "label6";
            label6.Size = new Size(59, 15);
            label6.TabIndex = 5;
            label6.Text = "소속 부서";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(365, 234);
            label7.Name = "label7";
            label7.Size = new Size(31, 15);
            label7.TabIndex = 4;
            label7.Text = "주소";
            // 
            // IdBox
            // 
            IdBox.Location = new Point(469, 72);
            IdBox.Name = "IdBox";
            IdBox.Size = new Size(121, 23);
            IdBox.TabIndex = 6;
            // 
            // TeamBox
            // 
            TeamBox.FormattingEnabled = true;
            TeamBox.Items.AddRange(new object[] { "인사팀", "마케팅팀", "개발팀" });
            TeamBox.Location = new Point(469, 317);
            TeamBox.Name = "TeamBox";
            TeamBox.Size = new Size(121, 23);
            TeamBox.TabIndex = 7;
            // 
            // PwBox
            // 
            PwBox.Location = new Point(469, 123);
            PwBox.Name = "PwBox";
            PwBox.Size = new Size(121, 23);
            PwBox.TabIndex = 8;
            // 
            // NameBox
            // 
            NameBox.Location = new Point(469, 176);
            NameBox.Name = "NameBox";
            NameBox.Size = new Size(121, 23);
            NameBox.TabIndex = 9;
            // 
            // AddressBox
            // 
            AddressBox.Location = new Point(469, 231);
            AddressBox.Name = "AddressBox";
            AddressBox.Size = new Size(121, 23);
            AddressBox.TabIndex = 10;
            // 
            // ZipCodeBox
            // 
            ZipCodeBox.Location = new Point(469, 260);
            ZipCodeBox.Name = "ZipCodeBox";
            ZipCodeBox.ReadOnly = true;
            ZipCodeBox.Size = new Size(121, 23);
            ZipCodeBox.TabIndex = 11;
            ZipCodeBox.Text = "12345";
            // 
            // CheckIDButton
            // 
            CheckIDButton.Location = new Point(611, 72);
            CheckIDButton.Name = "CheckIDButton";
            CheckIDButton.Size = new Size(75, 23);
            CheckIDButton.TabIndex = 12;
            CheckIDButton.Text = "중복 확인";
            CheckIDButton.UseVisualStyleBackColor = true;
            CheckIDButton.Click += CheckIDButton_Click;
            // 
            // SearchAddressButton
            // 
            SearchAddressButton.Location = new Point(611, 231);
            SearchAddressButton.Name = "SearchAddressButton";
            SearchAddressButton.Size = new Size(75, 23);
            SearchAddressButton.TabIndex = 13;
            SearchAddressButton.Text = "주소 검색";
            SearchAddressButton.UseVisualStyleBackColor = true;
            // 
            // ProfileImageBox
            // 
            ProfileImageBox.Image = DBP_WinformChat.Properties.Resources.default_jpg;
            ProfileImageBox.Location = new Point(108, 72);
            ProfileImageBox.Name = "ProfileImageBox";
            ProfileImageBox.Size = new Size(155, 143);
            ProfileImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            ProfileImageBox.TabIndex = 14;
            ProfileImageBox.TabStop = false;
            // 
            // ChangeProfileButton
            // 
            ChangeProfileButton.Location = new Point(108, 221);
            ChangeProfileButton.Name = "ChangeProfileButton";
            ChangeProfileButton.Size = new Size(155, 23);
            ChangeProfileButton.TabIndex = 15;
            ChangeProfileButton.Text = "프로필 변경";
            ChangeProfileButton.UseVisualStyleBackColor = true;
            ChangeProfileButton.Click += ChangeProfileButton_Click;
            // 
            // NickNameBox
            // 
            NickNameBox.Location = new Point(108, 286);
            NickNameBox.Name = "NickNameBox";
            NickNameBox.Size = new Size(155, 23);
            NickNameBox.TabIndex = 16;
            // 
            // SignUpButton
            // 
            SignUpButton.Location = new Point(611, 376);
            SignUpButton.Name = "SignUpButton";
            SignUpButton.Size = new Size(75, 23);
            SignUpButton.TabIndex = 17;
            SignUpButton.Text = "회원가입";
            SignUpButton.UseVisualStyleBackColor = true;
            SignUpButton.Click += SignUpButton_Click;
            // 
            // IsDuplicate
            // 
            IsDuplicate.AutoSize = true;
            IsDuplicate.Location = new Point(469, 98);
            IsDuplicate.Name = "IsDuplicate";
            IsDuplicate.Size = new Size(0, 15);
            IsDuplicate.TabIndex = 18;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // FilePath
            // 
            FilePath.AutoSize = true;
            FilePath.ForeColor = SystemColors.ButtonFace;
            FilePath.Location = new Point(108, 54);
            FilePath.Name = "FilePath";
            FilePath.Size = new Size(0, 15);
            FilePath.TabIndex = 19;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(FilePath);
            Controls.Add(IsDuplicate);
            Controls.Add(SignUpButton);
            Controls.Add(NickNameBox);
            Controls.Add(ChangeProfileButton);
            Controls.Add(ProfileImageBox);
            Controls.Add(SearchAddressButton);
            Controls.Add(CheckIDButton);
            Controls.Add(ZipCodeBox);
            Controls.Add(AddressBox);
            Controls.Add(NameBox);
            Controls.Add(PwBox);
            Controls.Add(TeamBox);
            Controls.Add(IdBox);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "SignUp";
            Text = "SignUp";
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label6;
        private Label label7;
        private TextBox IdBox;
        private ComboBox TeamBox;
        private TextBox PwBox;
        private TextBox NameBox;
        private TextBox AddressBox;
        private TextBox ZipCodeBox;
        private Button CheckIDButton;
        private Button SearchAddressButton;
        private PictureBox ProfileImageBox;
        private Button ChangeProfileButton;
        private TextBox NickNameBox;
        private Button SignUpButton;
        private Label IsDuplicate;
        private OpenFileDialog openFileDialog;
        private Label FilePath;
    }
}