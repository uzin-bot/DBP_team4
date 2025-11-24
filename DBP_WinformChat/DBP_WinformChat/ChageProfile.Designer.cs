namespace DBP_winformChat
{
    partial class ChageProfile
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
            tabControl1 = new TabControl();
            tabPageBasic = new TabPage();
            tabPageMultiProfile = new TabPage();
            panelBasic = new Panel();
            panelProfileCard = new Panel();
            pictureBox1 = new PictureBox();
            UserNamelabel = new Label();
            Deptlabel = new Label();
            button1 = new Button();
            panel1 = new Panel();
            label1 = new Label();
            EmailtextBox = new TextBox();
            newPWtextBox = new TextBox();
            label2 = new Label();
            camera_button = new Button();
            newNametextBox = new TextBox();
            label3 = new Label();
            newAddtextBox1 = new TextBox();
            label4 = new Label();
            departmenttextBox = new TextBox();
            label5 = new Label();
            newAddtextBox2 = new TextBox();
            panel2 = new Panel();
            savebutton = new Button();
            cancelbutton = new Button();
            newNicktextBox = new TextBox();
            label6 = new Label();
            button3 = new Button();
            panel3 = new Panel();
            label7 = new Label();
            label8 = new Label();
            multbutton = new Button();
            panel4 = new Panel();
            tabControl1.SuspendLayout();
            tabPageBasic.SuspendLayout();
            tabPageMultiProfile.SuspendLayout();
            panelBasic.SuspendLayout();
            panelProfileCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageBasic);
            tabControl1.Controls.Add(tabPageMultiProfile);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(365, 684);
            tabControl1.TabIndex = 0;
            // 
            // tabPageBasic
            // 
            tabPageBasic.Controls.Add(panelBasic);
            tabPageBasic.Location = new Point(4, 29);
            tabPageBasic.Name = "tabPageBasic";
            tabPageBasic.Padding = new Padding(3);
            tabPageBasic.Size = new Size(357, 651);
            tabPageBasic.TabIndex = 0;
            tabPageBasic.Text = "기본 정보";
            tabPageBasic.UseVisualStyleBackColor = true;
            // 
            // tabPageMultiProfile
            // 
            tabPageMultiProfile.Controls.Add(panel4);
            tabPageMultiProfile.Location = new Point(4, 29);
            tabPageMultiProfile.Name = "tabPageMultiProfile";
            tabPageMultiProfile.Padding = new Padding(3);
            tabPageMultiProfile.Size = new Size(357, 651);
            tabPageMultiProfile.TabIndex = 1;
            tabPageMultiProfile.Text = "멀티프로필";
            tabPageMultiProfile.UseVisualStyleBackColor = true;
            // 
            // panelBasic
            // 
            panelBasic.Controls.Add(panel2);
            panelBasic.Controls.Add(panel1);
            panelBasic.Controls.Add(panelProfileCard);
            panelBasic.Dock = DockStyle.Fill;
            panelBasic.Location = new Point(3, 3);
            panelBasic.Name = "panelBasic";
            panelBasic.Padding = new Padding(20);
            panelBasic.Size = new Size(351, 645);
            panelBasic.TabIndex = 0;
            // 
            // panelProfileCard
            // 
            panelProfileCard.BorderStyle = BorderStyle.FixedSingle;
            panelProfileCard.Controls.Add(button3);
            panelProfileCard.Controls.Add(button1);
            panelProfileCard.Controls.Add(Deptlabel);
            panelProfileCard.Controls.Add(UserNamelabel);
            panelProfileCard.Controls.Add(pictureBox1);
            panelProfileCard.Dock = DockStyle.Top;
            panelProfileCard.Location = new Point(20, 20);
            panelProfileCard.Name = "panelProfileCard";
            panelProfileCard.Size = new Size(311, 120);
            panelProfileCard.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(14, 24);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(64, 64);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // UserNamelabel
            // 
            UserNamelabel.AutoSize = true;
            UserNamelabel.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            UserNamelabel.Location = new Point(88, 11);
            UserNamelabel.Name = "UserNamelabel";
            UserNamelabel.Size = new Size(52, 28);
            UserNamelabel.TabIndex = 1;
            UserNamelabel.Text = "이름";
            // 
            // Deptlabel
            // 
            Deptlabel.AutoSize = true;
            Deptlabel.ForeColor = SystemColors.AppWorkspace;
            Deptlabel.Location = new Point(93, 48);
            Deptlabel.Name = "Deptlabel";
            Deptlabel.Size = new Size(39, 20);
            Deptlabel.TabIndex = 2;
            Deptlabel.Text = "부서";
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(199, 78);
            button1.Name = "button1";
            button1.Size = new Size(98, 30);
            button1.TabIndex = 3;
            button1.Text = "기본프로필";
            button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(newNicktextBox);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(newAddtextBox2);
            panel1.Controls.Add(departmenttextBox);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(newAddtextBox1);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(newNametextBox);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(camera_button);
            panel1.Controls.Add(newPWtextBox);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(EmailtextBox);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(20, 140);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 20, 0, 0);
            panel1.Size = new Size(311, 439);
            panel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 20);
            label1.Name = "label1";
            label1.Size = new Size(54, 20);
            label1.TabIndex = 0;
            label1.Text = "이메일";
            // 
            // EmailtextBox
            // 
            EmailtextBox.Location = new Point(15, 43);
            EmailtextBox.Name = "EmailtextBox";
            EmailtextBox.ReadOnly = true;
            EmailtextBox.Size = new Size(283, 27);
            EmailtextBox.TabIndex = 1;
            // 
            // newPWtextBox
            // 
            newPWtextBox.Location = new Point(15, 112);
            newPWtextBox.Name = "newPWtextBox";
            newPWtextBox.Size = new Size(201, 27);
            newPWtextBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 89);
            label2.Name = "label2";
            label2.Size = new Size(69, 20);
            label2.TabIndex = 2;
            label2.Text = "비밀번호";
            // 
            // camera_button
            // 
            camera_button.Location = new Point(231, 110);
            camera_button.Name = "camera_button";
            camera_button.Size = new Size(67, 29);
            camera_button.TabIndex = 4;
            camera_button.Text = "변경";
            camera_button.UseVisualStyleBackColor = true;
            // 
            // newNametextBox
            // 
            newNametextBox.Location = new Point(14, 172);
            newNametextBox.Name = "newNametextBox";
            newNametextBox.Size = new Size(283, 27);
            newNametextBox.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 149);
            label3.Name = "label3";
            label3.Size = new Size(39, 20);
            label3.TabIndex = 5;
            label3.Text = "이름";
            // 
            // newAddtextBox1
            // 
            newAddtextBox1.Location = new Point(15, 293);
            newAddtextBox1.Name = "newAddtextBox1";
            newAddtextBox1.Size = new Size(283, 27);
            newAddtextBox1.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 270);
            label4.Name = "label4";
            label4.Size = new Size(39, 20);
            label4.TabIndex = 7;
            label4.Text = "주소";
            // 
            // departmenttextBox
            // 
            departmenttextBox.Location = new Point(15, 388);
            departmenttextBox.Name = "departmenttextBox";
            departmenttextBox.ReadOnly = true;
            departmenttextBox.Size = new Size(283, 27);
            departmenttextBox.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 365);
            label5.Name = "label5";
            label5.Size = new Size(69, 20);
            label5.TabIndex = 9;
            label5.Text = "소속부서";
            // 
            // newAddtextBox2
            // 
            newAddtextBox2.Location = new Point(15, 326);
            newAddtextBox2.Name = "newAddtextBox2";
            newAddtextBox2.Size = new Size(283, 27);
            newAddtextBox2.TabIndex = 11;
            // 
            // panel2
            // 
            panel2.Controls.Add(cancelbutton);
            panel2.Controls.Add(savebutton);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(20, 585);
            panel2.Name = "panel2";
            panel2.Size = new Size(311, 40);
            panel2.TabIndex = 2;
            // 
            // savebutton
            // 
            savebutton.Location = new Point(15, 6);
            savebutton.Name = "savebutton";
            savebutton.Size = new Size(201, 29);
            savebutton.TabIndex = 0;
            savebutton.Text = "저장하기";
            savebutton.UseVisualStyleBackColor = true;
            // 
            // cancelbutton
            // 
            cancelbutton.Location = new Point(232, 6);
            cancelbutton.Name = "cancelbutton";
            cancelbutton.Size = new Size(66, 29);
            cancelbutton.TabIndex = 1;
            cancelbutton.Text = "취소";
            cancelbutton.UseVisualStyleBackColor = true;
            // 
            // newNicktextBox
            // 
            newNicktextBox.Location = new Point(14, 235);
            newNicktextBox.Name = "newNicktextBox";
            newNicktextBox.Size = new Size(283, 27);
            newNicktextBox.TabIndex = 13;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 212);
            label6.Name = "label6";
            label6.Size = new Size(39, 20);
            label6.TabIndex = 12;
            label6.Text = "별명";
            // 
            // button3
            // 

            button3.BackgroundImageLayout = ImageLayout.Zoom;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(76, 73);
            button3.Name = "button3";
            button3.Size = new Size(26, 26);
            button3.TabIndex = 4;
            button3.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.Controls.Add(multbutton);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(label7);
            panel3.Location = new Point(14, 12);
            panel3.Name = "panel3";
            panel3.Size = new Size(321, 96);
            panel3.TabIndex = 0;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("맑은 고딕", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label7.Location = new Point(13, 13);
            label7.Name = "label7";
            label7.Size = new Size(135, 23);
            label7.TabIndex = 0;
            label7.Text = "멀티프로필 관리";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("맑은 고딕", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label8.Location = new Point(13, 54);
            label8.Name = "label8";
            label8.Size = new Size(285, 17);
            label8.TabIndex = 1;
            label8.Text = "대화 상대마다 다른 프로필을 보여줄 수 있어요";
            // 
            // multbutton
            // 
            multbutton.Location = new Point(231, 12);
            multbutton.Name = "multbutton";
            multbutton.Size = new Size(67, 29);
            multbutton.TabIndex = 2;
            multbutton.Text = "+ 추가";
            multbutton.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            panel4.AutoScroll = true;
            panel4.Controls.Add(panel3);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(351, 645);
            panel4.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(365, 684);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Form1";
            tabControl1.ResumeLayout(false);
            tabPageBasic.ResumeLayout(false);
            tabPageMultiProfile.ResumeLayout(false);
            panelBasic.ResumeLayout(false);
            panelProfileCard.ResumeLayout(false);
            panelProfileCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPageBasic;
        private TabPage tabPageMultiProfile;
        private Panel panelBasic;
        private Panel panelProfileCard;
        private PictureBox pictureBox1;
        private Button button1;
        private Label Deptlabel;
        private Label UserNamelabel;
        private Panel panel1;
        private TextBox departmenttextBox;
        private Label label5;
        private TextBox newAddtextBox1;
        private Label label4;
        private TextBox newNametextBox;
        private Label label3;
        private Button camera_button;
        private TextBox newPWtextBox;
        private Label label2;
        private TextBox EmailtextBox;
        private Label label1;
        private Panel panel2;
        private Button cancelbutton;
        private Button savebutton;
        private TextBox newAddtextBox2;
        private TextBox newNicktextBox;
        private Label label6;
        private Button button3;
        private Panel panel3;
        private Label label8;
        private Label label7;
        private Panel panel4;
        private Button multbutton;
    }
}
