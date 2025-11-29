namespace 남예솔
{
	partial class chatlist
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
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(chatlist));
			panel1 = new Panel();
			btndept = new Button();
			label1 = new Label();
			lvlist = new ListView();
			columnHeaderIcon = new ColumnHeader();
			columnHeader1 = new ColumnHeader();
			columnHeader2 = new ColumnHeader();
			columnHeader3 = new ColumnHeader();
			columnHeaderMessage = new ColumnHeader();
			columnHeader4 = new ColumnHeader();
			contextMenuStrip1 = new ContextMenuStrip(components);
			addpin = new ToolStripMenuItem();
			deletepin = new ToolStripMenuItem();
			imageList1 = new ImageList(components);
			panel1.SuspendLayout();
			contextMenuStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// panel1
			// 
			panel1.BackColor = SystemColors.GradientInactiveCaption;
			panel1.Controls.Add(btndept);
			panel1.Controls.Add(label1);
			panel1.Controls.Add(lvlist);
			panel1.Location = new Point(12, 12);
			panel1.Name = "panel1";
			panel1.Size = new Size(827, 825);
			panel1.TabIndex = 1;
			// 
			// btndept
			// 
			btndept.Location = new Point(673, 22);
			btndept.Name = "btndept";
			btndept.Size = new Size(112, 34);
			btndept.TabIndex = 2;
			btndept.Text = "친구 목록";
			btndept.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("맑은 고딕", 12F);
			label1.Location = new Point(17, 22);
			label1.Name = "label1";
			label1.Size = new Size(222, 32);
			label1.TabIndex = 0;
			label1.Text = "현재 채팅중인 목록";
			// 
			// lvlist
			// 
			lvlist.Columns.AddRange(new ColumnHeader[] { columnHeaderIcon, columnHeader1, columnHeader2, columnHeader3, columnHeaderMessage, columnHeader4 });
			lvlist.ContextMenuStrip = contextMenuStrip1;
			lvlist.FullRowSelect = true;
			lvlist.GridLines = true;
			lvlist.Location = new Point(0, 77);
			lvlist.Name = "lvlist";
			lvlist.Size = new Size(827, 745);
			lvlist.SmallImageList = imageList1;
			lvlist.TabIndex = 1;
			lvlist.UseCompatibleStateImageBehavior = false;
			lvlist.View = View.Details;
			lvlist.DoubleClick += lvlist_DoubleClick;
			lvlist.MouseDown += lvlist_MouseDown;
			// 
			// columnHeaderIcon
			// 
			columnHeaderIcon.Text = "";
			columnHeaderIcon.Width = 40;
			// 
			// columnHeader1
			// 
			columnHeader1.Text = "ID";
			columnHeader1.Width = 100;
			// 
			// columnHeader2
			// 
			columnHeader2.Text = "이름";
			columnHeader2.Width = 120;
			// 
			// columnHeader3
			// 
			columnHeader3.Text = "부서";
			columnHeader3.Width = 130;
			// 
			// columnHeaderMessage
			// 
			columnHeaderMessage.Text = "최근 메시지";
			columnHeaderMessage.Width = 250;
			// 
			// columnHeader4
			// 
			columnHeader4.Text = "시간";
			columnHeader4.Width = 183;
			// 
			// contextMenuStrip1
			// 
			contextMenuStrip1.ImageScalingSize = new Size(24, 24);
			contextMenuStrip1.Items.AddRange(new ToolStripItem[] { addpin, deletepin });
			contextMenuStrip1.Name = "contextMenuStrip1";
			contextMenuStrip1.Size = new Size(163, 68);
			// 
			// addpin
			// 
			addpin.Name = "addpin";
			addpin.Size = new Size(162, 32);
			addpin.Text = "고정하기";
			addpin.Click += addpin_Click;
			// 
			// deletepin
			// 
			deletepin.Name = "deletepin";
			deletepin.Size = new Size(162, 32);
			deletepin.Text = "고정 해제";
			deletepin.Click += deletepin_Click;
			// 
			// imageList1
			// 
			imageList1.ColorDepth = ColorDepth.Depth32Bit;
			imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
			imageList1.TransparentColor = Color.Transparent;
			imageList1.Images.SetKeyName(0, "pin");
			// 
			// chatlist
			// 
			ClientSize = new Size(851, 846);
			Controls.Add(panel1);
			Name = "chatlist";
			Text = "채팅 목록";
			Load += chatlist_Load;
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			contextMenuStrip1.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private Panel panel1;
		private ListView lvlist;
		private Label label1;
		private ColumnHeader columnHeaderIcon;
		private ColumnHeader columnHeader1;
		private ColumnHeader columnHeader2;
		private ColumnHeader columnHeader3;
		private ColumnHeader columnHeaderMessage; // ★ 추가됨
		private ColumnHeader columnHeader4;
		private ImageList imageList1;
		private ContextMenuStrip contextMenuStrip1;
		private ToolStripMenuItem addpin;
		private ToolStripMenuItem deletepin;
		private Button btndept;
	}
}
