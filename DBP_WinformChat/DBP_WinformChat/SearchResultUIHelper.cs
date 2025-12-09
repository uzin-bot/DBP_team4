using DBP_Chat;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DBP_WinformChat
{
    internal class SearchResultUIHelper
    {
        public static void Apply(Form form, bool isDarkMode)
        {
            form.SuspendLayout();

            // 배경색
            form.BackColor = isDarkMode 
                ? ThemeManager.ColorScheme.Ivory 
                : ThemeManager.ColorScheme.Ivory;

            foreach (Control c in form.Controls)
            {
                StyleControl(c, isDarkMode);
            }

            // 모든 ListView 항목 색상 적용
            foreach (var control in form.Controls)
            {
                if (control is ListView lv)
                {
                    lv.OwnerDraw = false;
                    foreach (ListViewItem item in lv.Items)
                    {
                        item.BackColor = isDarkMode 
                            ? Color.FromArgb(40, 40, 40) 
                            : Color.White;
                        item.ForeColor = isDarkMode 
                            ? Color.White 
                            : ThemeManager.ColorScheme.DarkOlive;
                        
                        foreach (ListViewItem.ListViewSubItem sub in item.SubItems)
                        {
                            sub.ForeColor = item.ForeColor;
                            sub.BackColor = item.BackColor;
                        }
                    }
                }
            }

            form.ResumeLayout(true);
        }

        private static void StyleControl(Control ctrl, bool isDarkMode)
        {
            if (ctrl is Panel pnl)
            {
                pnl.BackColor = isDarkMode 
                    ? Color.FromArgb(45, 45, 45) 
                    : ThemeManager.ColorScheme.SageGreen;
            }

            if (ctrl is Label lbl)
            {
                lbl.BackColor = isDarkMode 
                    ? Color.FromArgb(45, 45, 45) 
                    : ThemeManager.ColorScheme.SageGreen;
                lbl.ForeColor = isDarkMode 
                    ? Color.White 
                    : ThemeManager.ColorScheme.White;
                lbl.Font = new Font("맑은 고딕", lbl.Font.Size, lbl.Font.Style);
            }

            if (ctrl is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.UseVisualStyleBackColor = false;
                btn.BackColor = isDarkMode 
                    ? Color.FromArgb(60, 60, 60) 
                    : ThemeManager.ColorScheme.SageGreen;
                btn.ForeColor = isDarkMode 
                    ? Color.White 
                    : ThemeManager.ColorScheme.White;
                
                var accentColor = isDarkMode 
                    ? Color.FromArgb(80, 80, 80) 
                    : ThemeManager.ColorScheme.DarkOlive;
                
                btn.FlatAppearance.MouseOverBackColor = accentColor;
                btn.FlatAppearance.MouseDownBackColor = accentColor;
                btn.FlatAppearance.CheckedBackColor = accentColor;
                btn.FlatAppearance.BorderColor = accentColor;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;
            }

            if (ctrl is ListView lv)
            {
                lv.OwnerDraw = false;
                lv.BackColor = isDarkMode 
                    ? Color.FromArgb(40, 40, 40) 
                    : Color.White;
                lv.ForeColor = isDarkMode 
                    ? Color.White 
                    : ThemeManager.ColorScheme.DarkOlive;
                lv.BorderStyle = BorderStyle.FixedSingle;
                lv.FullRowSelect = true;
                lv.HideSelection = false;
                lv.Font = new Font("맑은 고딕", 9F);

                // 열 너비 조정
                if (lv.Parent != null)
                {
                    lv.Width = Math.Max(lv.Width, lv.Parent.ClientSize.Width - 40);
                }

                foreach (ColumnHeader ch in lv.Columns)
                {
                    var text = (ch.Text ?? string.Empty).Trim();
                    
                    if (text.Equals("닉네임", StringComparison.OrdinalIgnoreCase) || 
                        text.Equals("Nickname", StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 160);
                    else if (text.Equals("팀", StringComparison.OrdinalIgnoreCase) || 
                             text.Equals("Team", StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 140);
                    else if (text.Equals("부서", StringComparison.OrdinalIgnoreCase) || 
                             text.Equals("Dept", StringComparison.OrdinalIgnoreCase) || 
                             text.Equals("Department", StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 200);
                    else if (text.Equals("이름", StringComparison.OrdinalIgnoreCase) || 
                             text.Equals("Name", StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 150);
                    else if (text.Equals("ID", StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 130);
                }
            }

            // 하위 컨트롤 재귀 처리
            foreach (Control child in ctrl.Controls)
                StyleControl(child, isDarkMode);
        }
    }
}
