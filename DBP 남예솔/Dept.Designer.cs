namespace DBP_Chat
{
    partial class Dept
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
			tvdept = new TreeView();
			gpinfo = new GroupBox();
			btnsearch = new Button();
			cbdept = new ComboBox();
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
			gpinfo.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// tvdept
			// 
			tvdept.BackColor = SystemColors.GradientInactiveCaption;
			tvdept.LineColor = Color.LightGray;
			tvdept.Location = new Point(29, 41);
			tvdept.Name = "tvdept";
			tvdept.Size = new Size(490, 816);
			tvdept.TabIndex = 0;
			tvdept.NodeMouseDoubleClick += tvdept_NodeMouseDoubleClick;
			// 
			// gpinfo
			// 
			gpinfo.BackColor = SystemColors.Info;
			gpinfo.Controls.Add(btnsearch);
			gpinfo.Controls.Add(cbdept);
			gpinfo.Controls.Add(txtname);
			gpinfo.Controls.Add(txtID);
			gpinfo.Controls.Add(label2);
			gpinfo.Controls.Add(label3);
			gpinfo.Controls.Add(label1);
			gpinfo.ForeColor = SystemColors.Desktop;
			gpinfo.Location = new Point(542, 41);
			gpinfo.Name = "gpinfo";
			gpinfo.Size = new Size(300, 303);
			gpinfo.TabIndex = 1;
			gpinfo.TabStop = false;
			gpinfo.Text = "직원 검색";
			// 
			// btnsearch
			// 
			btnsearch.BackColor = SystemColors.GradientActiveCaption;
			btnsearch.FlatStyle = FlatStyle.Popup;
			btnsearch.Location = new Point(92, 253);
			btnsearch.Name = "btnsearch";
			btnsearch.Size = new Size(112, 34);
			btnsearch.TabIndex = 8;
			btnsearch.Text = "검색";
			btnsearch.UseVisualStyleBackColor = false;
			// 
			// cbdept
			// 
			cbdept.FormattingEnabled = true;
			cbdept.Location = new Point(101, 200);
			cbdept.Name = "cbdept";
			cbdept.Size = new Size(176, 33);
			cbdept.TabIndex = 7;
			// 
			// txtname
			// 
			txtname.Location = new Point(101, 124);
			txtname.Name = "txtname";
			txtname.Size = new Size(176, 31);
			txtname.TabIndex = 6;
			// 
			// txtID
			// 
			txtID.Location = new Point(101, 52);
			txtID.Name = "txtID";
			txtID.Size = new Size(176, 31);
			txtID.TabIndex = 5;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(27, 203);
			label2.Name = "label2";
			label2.Size = new Size(58, 25);
			label2.TabIndex = 3;
			label2.Text = "부서 :";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(27, 52);
			label3.Name = "label3";
			label3.Size = new Size(40, 25);
			label3.TabIndex = 4;
			label3.Text = "ID :";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(27, 124);
			label1.Name = "label1";
			label1.Size = new Size(58, 25);
			label1.TabIndex = 2;
			label1.Text = "이름 :";
			// 
			// panel1
			// 
			panel1.Controls.Add(btndelete);
			panel1.Controls.Add(btnadd);
			panel1.Controls.Add(lBlist);
			panel1.Controls.Add(label4);
			panel1.Location = new Point(542, 372);
			panel1.Name = "panel1";
			panel1.Size = new Size(300, 420);
			panel1.TabIndex = 2;
			// 
			// btndelete
			// 
			btndelete.BackColor = SystemColors.GradientActiveCaption;
			btndelete.FlatStyle = FlatStyle.Popup;
			btndelete.Location = new Point(165, 353);
			btndelete.Name = "btndelete";
			btndelete.Size = new Size(112, 34);
			btndelete.TabIndex = 3;
			btndelete.Text = "삭제";
			btndelete.UseVisualStyleBackColor = false;
			// 
			// btnadd
			// 
			btnadd.BackColor = SystemColors.GradientActiveCaption;
			btnadd.FlatStyle = FlatStyle.Popup;
			btnadd.Location = new Point(27, 353);
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
			lBlist.Location = new Point(21, 63);
			lBlist.Name = "lBlist";
			lBlist.Size = new Size(260, 279);
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
			btnChat.BackColor = SystemColors.GradientActiveCaption;
			btnChat.FlatStyle = FlatStyle.Popup;
			btnChat.Location = new Point(634, 823);
			btnChat.Name = "btnChat";
			btnChat.Size = new Size(112, 34);
			btnChat.TabIndex = 3;
			btnChat.Text = "채팅하기";
			btnChat.UseVisualStyleBackColor = false;
			// 
			// Dept
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.Window;
			ClientSize = new Size(873, 902);
			Controls.Add(btnChat);
			Controls.Add(panel1);
			Controls.Add(gpinfo);
			Controls.Add(tvdept);
			FormBorderStyle = FormBorderStyle.None;
			MaximizeBox = false;
			Name = "Dept";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Form1";
			gpinfo.ResumeLayout(false);
			gpinfo.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private TreeView tvdept;
		private GroupBox gpinfo;
		private Label label2;
		private Label label3;
		private Label label1;
		private ComboBox cbdept;
		private TextBox txtname;
		private TextBox txtID;
		private Button btnsearch;
		private Panel panel1;
		private ListBox lBlist;
		private Label label4;
		private Button btndelete;
		private Button btnadd;
		private Button btnChat;
	}
}
