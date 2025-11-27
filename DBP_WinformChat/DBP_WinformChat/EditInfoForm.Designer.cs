using Org.BouncyCastle.Asn1.Crmf;
using System.Drawing.Printing;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace leehaeun
{
    partial class EditInfoForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            MulProfilePage = new TabPage();
            panel = new Panel();
            ProfileImage = new PictureBox();
            EditButton = new Button();
            NicknameLabel = new Label();
            flowProfiles = new FlowLayoutPanel();
            panel3 = new Panel();
            AddMulProfileButton = new Button();
            label7 = new Label();
            DefaultProfilePage = new TabPage();
            panelBasic = new Panel();
            label9 = new Label();
            label1 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            textBox1 = new TextBox();
            panel2 = new Panel();
            CancelButton = new Button();
            SaveButton = new Button();
            textBox2 = new TextBox();
            button3 = new Button();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            tabControl1 = new TabControl();
            UserInfoPage = new TabPage();
            button1 = new Button();
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
            textBox3 = new TextBox();
            label10 = new Label();
            MulProfilePage.SuspendLayout();
            panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImage).BeginInit();
            flowProfiles.SuspendLayout();
            panel3.SuspendLayout();
            DefaultProfilePage.SuspendLayout();
            panelBasic.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabControl1.SuspendLayout();
            UserInfoPage.SuspendLayout();
            SuspendLayout();
            // 
            // MulProfilePage
            // 
            MulProfilePage.Controls.Add(panel);
            MulProfilePage.Controls.Add(flowProfiles);
            MulProfilePage.Location = new Point(4, 24);
            MulProfilePage.Margin = new Padding(2);
            MulProfilePage.Name = "MulProfilePage";
            MulProfilePage.Padding = new Padding(2);
            MulProfilePage.Size = new Size(255, 368);
            MulProfilePage.TabIndex = 1;
            MulProfilePage.Text = "멀티프로필";
            MulProfilePage.UseVisualStyleBackColor = true;
            // 
            // panel
            // 
            panel.Controls.Add(ProfileImage);
            panel.Controls.Add(EditButton);
            panel.Controls.Add(NicknameLabel);
            panel.Location = new Point(19, 19);
            panel.Margin = new Padding(2);
            panel.Name = "panel";
            panel.Size = new Size(218, 35);
            panel.TabIndex = 7;
            // 
            // ProfileImage
            // 
            ProfileImage.Image = DBP_WinformChat.Properties.Resources._default;
            ProfileImage.Location = new Point(3, 3);
            ProfileImage.Name = "ProfileImage";
            ProfileImage.Size = new Size(30, 29);
            ProfileImage.SizeMode = PictureBoxSizeMode.StretchImage;
            ProfileImage.TabIndex = 4;
            ProfileImage.TabStop = false;
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
            // flowProfiles
            // 
            flowProfiles.Controls.Add(panel3);
            flowProfiles.Location = new Point(19, 56);
            flowProfiles.Name = "flowProfiles";
            flowProfiles.Size = new Size(218, 292);
            flowProfiles.TabIndex = 0;
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
            AddMulProfileButton.Text = "+ 추가";
            AddMulProfileButton.UseVisualStyleBackColor = true;
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
            // DefaultProfilePage
            // 
            DefaultProfilePage.Controls.Add(panelBasic);
            DefaultProfilePage.Location = new Point(4, 24);
            DefaultProfilePage.Margin = new Padding(2);
            DefaultProfilePage.Name = "DefaultProfilePage";
            DefaultProfilePage.Padding = new Padding(2);
            DefaultProfilePage.Size = new Size(255, 368);
            DefaultProfilePage.TabIndex = 0;
            DefaultProfilePage.Text = "기본프로필";
            DefaultProfilePage.UseVisualStyleBackColor = true;
            // 
            // panelBasic
            // 
            panelBasic.Controls.Add(label9);
            panelBasic.Controls.Add(label1);
            panelBasic.Controls.Add(flowLayoutPanel1);
            panelBasic.Controls.Add(textBox1);
            panelBasic.Controls.Add(panel2);
            panelBasic.Controls.Add(textBox2);
            panelBasic.Controls.Add(button3);
            panelBasic.Controls.Add(pictureBox1);
            panelBasic.Controls.Add(label2);
            panelBasic.Dock = DockStyle.Fill;
            panelBasic.Location = new Point(2, 2);
            panelBasic.Margin = new Padding(2);
            panelBasic.Name = "panelBasic";
            panelBasic.Padding = new Padding(16, 15, 16, 15);
            panelBasic.Size = new Size(251, 364);
            panelBasic.TabIndex = 0;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(16, 153);
            label9.Name = "label9";
            label9.Size = new Size(59, 15);
            label9.TabIndex = 0;
            label9.Text = "멤버 관리";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 78);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(67, 15);
            label1.TabIndex = 12;
            label1.Text = "상태메시지";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(16, 171);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(219, 147);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(16, 95);
            textBox1.Margin = new Padding(2);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(219, 51);
            textBox1.TabIndex = 14;
            // 
            // panel2
            // 
            panel2.Controls.Add(CancelButton);
            panel2.Controls.Add(SaveButton);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(16, 319);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(219, 30);
            panel2.TabIndex = 2;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(166, 4);
            CancelButton.Margin = new Padding(2);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(51, 22);
            CancelButton.TabIndex = 1;
            CancelButton.Text = "취소";
            CancelButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(2, 4);
            SaveButton.Margin = new Padding(2);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(160, 22);
            SaveButton.TabIndex = 0;
            SaveButton.Text = "저장하기";
            SaveButton.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(70, 17);
            textBox2.Margin = new Padding(2);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(165, 23);
            textBox2.TabIndex = 13;
            // 
            // button3
            // 
            button3.BackgroundImage = DBP_WinformChat.Properties.Resources.kamera;
            button3.BackgroundImageLayout = ImageLayout.Zoom;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(46, 45);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(20, 20);
            button3.TabIndex = 4;
            button3.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = DBP_WinformChat.Properties.Resources._default;
            pictureBox1.Location = new Point(16, 17);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 48);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Tag = "default.jpg";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.AppWorkspace;
            label2.Location = new Point(70, 45);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 2;
            label2.Text = "부서";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(DefaultProfilePage);
            tabControl1.Controls.Add(MulProfilePage);
            tabControl1.Controls.Add(UserInfoPage);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(263, 396);
            tabControl1.TabIndex = 0;
            // 
            // UserInfoPage
            // 
            UserInfoPage.Controls.Add(textBox3);
            UserInfoPage.Controls.Add(label10);
            UserInfoPage.Controls.Add(button1);
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
            UserInfoPage.Size = new Size(255, 368);
            UserInfoPage.TabIndex = 2;
            UserInfoPage.Text = "계정 정보";
            UserInfoPage.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(193, 326);
            button1.Name = "button1";
            button1.Size = new Size(40, 23);
            button1.TabIndex = 27;
            button1.Text = "취소";
            button1.UseVisualStyleBackColor = true;
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
            // textBox3
            // 
            textBox3.Location = new Point(21, 130);
            textBox3.Name = "textBox3";
            textBox3.PasswordChar = '*';
            textBox3.Size = new Size(212, 23);
            textBox3.TabIndex = 29;
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
            // EditInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(263, 396);
            Controls.Add(tabControl1);
            Margin = new Padding(2);
            Name = "EditInfoForm";
            Text = "Form1";
            MulProfilePage.ResumeLayout(false);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImage).EndInit();
            flowProfiles.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            DefaultProfilePage.ResumeLayout(false);
            panelBasic.ResumeLayout(false);
            panelBasic.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabControl1.ResumeLayout(false);
            UserInfoPage.ResumeLayout(false);
            UserInfoPage.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label UserNamelabel;
        private TabPage MulProfilePage;
        private FlowLayoutPanel flowProfiles;
        private Panel panel3;
        private Button AddMulProfileButton;
        private Label label7;
        private Panel panel;
        private PictureBox ProfileImage;
        private Button EditButton;
        private Label NicknameLabel;
        private TabPage DefaultProfilePage;
        private Panel panelBasic;
        private Button CancelButton;
        private Button SaveButton;
        private TabControl tabControl1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label9;
        private Panel panel2;
        private Label label1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button3;
        private PictureBox pictureBox1;
        private Label label2;
        private TabPage UserInfoPage;
        private Button button1;
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
        private TextBox textBox3;
        private Label label10;
    }
}