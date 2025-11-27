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
            tabControl = new TabControl();
            DefaultProfilePage = new TabPage();
            EditMemberLabel = new Label();
            label1 = new Label();
            MemberFLP = new FlowLayoutPanel();
            StatusBox = new TextBox();
            panel2 = new Panel();
            CancelButton0 = new Button();
            SaveButton = new Button();
            NicknameBox = new TextBox();
            ChangeProfileImageButton = new Button();
            ProfileImageBox0 = new PictureBox();
            DeptLabel = new Label();
            MulProfilePage = new TabPage();
            panel = new Panel();
            ProfileImageBox1 = new PictureBox();
            EditButton = new Button();
            NicknameLabel = new Label();
            ProfileFLP = new FlowLayoutPanel();
            panel3 = new Panel();
            AddMulProfileButton = new Button();
            label7 = new Label();
            UserInfoPage = new TabPage();
            CheckPwBox = new TextBox();
            label10 = new Label();
            CancleButton2 = new Button();
            SaveInfoButton = new Button();
            SearchAddressButton = new Button();
            DeptBox = new TextBox();
            AddressBox = new TextBox();
            ZipCodeBox = new TextBox();
            NameBox = new TextBox();
            PwBox = new TextBox();
            IdBox = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label6 = new Label();
            label8 = new Label();
            tabControl.SuspendLayout();
            DefaultProfilePage.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox0).BeginInit();
            MulProfilePage.SuspendLayout();
            panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox1).BeginInit();
            ProfileFLP.SuspendLayout();
            panel3.SuspendLayout();
            UserInfoPage.SuspendLayout();
            SuspendLayout();
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
            // DefaultProfilePage
            // 
            DefaultProfilePage.Controls.Add(EditMemberLabel);
            DefaultProfilePage.Controls.Add(label1);
            DefaultProfilePage.Controls.Add(MemberFLP);
            DefaultProfilePage.Controls.Add(StatusBox);
            DefaultProfilePage.Controls.Add(panel2);
            DefaultProfilePage.Controls.Add(NicknameBox);
            DefaultProfilePage.Controls.Add(ChangeProfileImageButton);
            DefaultProfilePage.Controls.Add(ProfileImageBox0);
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
            // EditMemberLabel
            // 
            EditMemberLabel.AutoSize = true;
            EditMemberLabel.Location = new Point(18, 154);
            EditMemberLabel.Name = "EditMemberLabel";
            EditMemberLabel.Size = new Size(59, 15);
            EditMemberLabel.TabIndex = 15;
            EditMemberLabel.Text = "멤버 관리";
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
            // MemberFLP
            // 
            MemberFLP.Location = new Point(16, 170);
            MemberFLP.Name = "MemberFLP";
            MemberFLP.Size = new Size(219, 147);
            MemberFLP.TabIndex = 19;
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
            // panel2
            // 
            panel2.Controls.Add(CancelButton0);
            panel2.Controls.Add(SaveButton);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(2, 333);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(248, 30);
            panel2.TabIndex = 17;
            // 
            // CancelButton0
            // 
            CancelButton0.Location = new Point(182, 4);
            CancelButton0.Margin = new Padding(2);
            CancelButton0.Name = "CancelButton0";
            CancelButton0.Size = new Size(51, 22);
            CancelButton0.TabIndex = 1;
            CancelButton0.Text = "취소";
            CancelButton0.UseVisualStyleBackColor = true;
            CancelButton0.Click += CancelButton0_Click;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(14, 4);
            SaveButton.Margin = new Padding(2);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(160, 22);
            SaveButton.TabIndex = 0;
            SaveButton.Text = "저장하기";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // NicknameBox
            // 
            NicknameBox.Location = new Point(70, 16);
            NicknameBox.Margin = new Padding(2);
            NicknameBox.Name = "NicknameBox";
            NicknameBox.Size = new Size(165, 23);
            NicknameBox.TabIndex = 22;
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
            // 
            // ProfileImageBox0
            // 
            ProfileImageBox0.Image = DBP_WinformChat.Properties.Resources._default;
            ProfileImageBox0.Location = new Point(16, 16);
            ProfileImageBox0.Margin = new Padding(2);
            ProfileImageBox0.Name = "ProfileImageBox0";
            ProfileImageBox0.Size = new Size(50, 48);
            ProfileImageBox0.SizeMode = PictureBoxSizeMode.Zoom;
            ProfileImageBox0.TabIndex = 16;
            ProfileImageBox0.TabStop = false;
            ProfileImageBox0.Tag = "default.jpg";
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
            // MulProfilePage
            // 
            MulProfilePage.Controls.Add(panel);
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
            // panel
            // 
            panel.Controls.Add(ProfileImageBox1);
            panel.Controls.Add(EditButton);
            panel.Controls.Add(NicknameLabel);
            panel.Location = new Point(19, 19);
            panel.Margin = new Padding(2);
            panel.Name = "panel";
            panel.Size = new Size(218, 35);
            panel.TabIndex = 7;
            // 
            // ProfileImageBox1
            // 
            ProfileImageBox1.Image = DBP_WinformChat.Properties.Resources._default;
            ProfileImageBox1.Location = new Point(3, 3);
            ProfileImageBox1.Name = "ProfileImageBox1";
            ProfileImageBox1.Size = new Size(30, 29);
            ProfileImageBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            ProfileImageBox1.TabIndex = 4;
            ProfileImageBox1.TabStop = false;
            // 
            // EditButton
            // 
            EditButton.Location = new Point(155, 7);
            EditButton.Margin = new Padding(2);
            EditButton.Name = "EditButton";
            EditButton.Size = new Size(52, 22);
            EditButton.TabIndex = 2;
            EditButton.Text = "관리";
            EditButton.UseVisualStyleBackColor = true;
            EditButton.Click += EditButton_Click;
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
            // ProfileFLP
            // 
            ProfileFLP.Controls.Add(panel3);
            ProfileFLP.Location = new Point(19, 56);
            ProfileFLP.Name = "ProfileFLP";
            ProfileFLP.Size = new Size(218, 292);
            ProfileFLP.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(AddMulProfileButton);
            panel3.Controls.Add(label7);
            panel3.Location = new Point(2, 2);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(216, 33);
            panel3.TabIndex = 6;
            // 
            // AddMulProfileButton
            // 
            AddMulProfileButton.Location = new Point(153, 5);
            AddMulProfileButton.Margin = new Padding(2);
            AddMulProfileButton.Name = "AddMulProfileButton";
            AddMulProfileButton.Size = new Size(52, 22);
            AddMulProfileButton.TabIndex = 2;
            AddMulProfileButton.Text = "추가";
            AddMulProfileButton.UseVisualStyleBackColor = true;
            AddMulProfileButton.Click += AddMulProfileButton_Click;
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
            // UserInfoPage
            // 
            UserInfoPage.Controls.Add(CheckPwBox);
            UserInfoPage.Controls.Add(label10);
            UserInfoPage.Controls.Add(CancleButton2);
            UserInfoPage.Controls.Add(SaveInfoButton);
            UserInfoPage.Controls.Add(SearchAddressButton);
            UserInfoPage.Controls.Add(DeptBox);
            UserInfoPage.Controls.Add(AddressBox);
            UserInfoPage.Controls.Add(ZipCodeBox);
            UserInfoPage.Controls.Add(NameBox);
            UserInfoPage.Controls.Add(PwBox);
            UserInfoPage.Controls.Add(IdBox);
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
            // CheckPwBox
            // 
            CheckPwBox.Location = new Point(21, 130);
            CheckPwBox.Name = "CheckPwBox";
            CheckPwBox.PasswordChar = '*';
            CheckPwBox.Size = new Size(212, 23);
            CheckPwBox.TabIndex = 29;
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
            // CancleButton2
            // 
            CancleButton2.Location = new Point(193, 326);
            CancleButton2.Name = "CancleButton2";
            CancleButton2.Size = new Size(40, 23);
            CancleButton2.TabIndex = 27;
            CancleButton2.Text = "취소";
            CancleButton2.UseVisualStyleBackColor = true;
            // 
            // SaveInfoButton
            // 
            SaveInfoButton.Location = new Point(21, 326);
            SaveInfoButton.Name = "SaveInfoButton";
            SaveInfoButton.Size = new Size(166, 23);
            SaveInfoButton.TabIndex = 26;
            SaveInfoButton.Text = "변경 내용 저장";
            SaveInfoButton.UseVisualStyleBackColor = true;
            // 
            // SearchAddressButton
            // 
            SearchAddressButton.Location = new Point(171, 222);
            SearchAddressButton.Name = "SearchAddressButton";
            SearchAddressButton.Size = new Size(62, 23);
            SearchAddressButton.TabIndex = 25;
            SearchAddressButton.Text = "검색";
            SearchAddressButton.UseVisualStyleBackColor = true;
            // 
            // DeptBox
            // 
            DeptBox.Location = new Point(21, 297);
            DeptBox.Name = "DeptBox";
            DeptBox.ReadOnly = true;
            DeptBox.Size = new Size(212, 23);
            DeptBox.TabIndex = 24;
            // 
            // AddressBox
            // 
            AddressBox.Location = new Point(21, 251);
            AddressBox.Name = "AddressBox";
            AddressBox.ReadOnly = true;
            AddressBox.Size = new Size(212, 23);
            AddressBox.TabIndex = 23;
            // 
            // ZipCodeBox
            // 
            ZipCodeBox.Location = new Point(21, 222);
            ZipCodeBox.Name = "ZipCodeBox";
            ZipCodeBox.ReadOnly = true;
            ZipCodeBox.Size = new Size(144, 23);
            ZipCodeBox.TabIndex = 22;
            // 
            // NameBox
            // 
            NameBox.Location = new Point(21, 176);
            NameBox.Name = "NameBox";
            NameBox.Size = new Size(212, 23);
            NameBox.TabIndex = 21;
            // 
            // PwBox
            // 
            PwBox.Location = new Point(21, 84);
            PwBox.Name = "PwBox";
            PwBox.PasswordChar = '*';
            PwBox.Size = new Size(212, 23);
            PwBox.TabIndex = 20;
            // 
            // IdBox
            // 
            IdBox.Location = new Point(21, 38);
            IdBox.Name = "IdBox";
            IdBox.ReadOnly = true;
            IdBox.Size = new Size(212, 23);
            IdBox.TabIndex = 19;
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
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 204);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 17;
            label4.Text = "주소";
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
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(21, 66);
            label6.Name = "label6";
            label6.Size = new Size(55, 15);
            label6.TabIndex = 15;
            label6.Text = "비밀번호";
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
            // EditInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(260, 393);
            Controls.Add(tabControl);
            Name = "EditInfoForm";
            Text = "EditInfoForm";
            tabControl.ResumeLayout(false);
            DefaultProfilePage.ResumeLayout(false);
            DefaultProfilePage.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox0).EndInit();
            MulProfilePage.ResumeLayout(false);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox1).EndInit();
            ProfileFLP.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            UserInfoPage.ResumeLayout(false);
            UserInfoPage.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage DefaultProfilePage;
        private Label EditMemberLabel;
        private Label label1;
        private FlowLayoutPanel MemberFLP;
        private TextBox StatusBox;
        private Panel panel2;
        private Button CancelButton0;
        private Button SaveButton;
        private TextBox NicknameBox;
        private Button ChangeProfileImageButton;
        private PictureBox ProfileImageBox0;
        private Label DeptLabel;
        private TabPage MulProfilePage;
        private Panel panel;
        private PictureBox ProfileImageBox1;
        private Button EditButton;
        private Label NicknameLabel;
        private FlowLayoutPanel ProfileFLP;
        private Panel panel3;
        private Button AddMulProfileButton;
        private Label label7;
        private TabPage UserInfoPage;
        private TextBox CheckPwBox;
        private Label label10;
        private Button CancleButton2;
        private Button SaveInfoButton;
        private Button SearchAddressButton;
        private TextBox DeptBox;
        private TextBox AddressBox;
        private TextBox ZipCodeBox;
        private TextBox NameBox;
        private TextBox PwBox;
        private TextBox IdBox;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label6;
        private Label label8;
    }
}