namespace DBP_Chat
{
	partial class SearchResultForm
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
			label1 = new Label();
			lvResult = new ListView();
			colId = new ColumnHeader();
			colName = new ColumnHeader();
			colDept = new ColumnHeader();
			colTeam = new ColumnHeader();
			panel1 = new Panel();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.BackColor = Color.FromArgb(119, 136, 115);
			label1.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
			label1.ForeColor = Color.White;
			label1.Location = new Point(32, 26);
			label1.Name = "label1";
			label1.Size = new Size(161, 30);
			label1.TabIndex = 0;
			label1.Text = "직원 검색 결과";
			// 
			// lvResult
			// 
			lvResult.Columns.AddRange(new ColumnHeader[] { colId, colName, colDept, colTeam });
			lvResult.FullRowSelect = true;
			lvResult.Location = new Point(-1, 79);
			lvResult.Name = "lvResult";
			lvResult.Size = new Size(645, 315);
			lvResult.TabIndex = 1;
			lvResult.UseCompatibleStateImageBehavior = false;
			lvResult.View = View.Details;
			// 
			// colId
			// 
			colId.Text = "ID";
			colId.Width = 120;
			// 
			// colName
			// 
			colName.Text = "이름";
			colName.Width = 130;
			// 
			// colDept
			// 
			colDept.Text = "부서";
			colDept.Width = 180;
			// 
			// colTeam
			// 
			colTeam.Text = "팀";
			colTeam.Width = 120;
			// 
			// panel1
			// 
			panel1.BackColor = Color.FromArgb(119, 136, 115);
			panel1.Controls.Add(label1);
			panel1.Location = new Point(-4, -1);
			panel1.Name = "panel1";
			panel1.Size = new Size(723, 81);
			panel1.TabIndex = 2;
			// 
			// SearchResultForm
			// 
			BackColor = Color.FromArgb(241, 243, 224);
			ClientSize = new Size(643, 393);
			Controls.Add(lvResult);
			Controls.Add(panel1);
			Name = "SearchResultForm";
			Text = "직원검색결과";
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private Label label1;
		private ListView lvResult;
		private ColumnHeader colId;
		private ColumnHeader colName;
		private ColumnHeader colDept;
		private ColumnHeader colTeam;
		private Panel panel1;
	}
}
