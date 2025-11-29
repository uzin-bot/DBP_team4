namespace leehaeun
{
    partial class EditInfoForm
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
            UserInfoPage = new TabPage();
            label8 = new Label();
            label6 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            IdBox = new TextBox();
            PwBox = new TextBox();
            NameBox = new TextBox();
            ZipCodeBox = new TextBox();
            AddressBox = new TextBox();
            DeptBox = new TextBox();
            SearchAddressButton = new Button();
            SaveIButton = new Button();
            CancleIButton = new Button();
            label10 = new Label();
            CheckPwBox = new TextBox();
            MulProfilePage = new TabPage();
            ProfileFLP = new FlowLayoutPanel();
            panel = new Panel();
            NicknameLabel = new Label();
            EditButton = new Button();
            ProfileImageMBox = new PictureBox();
            panel3 = new Panel();
            label7 = new Label();
            AddMulProfileButton = new Button();
            DefaultProfilePage = new TabPage();
            DeptLabel = new Label();
            ProfileImagePBox = new PictureBox();
            ChangeProfileImageButton = new Button();
            NicknameBox = new TextBox();
            panel2 = new Panel();
            SavePButton = new Button();
            CancelPButton = new Button();
            StatusBox = new TextBox();
            MemberFLP = new FlowLayoutPanel();
            label1 = new Label();
            EditMemberLabel = new Label();
            tabControl = new TabControl();
            UserInfoPage.SuspendLayout();
            MulProfilePage.SuspendLayout();
            ProfileFLP.SuspendLayout();
            panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImageMBox).BeginInit();
            panel3.SuspendLayout();
            DefaultProfilePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImagePBox).BeginInit();
            panel2.SuspendLayout();
            tabControl.SuspendLayout();
            SuspendLayout();
            // 
            // UserInfoPage
            // 
            UserInfoPage.Controls.Add(CheckPwBox);
            UserInfoPage.Controls.Add(DeptBox);
            UserInfoPage.Controls.Add(AddressBox);
            UserInfoPage.Controls.Add(ZipCodeBox);
            UserInfoPage.Controls.Add(NameBox);
            UserInfoPage.Controls.Add(PwBox);
            UserInfoPage.Controls.Add(IdBox);
            UserInfoPage.Controls.Add(label10);
            UserInfoPage.Controls.Add(CancleIButton);
            UserInfoPage.Controls.Add(SaveIButton);
            UserInfoPage.Controls.Add(SearchAddressButton);
            UserInfoPage.Controls.Add(label5);
            UserInfoPage.Controls.Add(label4);
            UserInfoPage.Controls.Add(label3);
            UserInfoPage.Controls.Add(label6);
            UserInfoPage.Controls.Add(label8);
            UserInfoPage.Location = new Point(4, 24);
            UserInfoPage.Name = "UserInfoPage";
            UserInfoPage.Size = new Size(252, 365);
            UserInfoPage.TabIndex = 2;
            UserInfoPage.Text = "계정 정보";
            UserInfoPage.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(21, 20);
            label8.Name = "label8";
            label8.Size = new Size(43, 15);
            label8.TabIndex = 14;
            label8.Text = "아이디";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(21, 66);
            label6.Name = "label6";
            label6.Size = new Size(55, 15);
            label6.TabIndex = 15;
            label6.Text = "비밀번호";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 158);
            label3.Name = "label3";
            label3.Size = new Size(31, 15);
            label3.TabIndex = 16;
            label3.Text = "이름";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 204);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 17;
            label4.Text = "주소";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(21, 279);
            label5.Name = "label5";
            label5.Size = new Size(59, 15);
            label5.TabIndex = 18;
            label5.Text = "소속 부서";
            // 
            // IdBox
            // 
            IdBox.Location = new Point(21, 38);
            IdBox.Name = "IdBox";
            IdBox.ReadOnly = true;
            IdBox.Size = new Size(212, 23);
            IdBox.TabIndex = 19;
            // 
            // PwBox
            // 
            PwBox.Location = new Point(21, 84);
            PwBox.Name = "PwBox";
            PwBox.PasswordChar = '*';
            PwBox.Size = new Size(212, 23);
            PwBox.TabIndex = 20;
            // 
            // NameBox
            // 
            NameBox.Location = new Point(21, 176);
            NameBox.Name = "NameBox";
            NameBox.Size = new Size(212, 23);
            NameBox.TabIndex = 21;
            // 
            // ZipCodeBox
            // 
            ZipCodeBox.Location = new Point(21, 222);
            ZipCodeBox.Name = "ZipCodeBox";
            ZipCodeBox.ReadOnly = true;
            ZipCodeBox.Size = new Size(144, 23);
            ZipCodeBox.TabIndex = 22;
            // 
            // AddressBox
            // 
            AddressBox.Location = new Point(21, 251);
            AddressBox.Name = "AddressBox";
            AddressBox.ReadOnly = true;
            AddressBox.Size = new Size(212, 23);
            AddressBox.TabIndex = 23;
            // 
            // DeptBox
            // 
            DeptBox.Location = new Point(21, 297);
            DeptBox.Name = "DeptBox";
            DeptBox.ReadOnly = true;
            DeptBox.Size = new Size(212, 23);
            DeptBox.TabIndex = 24;
            // 
            // SearchAddressButton
            // 
            SearchAddressButton.Location = new Point(171, 222);
            SearchAddressButton.Name = "SearchAddressButton";
            SearchAddressButton.Size = new Size(62, 23);
            SearchAddressButton.TabIndex = 25;
            SearchAddressButton.Text = "검색";
            SearchAddressButton.UseVisualStyleBackColor = true;
            SearchAddressButton.Click += SearchAddressButton_Click;
            // 
            // SaveIButton
            // 
            SaveIButton.Location = new Point(21, 326);
            SaveIButton.Name = "SaveIButton";
            SaveIButton.Size = new Size(144, 23);
            SaveIButton.TabIndex = 26;
            SaveIButton.Text = "변경 내용 저장";
            SaveIButton.UseVisualStyleBackColor = true;
            SaveIButton.Click += SaveIButton_Click;
            // 
            // CancleIButton
            // 
            CancleIButton.Location = new Point(171, 326);
            CancleIButton.Name = "CancleIButton";
            CancleIButton.Size = new Size(62, 23);
            CancleIButton.TabIndex = 27;
            CancleIButton.Text = "취소";
            CancleIButton.UseVisualStyleBackColor = true;
            CancleIButton.Click += CancleIButton_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(21, 112);
            label10.Name = "label10";
            label10.Size = new Size(83, 15);
            label10.TabIndex = 28;
            label10.Text = "비밀번호 확인";
            // 
            // CheckPwBox
            // 
            CheckPwBox.Location = new Point(21, 130);
            CheckPwBox.Name = "CheckPwBox";
            CheckPwBox.PasswordChar = '*';
            CheckPwBox.Size = new Size(212, 23);
            CheckPwBox.TabIndex = 29;
            // 
            // MulProfilePage
            // 
            MulProfilePage.Controls.Add(panel3);
            MulProfilePage.Controls.Add(ProfileFLP);
            MulProfilePage.Location = new Point(4, 24);
            MulProfilePage.Margin = new Padding(2);
            MulProfilePage.Name = "MulProfilePage";
            MulProfilePage.Padding = new Padding(2);
            MulProfilePage.Size = new Size(252, 365);
            MulProfilePage.TabIndex = 1;
            MulProfilePage.Text = "멀티프로필";
            MulProfilePage.UseVisualStyleBackColor = true;
            // 
            // ProfileFLP
            // 
            ProfileFLP.Controls.Add(panel);
            ProfileFLP.Location = new Point(19, 56);
            ProfileFLP.Name = "ProfileFLP";
            ProfileFLP.Size = new Size(218, 292);
            ProfileFLP.TabIndex = 0;
            // 
            // panel
            // 
            panel.Controls.Add(ProfileImageMBox);
            panel.Controls.Add(EditButton);
            panel.Controls.Add(NicknameLabel);
            panel.Location = new Point(2, 2);
            panel.Margin = new Padding(2);
            panel.Name = "panel";
            panel.Size = new Size(216, 35);
            panel.TabIndex = 7;
            // 
            // NicknameLabel
            // 
            NicknameLabel.AutoSize = true;
            NicknameLabel.Location = new Point(38, 10);
            NicknameLabel.Margin = new Padding(2, 0, 2, 0);
            NicknameLabel.Name = "NicknameLabel";
            NicknameLabel.Size = new Size(31, 15);
            NicknameLabel.TabIndex = 0;
            NicknameLabel.Text = "별명";
            // 
            // EditButton
            // 
            EditButton.Location = new Point(168, 6);
            EditButton.Margin = new Padding(2);
            EditButton.Name = "EditButton";
            EditButton.Size = new Size(39, 22);
            EditButton.TabIndex = 2;
            EditButton.Text = "관리";
            EditButton.UseVisualStyleBackColor = true;
            EditButton.Click += EditButton_Click;
            // 
            // ProfileImageMBox
            // 
            ProfileImageMBox.Image = DBP_WinformChat.Properties.Resources._default;
            ProfileImageMBox.Location = new Point(3, 3);
            ProfileImageMBox.Name = "ProfileImageMBox";
            ProfileImageMBox.Size = new Size(30, 29);
            ProfileImageMBox.SizeMode = PictureBoxSizeMode.StretchImage;
            ProfileImageMBox.TabIndex = 4;
            ProfileImageMBox.TabStop = false;
            // 
            // panel3
            // 
            panel3.Controls.Add(AddMulProfileButton);
            panel3.Controls.Add(label7);
            panel3.Location = new Point(19, 18);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(218, 33);
            panel3.TabIndex = 6;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(3, 9);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(95, 15);
            label7.TabIndex = 0;
            label7.Text = "멀티프로필 관리";
            // 
            // AddMulProfileButton
            // 
            AddMulProfileButton.Location = new Point(170, 5);
            AddMulProfileButton.Margin = new Padding(2);
            AddMulProfileButton.Name = "AddMulProfileButton";
            AddMulProfileButton.Size = new Size(39, 22);
            AddMulProfileButton.TabIndex = 2;
            AddMulProfileButton.Text = "추가";
            AddMulProfileButton.UseVisualStyleBackColor = true;
            AddMulProfileButton.Click += AddMulProfileButton_Click;
            // 
            // DefaultProfilePage
            // 
            DefaultProfilePage.Controls.Add(EditMemberLabel);
            DefaultProfilePage.Controls.Add(label1);
            DefaultProfilePage.Controls.Add(MemberFLP);
            DefaultProfilePage.Controls.Add(StatusBox);
            DefaultProfilePage.Controls.Add(NicknameBox);
            DefaultProfilePage.Controls.Add(panel2);
            DefaultProfilePage.Controls.Add(ChangeProfileImageButton);
            DefaultProfilePage.Controls.Add(ProfileImagePBox);
            DefaultProfilePage.Controls.Add(DeptLabel);
            DefaultProfilePage.Location = new Point(4, 24);
            DefaultProfilePage.Margin = new Padding(2);
            DefaultProfilePage.Name = "DefaultProfilePage";
            DefaultProfilePage.Padding = new Padding(2);
            DefaultProfilePage.Size = new Size(252, 365);
            DefaultProfilePage.TabIndex = 0;
            DefaultProfilePage.Text = "프로필";
            DefaultProfilePage.UseVisualStyleBackColor = true;
            // 
            // DeptLabel
            // 
            DeptLabel.AutoSize = true;
            DeptLabel.ForeColor = SystemColors.AppWorkspace;
            DeptLabel.Location = new Point(72, 46);
            DeptLabel.Margin = new Padding(2, 0, 2, 0);
            DeptLabel.Name = "DeptLabel";
            DeptLabel.Size = new Size(31, 15);
            DeptLabel.TabIndex = 18;
            DeptLabel.Text = "부서";
            // 
            // ProfileImagePBox
            // 
            ProfileImagePBox.Image = DBP_WinformChat.Properties.Resources._default;
            ProfileImagePBox.Location = new Point(16, 16);
            ProfileImagePBox.Margin = new Padding(2);
            ProfileImagePBox.Name = "ProfileImagePBox";
            ProfileImagePBox.Size = new Size(50, 48);
            ProfileImagePBox.SizeMode = PictureBoxSizeMode.Zoom;
            ProfileImagePBox.TabIndex = 16;
            ProfileImagePBox.TabStop = false;
            ProfileImagePBox.Tag = "";
            // 
            // ChangeProfileImageButton
            // 
            ChangeProfileImageButton.BackgroundImage = DBP_WinformChat.Properties.Resources.kamera;
            ChangeProfileImageButton.BackgroundImageLayout = ImageLayout.Zoom;
            ChangeProfileImageButton.FlatAppearance.BorderSize = 0;
            ChangeProfileImageButton.FlatStyle = FlatStyle.Flat;
            ChangeProfileImageButton.Location = new Point(46, 44);
            ChangeProfileImageButton.Margin = new Padding(2);
            ChangeProfileImageButton.Name = "ChangeProfileImageButton";
            ChangeProfileImageButton.Size = new Size(20, 20);
            ChangeProfileImageButton.TabIndex = 20;
            ChangeProfileImageButton.UseVisualStyleBackColor = true;
            ChangeProfileImageButton.Click += ChangeProfileImageButton_Click;
            // 
            // NicknameBox
            // 
            NicknameBox.Location = new Point(70, 16);
            NicknameBox.Margin = new Padding(2);
            NicknameBox.Name = "NicknameBox";
            NicknameBox.Size = new Size(165, 23);
            NicknameBox.TabIndex = 22;
            // 
            // panel2
            // 
            panel2.Controls.Add(CancelPButton);
            panel2.Controls.Add(SavePButton);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(2, 333);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(248, 30);
            panel2.TabIndex = 17;
            // 
            // SavePButton
            // 
            SavePButton.Location = new Point(14, 4);
            SavePButton.Margin = new Padding(2);
            SavePButton.Name = "SavePButton";
            SavePButton.Size = new Size(160, 22);
            SavePButton.TabIndex = 0;
            SavePButton.Text = "저장하기";
            SavePButton.UseVisualStyleBackColor = true;
            SavePButton.Click += SavePButton_Click;
            // 
            // CancelPButton
            // 
            CancelPButton.Location = new Point(182, 4);
            CancelPButton.Margin = new Padding(2);
            CancelPButton.Name = "CancelPButton";
            CancelPButton.Size = new Size(51, 22);
            CancelPButton.TabIndex = 1;
            CancelPButton.Text = "취소";
            CancelPButton.UseVisualStyleBackColor = true;
            CancelPButton.Click += CancelPButton_Click;
            // 
            // StatusBox
            // 
            StatusBox.Location = new Point(16, 94);
            StatusBox.Margin = new Padding(2);
            StatusBox.Multiline = true;
            StatusBox.Name = "StatusBox";
            StatusBox.Size = new Size(219, 51);
            StatusBox.TabIndex = 23;
            // 
            // MemberFLP
            // 
            MemberFLP.Location = new Point(16, 170);
            MemberFLP.Name = "MemberFLP";
            MemberFLP.Size = new Size(219, 147);
            MemberFLP.TabIndex = 19;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 79);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(67, 15);
            label1.TabIndex = 21;
            label1.Text = "상태메시지";
            // 
            // EditMemberLabel
            // 
            EditMemberLabel.AutoSize = true;
            EditMemberLabel.Location = new Point(18, 154);
            EditMemberLabel.Name = "EditMemberLabel";
            EditMemberLabel.Size = new Size(59, 15);
            EditMemberLabel.TabIndex = 15;
            EditMemberLabel.Text = "멤버 관리";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(DefaultProfilePage);
            tabControl.Controls.Add(MulProfilePage);
            tabControl.Controls.Add(UserInfoPage);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Margin = new Padding(2);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(260, 393);
            tabControl.TabIndex = 1;
            // 
            // EditInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(260, 393);
            Controls.Add(tabControl);
            Name = "EditInfoForm";
            Text = "EditInfoForm";
            UserInfoPage.ResumeLayout(false);
            UserInfoPage.PerformLayout();
            MulProfilePage.ResumeLayout(false);
            ProfileFLP.ResumeLayout(false);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImageMBox).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            DefaultProfilePage.ResumeLayout(false);
            DefaultProfilePage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImagePBox).EndInit();
            panel2.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabPage UserInfoPage;
        private TextBox CheckPwBox;
        private TextBox DeptBox;
        private TextBox AddressBox;
        private TextBox ZipCodeBox;
        private TextBox NameBox;
        private TextBox PwBox;
        private TextBox IdBox;
        private Label label10;
        private Button CancleIButton;
        private Button SaveIButton;
        private Button SearchAddressButton;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label6;
        private Label label8;
        private TabPage MulProfilePage;
        private Panel panel3;
        private Button AddMulProfileButton;
        private Label label7;
        private FlowLayoutPanel ProfileFLP;
        private Panel panel;
        private PictureBox ProfileImageMBox;
        private Button EditButton;
        private Label NicknameLabel;
        private TabPage DefaultProfilePage;
        private Label EditMemberLabel;
        private Label label1;
        private FlowLayoutPanel MemberFLP;
        private TextBox StatusBox;
        private TextBox NicknameBox;
        private Panel panel2;
        private Button CancelPButton;
        private Button SavePButton;
        private Button ChangeProfileImageButton;
        private PictureBox ProfileImagePBox;
        private Label DeptLabel;
        private TabControl tabControl;
    }
}