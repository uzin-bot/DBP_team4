namespace DBP_Chat
{
	partial class Dept
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
            tvdept = new TreeView();
            gpinfo = new GroupBox();
            btnsearch = new Button();
            txtdept = new TextBox();
            txtname = new TextBox();
            txtID = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label1 = new Label();
            panel1 = new Panel();
            btndelete = new Button();
            btnadd = new Button();
            lBlist = new ListBox();
            label4 = new Label();
            btnChat = new Button();
            btnchatlist = new Button();
            change_profile_button = new Button();
            logout_button = new Button();
            radioButton1 = new RadioButton();
            gpinfo.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tvdept
            // 
            tvdept.BackColor = SystemColors.GradientInactiveCaption;
            tvdept.Location = new Point(16, 16);
            tvdept.Margin = new Padding(2);
            tvdept.Name = "tvdept";
            tvdept.Size = new Size(344, 492);
            tvdept.TabIndex = 0;
            // 
            // gpinfo
            // 
            gpinfo.BackColor = SystemColors.Info;
            gpinfo.Controls.Add(btnsearch);
            gpinfo.Controls.Add(txtdept);
            gpinfo.Controls.Add(txtname);
            gpinfo.Controls.Add(txtID);
            gpinfo.Controls.Add(label2);
            gpinfo.Controls.Add(label3);
            gpinfo.Controls.Add(label1);
            gpinfo.ForeColor = SystemColors.Desktop;
            gpinfo.Location = new Point(380, 25);
            gpinfo.Margin = new Padding(2);
            gpinfo.Name = "gpinfo";
            gpinfo.Padding = new Padding(2);
            gpinfo.Size = new Size(210, 208);
            gpinfo.TabIndex = 1;
            gpinfo.TabStop = false;
            gpinfo.Text = "직원 검색";
            // 
            // btnsearch
            // 
            btnsearch.BackColor = SystemColors.GradientActiveCaption;
            btnsearch.FlatStyle = FlatStyle.Popup;
            btnsearch.Location = new Point(65, 170);
            btnsearch.Margin = new Padding(2);
            btnsearch.Name = "btnsearch";
            btnsearch.Size = new Size(79, 20);
            btnsearch.TabIndex = 8;
            btnsearch.Text = "검색";
            btnsearch.UseVisualStyleBackColor = false;
            // 
            // txtdept
            // 
            txtdept.Location = new Point(71, 128);
            txtdept.Margin = new Padding(2);
            txtdept.Name = "txtdept";
            txtdept.Size = new Size(125, 23);
            txtdept.TabIndex = 7;
            // 
            // txtname
            // 
            txtname.Location = new Point(71, 82);
            txtname.Margin = new Padding(2);
            txtname.Name = "txtname";
            txtname.Size = new Size(125, 23);
            txtname.TabIndex = 6;
            // 
            // txtID
            // 
            txtID.Location = new Point(71, 39);
            txtID.Margin = new Padding(2);
            txtID.Name = "txtID";
            txtID.Size = new Size(125, 23);
            txtID.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 130);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 3;
            label2.Text = "부서 :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(19, 39);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(26, 15);
            label3.TabIndex = 4;
            label3.Text = "ID :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 82);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 2;
            label1.Text = "이름 :";
            // 
            // panel1
            // 
            panel1.Controls.Add(btndelete);
            panel1.Controls.Add(btnadd);
            panel1.Controls.Add(lBlist);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(380, 255);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(210, 252);
            panel1.TabIndex = 2;
            // 
            // btndelete
            // 
            btndelete.BackColor = SystemColors.GradientActiveCaption;
            btndelete.FlatStyle = FlatStyle.Popup;
            btndelete.Location = new Point(115, 212);
            btndelete.Margin = new Padding(2);
            btndelete.Name = "btndelete";
            btndelete.Size = new Size(79, 20);
            btndelete.TabIndex = 3;
            btndelete.Text = "삭제";
            btndelete.UseVisualStyleBackColor = false;
            // 
            // btnadd
            // 
            btnadd.BackColor = SystemColors.GradientActiveCaption;
            btnadd.FlatStyle = FlatStyle.Popup;
            btnadd.Location = new Point(19, 212);
            btnadd.Margin = new Padding(2);
            btnadd.Name = "btnadd";
            btnadd.Size = new Size(79, 20);
            btnadd.TabIndex = 2;
            btnadd.Text = "추가";
            btnadd.UseVisualStyleBackColor = false;
            // 
            // lBlist
            // 
            lBlist.FormattingEnabled = true;
            lBlist.ItemHeight = 15;
            lBlist.Location = new Point(15, 38);
            lBlist.Margin = new Padding(2);
            lBlist.Name = "lBlist";
            lBlist.Size = new Size(183, 169);
            lBlist.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 9);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(55, 15);
            label4.TabIndex = 0;
            label4.Text = "즐겨찾기";
            // 
            // btnChat
            // 
            btnChat.BackColor = SystemColors.GradientActiveCaption;
            btnChat.FlatStyle = FlatStyle.Popup;
            btnChat.Location = new Point(444, 526);
            btnChat.Margin = new Padding(2);
            btnChat.Name = "btnChat";
            btnChat.Size = new Size(79, 20);
            btnChat.TabIndex = 3;
            btnChat.Text = "채팅하기";
            btnChat.UseVisualStyleBackColor = false;
            // 
            // btnchatlist
            // 
            btnchatlist.Location = new Point(270, 526);
            btnchatlist.Margin = new Padding(2);
            btnchatlist.Name = "btnchatlist";
            btnchatlist.Size = new Size(79, 20);
            btnchatlist.TabIndex = 4;
            btnchatlist.Text = "채팅 목록";
            btnchatlist.UseVisualStyleBackColor = true;
            // 
            // change_profile_button
            // 
            change_profile_button.Location = new Point(135, 526);
            change_profile_button.Margin = new Padding(2);
            change_profile_button.Name = "change_profile_button";
            change_profile_button.Size = new Size(94, 22);
            change_profile_button.TabIndex = 5;
            change_profile_button.Text = "프로필 변경";
            change_profile_button.UseVisualStyleBackColor = true;
            change_profile_button.Click += change_profile_button_Click;
            // 
            // logout_button
            // 
            logout_button.Location = new Point(16, 526);
            logout_button.Margin = new Padding(2);
            logout_button.Name = "logout_button";
            logout_button.Size = new Size(73, 22);
            logout_button.TabIndex = 6;
            logout_button.Text = "로그아웃";
            logout_button.UseVisualStyleBackColor = true;
            logout_button.Click += logout_button_Click;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(380, 1);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(95, 19);
            radioButton1.TabIndex = 7;
            radioButton1.TabStop = true;
            radioButton1.Text = "radioButton1";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // Dept
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(611, 566);
            Controls.Add(radioButton1);
            Controls.Add(logout_button);
            Controls.Add(change_profile_button);
            Controls.Add(btnchatlist);
            Controls.Add(btnChat);
            Controls.Add(panel1);
            Controls.Add(gpinfo);
            Controls.Add(tvdept);
            Margin = new Padding(2);
            Name = "Dept";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dept";
            gpinfo.ResumeLayout(false);
            gpinfo.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvdept;
		private System.Windows.Forms.GroupBox gpinfo;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtdept;
		private System.Windows.Forms.TextBox txtname;
		private System.Windows.Forms.TextBox txtID;
		private System.Windows.Forms.Button btnsearch;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ListBox lBlist;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btndelete;
		private System.Windows.Forms.Button btnadd;
		private System.Windows.Forms.Button btnChat;
		private Button btnchatlist;
        private Button change_profile_button;
        private Button logout_button;
        private RadioButton radioButton1;
    }
}
