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

		private Panel panelMain;
		private Panel panelHeader;
		private Button btndept;
		private Label label1;
		private ListView lvlist;
		private ColumnHeader columnHeaderIcon;
		private ColumnHeader columnHeader1;
		private ColumnHeader columnHeader2;
		private ColumnHeader columnHeader3;
		private ColumnHeader columnHeaderMessage;
		private ColumnHeader columnHeader4;
		private ImageList imageList1;
		private ContextMenuStrip contextMenuStrip1;
		private ToolStripMenuItem addpin;
		private ToolStripMenuItem deletepin;

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(chatlist));
			panelMain = new Panel();
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
			panelHeader = new Panel();
			btndept = new Button();
			label1 = new Label();
			panelMain.SuspendLayout();
			contextMenuStrip1.SuspendLayout();
			panelHeader.SuspendLayout();
			SuspendLayout();
			// 
			// panelMain
			// 
			panelMain.Controls.Add(lvlist);
			panelMain.Controls.Add(panelHeader);
			panelMain.Dock = DockStyle.Fill;
			panelMain.Location = new Point(0, 0);
			panelMain.Name = "panelMain";
			panelMain.Size = new Size(1049, 850);
			panelMain.TabIndex = 1;
			// 
			// lvlist
			// 
			lvlist.Columns.AddRange(new ColumnHeader[] { columnHeaderIcon, columnHeader1, columnHeader2, columnHeader3, columnHeaderMessage, columnHeader4 });
			lvlist.ContextMenuStrip = contextMenuStrip1;
			lvlist.Dock = DockStyle.Fill;
			lvlist.FullRowSelect = true;
			lvlist.Location = new Point(0, 90);
			lvlist.Name = "lvlist";
			lvlist.Size = new Size(1049, 760);
			lvlist.SmallImageList = imageList1;
			lvlist.TabIndex = 0;
			lvlist.UseCompatibleStateImageBehavior = false;
			lvlist.View = View.Details;
			// 
			// columnHeaderIcon
			// 
			columnHeaderIcon.Text = "";
			columnHeaderIcon.Width = 50;
			// 
			// columnHeader1
			// 
			columnHeader1.Text = "ID";
			columnHeader1.Width = 100;
			// 
			// columnHeader2
			// 
			columnHeader2.Text = "이름";
			columnHeader2.Width = 110;
			// 
			// columnHeader3
			// 
			columnHeader3.Text = "부서";
			columnHeader3.Width = 130;
			// 
			// columnHeaderMessage
			// 
			columnHeaderMessage.Text = "최근 메시지";
			columnHeaderMessage.Width = 390;
			// 
			// columnHeader4
			// 
			columnHeader4.Text = "시간";
			columnHeader4.Width = 265;
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
			imageList1.Images.SetKeyName(0, "imageList1");
			// 
			// panelHeader
			// 
			panelHeader.BackColor = Color.FromArgb(119, 136, 115);
			panelHeader.Controls.Add(btndept);
			panelHeader.Controls.Add(label1);
			panelHeader.Dock = DockStyle.Top;
			panelHeader.Location = new Point(0, 0);
			panelHeader.Name = "panelHeader";
			panelHeader.Size = new Size(1049, 90);
			panelHeader.TabIndex = 1;
			// 
			// btndept
			// 
			btndept.BackColor = Color.FromArgb(241, 243, 224);
			btndept.FlatStyle = FlatStyle.Flat;
			btndept.ForeColor = Color.FromArgb(119, 136, 115);
			btndept.Location = new Point(864, 23);
			btndept.Name = "btndept";
			btndept.Size = new Size(144, 35);
			btndept.TabIndex = 0;
			btndept.Text = "친구 목록";
			btndept.UseVisualStyleBackColor = false;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
			label1.ForeColor = Color.White;
			label1.Location = new Point(28, 28);
			label1.Name = "label1";
			label1.Size = new Size(222, 32);
			label1.TabIndex = 1;
			label1.Text = "현재 채팅중인 목록";
			// 
			// chatlist
			// 
			ClientSize = new Size(1049, 850);
			Controls.Add(panelMain);
			Name = "chatlist";
			Text = "채팅 목록";
			Load += chatlist_Load;
			panelMain.ResumeLayout(false);
			contextMenuStrip1.ResumeLayout(false);
			panelHeader.ResumeLayout(false);
			panelHeader.PerformLayout();
			ResumeLayout(false);
		}

		#endregion
	}
}
