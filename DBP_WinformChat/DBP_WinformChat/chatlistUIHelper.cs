using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DBP_WinformChat
{
	internal class chatlistUIHelper
	{

		public static void Apply(Form form)
		{
			form.BackColor = Color.White;
			form.Font = new Font("맑은 고딕", 10);

			foreach (Control c in form.Controls)
				StyleControl(c);
		}

		private static void StyleControl(Control ctrl)
		{
			// ========== 상단 헤더(Label) ==========
			if (ctrl is Label lbl && lbl.Name == "label1")
			{
				lbl.BackColor = Color.FromArgb(119, 136, 115);
				lbl.ForeColor = Color.White;
				lbl.Font = new Font("맑은 고딕", 13, FontStyle.Bold);
				lbl.AutoSize = false;
				lbl.Dock = DockStyle.Top;
				lbl.Height = 80;
				lbl.TextAlign = ContentAlignment.MiddleLeft;
				lbl.Padding = new Padding(15, 0, 0, 0);
			}

			// ========== 패널 ==========
			if (ctrl is Panel pnl)
			{
				pnl.BackColor = Color.White;
			}

			// ========== ListView ==========
			if (ctrl is ListView lv)
			{
				lv.OwnerDraw = true;
				lv.BorderStyle = BorderStyle.None;
				lv.FullRowSelect = true;
				lv.HideSelection = false;

				lv.BackColor = Color.White;
				lv.ForeColor = Color.FromArgb(119, 136, 115);

				// --- 컬럼 헤더 ---
				lv.DrawColumnHeader += (s, e) =>
				{
					using (SolidBrush br = new SolidBrush(Color.White))
					{
						e.Graphics.FillRectangle(br, e.Bounds);
					}

					TextRenderer.DrawText(
						e.Graphics,
						e.Header.Text,
						new Font("맑은 고딕", 10, FontStyle.Bold),
						e.Bounds,
						Color.FromArgb(119, 136, 115),
					TextFormatFlags.VerticalCenter | TextFormatFlags.Left
					);
				};

				// --- 행 배경 ---
				lv.DrawItem += (s, e) =>
				{
					e.DrawDefault = false;

					Color bg = e.Item.Selected ? Color.FromArgb(210, 220, 182) : Color.FromArgb(241, 243, 224);

					using (SolidBrush br = new SolidBrush(bg))
					{
						e.Graphics.FillRectangle(br, e.Bounds);
					}
				};

				// --- 행 텍스트 & 아이콘 ---
				lv.DrawSubItem += (s, e) =>
				{
					//첫 번째 컬럼 = 아이콘 컬럼
					if (e.ColumnIndex == 0)
					{
						var item = e.Item;

						if (item.ImageIndex >= 0 && lv.SmallImageList != null)
						{
							Image img = lv.SmallImageList.Images[item.ImageIndex];
							if (img != null)
							{
								int x = e.Bounds.X + 8;
								int y = e.Bounds.Y + (e.Bounds.Height - img.Height) / 2;

								e.Graphics.DrawImage(img, x, y, img.Width, img.Height);
							}
						}
						return;  
					}

					//기본 텍스트 출력
					TextRenderer.DrawText(
						e.Graphics,
						e.SubItem.Text,
						new Font("맑은 고딕", 10),
						e.Bounds,
						Color.FromArgb(119, 136, 115),
						TextFormatFlags.VerticalCenter | TextFormatFlags.Left
					);
				};
			}

			// ========== 버튼 ==========
			if (ctrl is Button btn)
			{
				btn.FlatStyle = FlatStyle.Flat;
				btn.FlatAppearance.BorderSize = 0;

				btn.BackColor = Color.FromArgb(241, 243, 224);
				btn.ForeColor = Color.FromArgb(119, 136, 115);

				btn.Font = new Font("맑은 고딕", 9.5f, FontStyle.Bold);
				btn.Cursor = Cursors.Hand;

				btn.MouseEnter += (s, e) =>
				{
					btn.BackColor = Color.FromArgb(210, 220, 182);
				};
				btn.MouseLeave += (s, e) =>
				{
					btn.BackColor = Color.FromArgb(241, 243, 224); 
				};
			}

			foreach (Control child in ctrl.Controls)
				StyleControl(child);
		}
	}
}
