using System.Drawing;
using System.Windows.Forms;

namespace DBP_WinformChat
{
	internal class SearchResultUIHelper
	{
		//메인 적용
		public static void Apply(Form form)
		{
			//전체 배경
			form.BackColor = Color.FromArgb(241, 243, 224);

			foreach (Control c in form.Controls)
			{
				StyleControl(c);
			}
		}

		private static void StyleControl(Control ctrl)
		{
			//Panel  
			if (ctrl is Panel pnl)
			{
				pnl.BackColor = Color.FromArgb(119, 136, 115);
			}

			//Label  
			if (ctrl is Label lbl)
			{
				lbl.BackColor = Color.FromArgb(119, 136, 115);
				lbl.ForeColor = Color.White;
			} 

			//Button
			if (ctrl is Button btn)
			{
				btn.FlatStyle = FlatStyle.Flat;
				btn.UseVisualStyleBackColor = false;

				btn.BackColor = Color.FromArgb(119, 136, 115);
				btn.ForeColor = Color.White;

				btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(119, 136, 115);
				btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(119, 136, 115);
				btn.FlatAppearance.CheckedBackColor = Color.FromArgb(119, 136, 115);
				btn.FlatAppearance.BorderColor = Color.FromArgb(119, 136, 115);

				btn.FlatAppearance.BorderSize = 0;
				btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
			}

			//ListView
			if (ctrl is ListView lv)
			{
				lv.BackColor = Color.White;
				lv.ForeColor = Color.Black;
				lv.BorderStyle = BorderStyle.FixedSingle;

				lv.FullRowSelect = true;
				lv.HideSelection = false;
			}

			//자식 컨트롤에도 동일 적용 (재귀)
			foreach (Control child in ctrl.Controls)
				StyleControl(child);
		}
	}
}
