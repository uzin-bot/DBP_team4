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
			colStatus = new ColumnHeader();
			btnAddFavorite = new Button();
			btnClose = new Button();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(34, 28);
			label1.Name = "label1";
			label1.Size = new Size(132, 25);
			label1.TabIndex = 3;
			label1.Text = "직원 검색 결과";
			// 
			// lvResult
			// 
			lvResult.CheckBoxes = true;
			lvResult.Columns.AddRange(new ColumnHeader[] { colId, colName, colDept, colStatus });
			lvResult.FullRowSelect = true;
			lvResult.Location = new Point(34, 80);
			lvResult.Name = "lvResult";
			lvResult.Size = new Size(650, 260);
			lvResult.TabIndex = 2;
			lvResult.UseCompatibleStateImageBehavior = false;
			lvResult.View = View.Details;
			lvResult.DoubleClick += lvResult_DoubleClick;
			// 
			// colId
			// 
			colId.Text = "ID";
			colId.Width = 120;
			// 
			// colName
			// 
			colName.Text = "이름";
			colName.Width = 150;
			// 
			// colDept
			// 
			colDept.Text = "부서";
			colDept.Width = 200;
			// 
			// colStatus
			// 
			colStatus.Text = "상태";
			colStatus.Width = 180;
			// 
			// btnAddFavorite
			// 
			btnAddFavorite.Location = new Point(120, 370);
			btnAddFavorite.Name = "btnAddFavorite";
			btnAddFavorite.Size = new Size(160, 40);
			btnAddFavorite.TabIndex = 1;
			btnAddFavorite.Text = "즐겨찾기 추가";
			btnAddFavorite.Click += btnAddFavorite_Click;
			// 
			// btnClose
			// 
			btnClose.Location = new Point(420, 370);
			btnClose.Name = "btnClose";
			btnClose.Size = new Size(160, 40);
			btnClose.TabIndex = 0;
			btnClose.Text = "닫기";
			btnClose.Click += btnClose_Click;
			// 
			// SearchResultForm
			// 
			ClientSize = new Size(720, 450);
			Controls.Add(btnClose);
			Controls.Add(btnAddFavorite);
			Controls.Add(lvResult);
			Controls.Add(label1);
			Name = "SearchResultForm";
			Text = "직원 검색 결과";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView lvResult;
		private System.Windows.Forms.ColumnHeader colId;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colDept;
		private System.Windows.Forms.ColumnHeader colStatus;
		private System.Windows.Forms.Button btnAddFavorite;
		private System.Windows.Forms.Button btnClose;
	}
}
