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
            gpinfo.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tvdept
            // 
            tvdept.BackColor = SystemColors.GradientInactiveCaption;
            tvdept.Location = new Point(21, 22);
            tvdept.Margin = new Padding(3, 2, 3, 2);
            tvdept.Name = "tvdept";
            tvdept.Size = new Size(441, 654);
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
            gpinfo.Location = new Point(488, 33);
            gpinfo.Margin = new Padding(3, 2, 3, 2);
            gpinfo.Name = "gpinfo";
            gpinfo.Padding = new Padding(3, 2, 3, 2);
            gpinfo.Size = new Size(270, 278);
            gpinfo.TabIndex = 1;
            gpinfo.TabStop = false;
            gpinfo.Text = "직원 검색";
            // 
            // btnsearch
            // 
            btnsearch.BackColor = SystemColors.GradientActiveCaption;
            btnsearch.FlatStyle = FlatStyle.Popup;
            btnsearch.Location = new Point(83, 226);
            btnsearch.Margin = new Padding(3, 2, 3, 2);
            btnsearch.Name = "btnsearch";
            btnsearch.Size = new Size(101, 27);
            btnsearch.TabIndex = 8;
            btnsearch.Text = "검색";
            btnsearch.UseVisualStyleBackColor = false;
            // 
            // txtdept
            // 
            txtdept.Location = new Point(91, 170);
            txtdept.Margin = new Padding(3, 2, 3, 2);
            txtdept.Name = "txtdept";
            txtdept.Size = new Size(159, 27);
            txtdept.TabIndex = 7;
            // 
            // txtname
            // 
            txtname.Location = new Point(91, 110);
            txtname.Margin = new Padding(3, 2, 3, 2);
            txtname.Name = "txtname";
            txtname.Size = new Size(159, 27);
            txtname.TabIndex = 6;
            // 
            // txtID
            // 
            txtID.Location = new Point(91, 52);
            txtID.Margin = new Padding(3, 2, 3, 2);
            txtID.Name = "txtID";
            txtID.Size = new Size(159, 27);
            txtID.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 173);
            label2.Name = "label2";
            label2.Size = new Size(47, 20);
            label2.TabIndex = 3;
            label2.Text = "부서 :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 52);
            label3.Name = "label3";
            label3.Size = new Size(32, 20);
            label3.TabIndex = 4;
            label3.Text = "ID :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 110);
            label1.Name = "label1";
            label1.Size = new Size(47, 20);
            label1.TabIndex = 2;
            label1.Text = "이름 :";
            // 
            // panel1
            // 
            panel1.Controls.Add(btndelete);
            panel1.Controls.Add(btnadd);
            panel1.Controls.Add(lBlist);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(488, 340);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(270, 336);
            panel1.TabIndex = 2;
            // 
            // btndelete
            // 
            btndelete.BackColor = SystemColors.GradientActiveCaption;
            btndelete.FlatStyle = FlatStyle.Popup;
            btndelete.Location = new Point(148, 282);
            btndelete.Margin = new Padding(3, 2, 3, 2);
            btndelete.Name = "btndelete";
            btndelete.Size = new Size(101, 27);
            btndelete.TabIndex = 3;
            btndelete.Text = "삭제";
            btndelete.UseVisualStyleBackColor = false;
            // 
            // btnadd
            // 
            btnadd.BackColor = SystemColors.GradientActiveCaption;
            btnadd.FlatStyle = FlatStyle.Popup;
            btnadd.Location = new Point(24, 282);
            btnadd.Margin = new Padding(3, 2, 3, 2);
            btnadd.Name = "btnadd";
            btnadd.Size = new Size(101, 27);
            btnadd.TabIndex = 2;
            btnadd.Text = "추가";
            btnadd.UseVisualStyleBackColor = false;
            // 
            // lBlist
            // 
            lBlist.FormattingEnabled = true;
            lBlist.Location = new Point(19, 50);
            lBlist.Margin = new Padding(3, 2, 3, 2);
            lBlist.Name = "lBlist";
            lBlist.Size = new Size(234, 224);
            lBlist.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 12);
            label4.Name = "label4";
            label4.Size = new Size(69, 20);
            label4.TabIndex = 0;
            label4.Text = "즐겨찾기";
            // 
            // btnChat
            // 
            btnChat.BackColor = SystemColors.GradientActiveCaption;
            btnChat.FlatStyle = FlatStyle.Popup;
            btnChat.Location = new Point(571, 701);
            btnChat.Margin = new Padding(3, 2, 3, 2);
            btnChat.Name = "btnChat";
            btnChat.Size = new Size(101, 27);
            btnChat.TabIndex = 3;
            btnChat.Text = "채팅하기";
            btnChat.UseVisualStyleBackColor = false;
            // 
            // btnchatlist
            // 
            btnchatlist.Location = new Point(347, 701);
            btnchatlist.Margin = new Padding(3, 2, 3, 2);
            btnchatlist.Name = "btnchatlist";
            btnchatlist.Size = new Size(101, 27);
            btnchatlist.TabIndex = 4;
            btnchatlist.Text = "채팅 목록";
            btnchatlist.UseVisualStyleBackColor = true;
            // 
            // change_profile_button
            // 
            change_profile_button.Location = new Point(174, 701);
            change_profile_button.Name = "change_profile_button";
            change_profile_button.Size = new Size(121, 29);
            change_profile_button.TabIndex = 5;
            change_profile_button.Text = "프로필 변경";
            change_profile_button.UseVisualStyleBackColor = true;
            change_profile_button.Click += change_profile_button_Click;
            // 
            // logout_button
            // 
            logout_button.Location = new Point(21, 701);
            logout_button.Name = "logout_button";
            logout_button.Size = new Size(94, 29);
            logout_button.TabIndex = 6;
            logout_button.Text = "로그아웃";
            logout_button.UseVisualStyleBackColor = true;
            logout_button.Click += logout_button_Click;
            // 
            // Dept
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(786, 754);
            Controls.Add(logout_button);
            Controls.Add(change_profile_button);
            Controls.Add(btnchatlist);
            Controls.Add(btnChat);
            Controls.Add(panel1);
            Controls.Add(gpinfo);
            Controls.Add(tvdept);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Dept";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dept";
            gpinfo.ResumeLayout(false);
            gpinfo.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

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
    }
}
