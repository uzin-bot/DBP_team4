using Org.BouncyCastle.Asn1.Crmf;
using System.Drawing.Printing;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace leehaeun
{
    partial class EditProfileForm
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
            tabPageMultiProfile = new TabPage();
            flowProfiles = new FlowLayoutPanel();
            panel3 = new Panel();
            AddMulProfileButton = new Button();
            label7 = new Label();
            panel = new Panel();
            ProfileImage = new PictureBox();
            EditButton = new Button();
            NicknameLabel = new Label();
            tabPageBasic = new TabPage();
            panelBasic = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label9 = new Label();
            panel2 = new Panel();
            CancelButton = new Button();
            SaveButton = new Button();
            panelProfileCard = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            label3 = new Label();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            button2 = new Button();
            label4 = new Label();
            pictureBox2 = new PictureBox();
            button4 = new Button();
            label5 = new Label();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            button5 = new Button();
            label8 = new Label();
            pictureBox3 = new PictureBox();
            panel1 = new Panel();
            label1 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button3 = new Button();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            button1 = new Button();
            label6 = new Label();
            StatusBox = new TextBox();
            NicknameBox = new TextBox();
            ChangeProfileImageButton = new Button();
            Deptlabel = new Label();
            ProfileImageBox = new PictureBox();
            tabControl1 = new TabControl();
            tabPageMultiProfile.SuspendLayout();
            flowProfiles.SuspendLayout();
            panel3.SuspendLayout();
            panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImage).BeginInit();
            tabPageBasic.SuspendLayout();
            panelBasic.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            panelProfileCard.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox).BeginInit();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // tabPageMultiProfile
            // 
            tabPageMultiProfile.Controls.Add(flowProfiles);
            tabPageMultiProfile.Location = new Point(4, 24);
            tabPageMultiProfile.Margin = new Padding(2);
            tabPageMultiProfile.Name = "tabPageMultiProfile";
            tabPageMultiProfile.Padding = new Padding(2);
            tabPageMultiProfile.Size = new Size(276, 462);
            tabPageMultiProfile.TabIndex = 1;
            tabPageMultiProfile.Text = "멀티프로필";
            tabPageMultiProfile.UseVisualStyleBackColor = true;
            // 
            // flowProfiles
            // 
            flowProfiles.Controls.Add(panel3);
            flowProfiles.Controls.Add(panel);
            flowProfiles.Location = new Point(0, 0);
            flowProfiles.Name = "flowProfiles";
            flowProfiles.Size = new Size(276, 462);
            flowProfiles.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(AddMulProfileButton);
            panel3.Controls.Add(label7);
            panel3.Location = new Point(2, 2);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(272, 33);
            panel3.TabIndex = 6;
            // 
            // AddMulProfileButton
            // 
            AddMulProfileButton.Location = new Point(215, 5);
            AddMulProfileButton.Margin = new Padding(2);
            AddMulProfileButton.Name = "AddMulProfileButton";
            AddMulProfileButton.Size = new Size(52, 22);
            AddMulProfileButton.TabIndex = 2;
            AddMulProfileButton.Text = "+ 추가";
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
            // panel
            // 
            panel.Controls.Add(ProfileImage);
            panel.Controls.Add(EditButton);
            panel.Controls.Add(NicknameLabel);
            panel.Location = new Point(2, 39);
            panel.Margin = new Padding(2);
            panel.Name = "panel";
            panel.Size = new Size(272, 35);
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
            EditButton.Location = new Point(215, 6);
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
            // tabPageBasic
            // 
            tabPageBasic.Controls.Add(panelBasic);
            tabPageBasic.Location = new Point(4, 24);
            tabPageBasic.Margin = new Padding(2);
            tabPageBasic.Name = "tabPageBasic";
            tabPageBasic.Padding = new Padding(2);
            tabPageBasic.Size = new Size(276, 462);
            tabPageBasic.TabIndex = 0;
            tabPageBasic.Text = "기본 정보";
            tabPageBasic.UseVisualStyleBackColor = true;
            // 
            // panelBasic
            // 
            panelBasic.Controls.Add(flowLayoutPanel1);
            panelBasic.Controls.Add(panel2);
            panelBasic.Controls.Add(panelProfileCard);
            panelBasic.Dock = DockStyle.Fill;
            panelBasic.Location = new Point(2, 2);
            panelBasic.Margin = new Padding(2);
            panelBasic.Name = "panelBasic";
            panelBasic.Padding = new Padding(16, 15, 16, 15);
            panelBasic.Size = new Size(272, 458);
            panelBasic.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label9);
            flowLayoutPanel1.Location = new Point(16, 184);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(240, 228);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(3, 0);
            label9.Name = "label9";
            label9.Padding = new Padding(5);
            label9.Size = new Size(69, 25);
            label9.TabIndex = 0;
            label9.Text = "멤버 관리";
            // 
            // panel2
            // 
            panel2.Controls.Add(CancelButton);
            panel2.Controls.Add(SaveButton);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(16, 413);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(240, 30);
            panel2.TabIndex = 2;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(180, 4);
            CancelButton.Margin = new Padding(2);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(51, 22);
            CancelButton.TabIndex = 1;
            CancelButton.Text = "취소";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(12, 4);
            SaveButton.Margin = new Padding(2);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(156, 22);
            SaveButton.TabIndex = 0;
            SaveButton.Text = "저장하기";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // panelProfileCard
            // 
            panelProfileCard.BorderStyle = BorderStyle.FixedSingle;
            panelProfileCard.Controls.Add(panel4);
            panelProfileCard.Controls.Add(panel1);
            panelProfileCard.Controls.Add(button1);
            panelProfileCard.Controls.Add(label6);
            panelProfileCard.Controls.Add(StatusBox);
            panelProfileCard.Controls.Add(NicknameBox);
            panelProfileCard.Controls.Add(ChangeProfileImageButton);
            panelProfileCard.Controls.Add(Deptlabel);
            panelProfileCard.Controls.Add(ProfileImageBox);
            panelProfileCard.Dock = DockStyle.Top;
            panelProfileCard.Location = new Point(16, 15);
            panelProfileCard.Margin = new Padding(2);
            panelProfileCard.Name = "panelProfileCard";
            panelProfileCard.Size = new Size(240, 167);
            panelProfileCard.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(button4);
            panel4.Controls.Add(label5);
            panel4.Controls.Add(textBox5);
            panel4.Controls.Add(textBox6);
            panel4.Controls.Add(button5);
            panel4.Controls.Add(label8);
            panel4.Controls.Add(pictureBox3);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 167);
            panel4.Margin = new Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new Size(238, 167);
            panel4.TabIndex = 17;
            // 
            // panel5
            // 
            panel5.BorderStyle = BorderStyle.FixedSingle;
            panel5.Controls.Add(label3);
            panel5.Controls.Add(textBox3);
            panel5.Controls.Add(textBox4);
            panel5.Controls.Add(button2);
            panel5.Controls.Add(label4);
            panel5.Controls.Add(pictureBox2);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Margin = new Padding(2);
            panel5.Name = "panel5";
            panel5.Size = new Size(236, 167);
            panel5.TabIndex = 16;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 79);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 12;
            label3.Text = "상태메시지";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(11, 96);
            textBox3.Margin = new Padding(2);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(219, 51);
            textBox3.TabIndex = 14;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(65, 18);
            textBox4.Margin = new Padding(2);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(165, 23);
            textBox4.TabIndex = 13;
            // 
            // button2
            // 
            button2.BackgroundImageLayout = ImageLayout.Zoom;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(41, 46);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(20, 20);
            button2.TabIndex = 4;
            button2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.AppWorkspace;
            label4.Location = new Point(65, 46);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 2;
            label4.Text = "부서";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = DBP_WinformChat.Properties.Resources._default;
            pictureBox2.Location = new Point(11, 18);
            pictureBox2.Margin = new Padding(2);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(50, 48);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Tag = "default.jpg";
            // 
            // button4
            // 
            button4.Location = new Point(155, 45);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 15;
            button4.Text = "멤버 관리";
            button4.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 79);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(67, 15);
            label5.TabIndex = 12;
            label5.Text = "상태메시지";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(11, 96);
            textBox5.Margin = new Padding(2);
            textBox5.Multiline = true;
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(219, 51);
            textBox5.TabIndex = 14;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(65, 18);
            textBox6.Margin = new Padding(2);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(165, 23);
            textBox6.TabIndex = 13;
            // 
            // button5
            // 
            button5.BackgroundImageLayout = ImageLayout.Zoom;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Location = new Point(41, 46);
            button5.Margin = new Padding(2);
            button5.Name = "button5";
            button5.Size = new Size(20, 20);
            button5.TabIndex = 4;
            button5.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = SystemColors.AppWorkspace;
            label8.Location = new Point(65, 46);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(31, 15);
            label8.TabIndex = 2;
            label8.Text = "부서";
            // 
            // pictureBox3
            // 
            pictureBox3.Image = DBP_WinformChat.Properties.Resources._default;
            pictureBox3.Location = new Point(11, 18);
            pictureBox3.Margin = new Padding(2);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(50, 48);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 0;
            pictureBox3.TabStop = false;
            pictureBox3.Tag = "default.jpg";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(238, 167);
            panel1.TabIndex = 16;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 79);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(67, 15);
            label1.TabIndex = 12;
            label1.Text = "상태메시지";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(11, 96);
            textBox1.Margin = new Padding(2);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(219, 51);
            textBox1.TabIndex = 14;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(65, 18);
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
            button3.Location = new Point(41, 46);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(20, 20);
            button3.TabIndex = 4;
            button3.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.AppWorkspace;
            label2.Location = new Point(65, 46);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 2;
            label2.Text = "부서";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = DBP_WinformChat.Properties.Resources._default;
            pictureBox1.Location = new Point(11, 18);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 48);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Tag = "default.jpg";
            // 
            // button1
            // 
            button1.Location = new Point(155, 45);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 15;
            button1.Text = "멤버 관리";
            button1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(11, 79);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(67, 15);
            label6.TabIndex = 12;
            label6.Text = "상태메시지";
            // 
            // StatusBox
            // 
            StatusBox.Location = new Point(11, 96);
            StatusBox.Margin = new Padding(2);
            StatusBox.Multiline = true;
            StatusBox.Name = "StatusBox";
            StatusBox.Size = new Size(219, 51);
            StatusBox.TabIndex = 14;
            // 
            // NicknameBox
            // 
            NicknameBox.Location = new Point(65, 18);
            NicknameBox.Margin = new Padding(2);
            NicknameBox.Name = "NicknameBox";
            NicknameBox.Size = new Size(165, 23);
            NicknameBox.TabIndex = 13;
            // 
            // ChangeProfileImageButton
            // 
            ChangeProfileImageButton.BackgroundImageLayout = ImageLayout.Zoom;
            ChangeProfileImageButton.FlatAppearance.BorderSize = 0;
            ChangeProfileImageButton.FlatStyle = FlatStyle.Flat;
            ChangeProfileImageButton.Location = new Point(41, 46);
            ChangeProfileImageButton.Margin = new Padding(2);
            ChangeProfileImageButton.Name = "ChangeProfileImageButton";
            ChangeProfileImageButton.Size = new Size(20, 20);
            ChangeProfileImageButton.TabIndex = 4;
            ChangeProfileImageButton.UseVisualStyleBackColor = true;
            ChangeProfileImageButton.Click += ChangeProfileImageButton_Click;
            // 
            // Deptlabel
            // 
            Deptlabel.AutoSize = true;
            Deptlabel.ForeColor = SystemColors.AppWorkspace;
            Deptlabel.Location = new Point(65, 46);
            Deptlabel.Margin = new Padding(2, 0, 2, 0);
            Deptlabel.Name = "Deptlabel";
            Deptlabel.Size = new Size(31, 15);
            Deptlabel.TabIndex = 2;
            Deptlabel.Text = "부서";
            // 
            // ProfileImageBox
            // 
            ProfileImageBox.Image = DBP_WinformChat.Properties.Resources._default;
            ProfileImageBox.Location = new Point(11, 18);
            ProfileImageBox.Margin = new Padding(2);
            ProfileImageBox.Name = "ProfileImageBox";
            ProfileImageBox.Size = new Size(50, 48);
            ProfileImageBox.SizeMode = PictureBoxSizeMode.Zoom;
            ProfileImageBox.TabIndex = 0;
            ProfileImageBox.TabStop = false;
            ProfileImageBox.Tag = "default.jpg";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageBasic);
            tabControl1.Controls.Add(tabPageMultiProfile);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(284, 490);
            tabControl1.TabIndex = 0;
            // 
            // EditProfileForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 490);
            Controls.Add(tabControl1);
            Margin = new Padding(2);
            Name = "EditProfileForm";
            Text = "Form1";
            tabPageMultiProfile.ResumeLayout(false);
            flowProfiles.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ProfileImage).EndInit();
            tabPageBasic.ResumeLayout(false);
            panelBasic.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
            panelProfileCard.ResumeLayout(false);
            panelProfileCard.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ProfileImageBox).EndInit();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Label UserNamelabel;
        private TabPage tabPageMultiProfile;
        private FlowLayoutPanel flowProfiles;
        private Panel panel3;
        private Button AddMulProfileButton;
        private Label label7;
        private Panel panel;
        private PictureBox ProfileImage;
        private Button EditButton;
        private Label NicknameLabel;
        private TabPage tabPageBasic;
        private Panel panelBasic;
        private Button CancelButton;
        private Button SaveButton;
        private Panel panelProfileCard;
        private Label label6;
        private TextBox StatusBox;
        private TextBox NicknameBox;
        private Button ChangeProfileImageButton;
        private Label Deptlabel;
        private PictureBox ProfileImageBox;
        private TabControl tabControl1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label9;
        private Panel panel4;
        private Panel panel5;
        private Label label3;
        private TextBox textBox3;
        private TextBox textBox4;
        private Button button2;
        private Label label4;
        private PictureBox pictureBox2;
        private Button button4;
        private Label label5;
        private TextBox textBox5;
        private TextBox textBox6;
        private Button button5;
        private Label label8;
        private PictureBox pictureBox3;
        private Panel panel1;
        private Label label1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button3;
        private Label label2;
        private PictureBox pictureBox1;
        private Button button1;
        private Panel panel2;
    }
}