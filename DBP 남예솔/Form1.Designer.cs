namespace DBP_Chat
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			label1 = new Label();
			listView1 = new ListView();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("한컴 고딕", 11.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
			label1.Location = new Point(31, 22);
			label1.Name = "label1";
			label1.Size = new Size(102, 31);
			label1.TabIndex = 0;
			label1.Text = "대화목록";
			// 
			// listView1
			// 
			listView1.Location = new Point(75, 138);
			listView1.Name = "listView1";
			listView1.Size = new Size(545, 644);
			listView1.TabIndex = 1;
			listView1.UseCompatibleStateImageBehavior = false;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(723, 837);
			Controls.Add(listView1);
			Controls.Add(label1);
			FormBorderStyle = FormBorderStyle.None;
			Name = "Form1";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Form1";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label label1;
		private ListView listView1;
	}
}