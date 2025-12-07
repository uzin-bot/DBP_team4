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

                // 폼 폭에 맞게 ListView 폭 확장 (우측 여백 고려)
                if (lv.Parent != null)
                {
                    // 너무 과도하게 넓어지지 않도록 여백을 조금 더 둔다
                    lv.Width = Math.Max(lv.Width, lv.Parent.ClientSize.Width - 40);
                }

                // 컬럼 폭 조정: 텍스트 기준 매칭
                foreach (ColumnHeader ch in lv.Columns)
                {
                    var text = (ch.Text ?? string.Empty).Trim();
                    if (text.Equals("닉네임") || text.Equals("Nickname", System.StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 160);
                    else if (text.Equals("팀") || text.Equals("Team", System.StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 140);
                    else if (text.Equals("부서") || text.Equals("Dept", System.StringComparison.OrdinalIgnoreCase) || text.Equals("Department", System.StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 200);
                    else if (text.Equals("이름") || text.Equals("Name", System.StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 150);
                    else if (text.Equals("ID", System.StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 130);
                }
            }

            //자식 컨트롤에도 동일 적용 (재귀)
            foreach (Control child in ctrl.Controls)
                StyleControl(child);
        }
    }
}