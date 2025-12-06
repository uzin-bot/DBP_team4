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
			headerPanel = new Panel();
			headerLabel = new Label();
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
			label5 = new Label();
			headerPanel.SuspendLayout();
			gpinfo.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// headerPanel
			// 
			headerPanel.BackColor = Color.FromArgb(119, 136, 115);
			headerPanel.Controls.Add(headerLabel);
			headerPanel.Dock = DockStyle.Top;
			headerPanel.Location = new Point(0, 0);
			headerPanel.Margin = new Padding(3, 4, 3, 4);
			headerPanel.Name = "headerPanel";
			headerPanel.Padding = new Padding(17, 0, 0, 0);
			headerPanel.Size = new Size(1185, 88);
			headerPanel.TabIndex = 0;
			// 
			// headerLabel
			// 
			headerLabel.AutoSize = true;
			headerLabel.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
			headerLabel.ForeColor = Color.White;
			headerLabel.Location = new Point(23, 24);
			headerLabel.Name = "headerLabel";
			headerLabel.Size = new Size(139, 38);
			headerLabel.TabIndex = 0;
			headerLabel.Text = "친구 목록";
			// 
			// tvdept
			// 
			tvdept.BackColor = Color.FromArgb(241, 243, 224);
			tvdept.Location = new Point(23, 144);
			tvdept.Margin = new Padding(3, 2, 3, 2);
			tvdept.Name = "tvdept";
			tvdept.Size = new Size(490, 734);
			tvdept.TabIndex = 0;
			// 
			// gpinfo
			// 
			gpinfo.BackColor = Color.FromArgb(241, 243, 224);
			gpinfo.Controls.Add(btnsearch);
			gpinfo.Controls.Add(txtdept);
			gpinfo.Controls.Add(txtname);
			gpinfo.Controls.Add(txtID);
			gpinfo.Controls.Add(label2);
			gpinfo.Controls.Add(label3);
			gpinfo.Controls.Add(label1);
			gpinfo.ForeColor = SystemColors.Desktop;
			gpinfo.Location = new Point(542, 113);
			gpinfo.Margin = new Padding(3, 2, 3, 2);
			gpinfo.Name = "gpinfo";
			gpinfo.Padding = new Padding(3, 2, 3, 2);
			gpinfo.Size = new Size(610, 348);
			gpinfo.TabIndex = 1;
			gpinfo.TabStop = false;
			gpinfo.Text = "직원 검색";
			// 
			// btnsearch
			// 
			btnsearch.BackColor = Color.FromArgb(161, 188, 152);
			btnsearch.FlatStyle = FlatStyle.Flat;
			btnsearch.ForeColor = Color.White;
			btnsearch.Location = new Point(417, 286);
			btnsearch.Margin = new Padding(3, 2, 3, 2);
			btnsearch.Name = "btnsearch";
			btnsearch.Size = new Size(112, 34);
			btnsearch.TabIndex = 8;
			btnsearch.Text = "검색";
			btnsearch.UseVisualStyleBackColor = false;
			// 
			// txtdept
			// 
			txtdept.Location = new Point(174, 220);
			txtdept.Margin = new Padding(3, 2, 3, 2);
			txtdept.Name = "txtdept";
			txtdept.Size = new Size(382, 31);
			txtdept.TabIndex = 7;
			// 
			// txtname
			// 
			txtname.Location = new Point(174, 146);
			txtname.Margin = new Padding(3, 2, 3, 2);
			txtname.Name = "txtname";
			txtname.Size = new Size(382, 31);
			txtname.TabIndex = 6;
			// 
			// txtID
			// 
			txtID.Location = new Point(174, 73);
			txtID.Margin = new Padding(3, 2, 3, 2);
			txtID.Name = "txtID";
			txtID.Size = new Size(382, 31);
			txtID.TabIndex = 5;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(65, 222);
			label2.Name = "label2";
			label2.Size = new Size(58, 25);
			label2.TabIndex = 3;
			label2.Text = "부서 :";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(65, 71);
			label3.Name = "label3";
			label3.Size = new Size(40, 25);
			label3.TabIndex = 4;
			label3.Text = "ID :";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(65, 144);
			label1.Name = "label1";
			label1.Size = new Size(58, 25);
			label1.TabIndex = 2;
			label1.Text = "이름 :";
			// 
			// panel1
			// 
			panel1.BackColor = Color.FromArgb(241, 243, 224);
			panel1.Controls.Add(btndelete);
			panel1.Controls.Add(btnadd);
			panel1.Controls.Add(lBlist);
			panel1.Controls.Add(label4);
			panel1.Location = new Point(542, 498);
			panel1.Margin = new Padding(3, 2, 3, 2);
			panel1.Name = "panel1";
			panel1.Size = new Size(610, 384);
			panel1.TabIndex = 2;
			// 
			// btndelete
			// 
			btndelete.BackColor = Color.FromArgb(161, 188, 152);
			btndelete.FlatStyle = FlatStyle.Flat;
			btndelete.ForeColor = Color.White;
			btndelete.Location = new Point(363, 320);
			btndelete.Margin = new Padding(3, 2, 3, 2);
			btndelete.Name = "btndelete";
			btndelete.Size = new Size(112, 34);
			btndelete.TabIndex = 3;
			btndelete.Text = "삭제";
			btndelete.UseVisualStyleBackColor = false;
			// 
			// btnadd
			// 
			btnadd.BackColor = Color.FromArgb(161, 188, 152);
			btnadd.FlatStyle = FlatStyle.Flat;
			btnadd.ForeColor = Color.White;
			btnadd.Location = new Point(126, 320);
			btnadd.Margin = new Padding(3, 2, 3, 2);
			btnadd.Name = "btnadd";
			btnadd.Size = new Size(112, 34);
			btnadd.TabIndex = 2;
			btnadd.Text = "추가";
			btnadd.UseVisualStyleBackColor = false;
			// 
			// lBlist
			// 
			lBlist.FormattingEnabled = true;
			lBlist.ItemHeight = 25;
			lBlist.Location = new Point(21, 62);
			lBlist.Margin = new Padding(3, 2, 3, 2);
			lBlist.Name = "lBlist";
			lBlist.Size = new Size(564, 229);
			lBlist.TabIndex = 1;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(17, 15);
			label4.Name = "label4";
			label4.Size = new Size(84, 25);
			label4.TabIndex = 0;
			label4.Text = "즐겨찾기";
			// 
			// btnChat
			// 
			btnChat.BackColor = Color.FromArgb(161, 188, 152);
			btnChat.FlatStyle = FlatStyle.Flat;
			btnChat.ForeColor = Color.White;
			btnChat.Location = new Point(1030, 904);
			btnChat.Margin = new Padding(3, 2, 3, 2);
			btnChat.Name = "btnChat";
			btnChat.Size = new Size(112, 34);
			btnChat.TabIndex = 3;
			btnChat.Text = "채팅하기";
			btnChat.UseVisualStyleBackColor = false;
			// 
			// btnchatlist
			// 
			btnchatlist.BackColor = Color.FromArgb(161, 188, 152);
			btnchatlist.FlatStyle = FlatStyle.Flat;
			btnchatlist.ForeColor = Color.White;
			btnchatlist.Location = new Point(165, 905);
			btnchatlist.Margin = new Padding(3, 2, 3, 2);
			btnchatlist.Name = "btnchatlist";
			btnchatlist.Size = new Size(112, 34);
			btnchatlist.TabIndex = 4;
			btnchatlist.Text = "채팅 목록";
			btnchatlist.UseVisualStyleBackColor = false;
			// 
			// change_profile_button
			// 
			change_profile_button.BackColor = Color.FromArgb(161, 188, 152);
			change_profile_button.FlatStyle = FlatStyle.Flat;
			change_profile_button.ForeColor = Color.White;
			change_profile_button.Location = new Point(354, 904);
			change_profile_button.Margin = new Padding(3, 4, 3, 4);
			change_profile_button.Name = "change_profile_button";
			change_profile_button.Size = new Size(134, 36);
			change_profile_button.TabIndex = 5;
			change_profile_button.Text = "프로필 변경";
			change_profile_button.UseVisualStyleBackColor = false;
			change_profile_button.Click += change_profile_button_Click;
			// 
			// logout_button
			// 
			logout_button.BackColor = Color.FromArgb(161, 188, 152);
			logout_button.FlatStyle = FlatStyle.Flat;
			logout_button.ForeColor = Color.White;
			logout_button.Location = new Point(23, 905);
			logout_button.Margin = new Padding(3, 4, 3, 4);
			logout_button.Name = "logout_button";
			logout_button.Size = new Size(104, 36);
			logout_button.TabIndex = 6;
			logout_button.Text = "로그아웃";
			logout_button.UseVisualStyleBackColor = false;
			logout_button.Click += logout_button_Click;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(23, 103);
			label5.Name = "label5";
			label5.Size = new Size(66, 25);
			label5.TabIndex = 7;
			label5.Text = "조직도";
			// 
			// Dept
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.Window;
			ClientSize = new Size(1185, 961);
			Controls.Add(label5);
			Controls.Add(headerPanel);
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
			headerPanel.ResumeLayout(false);
			headerPanel.PerformLayout();
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
		private Panel headerPanel;
		private Label headerLabel;
		private Label label5;
	}
}
