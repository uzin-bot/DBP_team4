using DBP_Chat;
using System.Drawing;
using System.Windows.Forms;

namespace DBP_WinformChat
{
    internal class SearchResultUIHelper
    {
        public static void Apply(Form form, bool darkMode = false)
        {
            form.SuspendLayout();

            form.BackColor = darkMode ? Color.FromArgb(32, 32, 32) : Color.FromArgb(241, 243, 224);

            foreach (Control c in form.Controls)
            {
                StyleControl(c, darkMode);
            }

            // ��� ListView �׸�/��� �� �缳�� (��ũ��� ���� �� ���� ��� ���� ����)
            foreach (var lv in form.Controls)
            {
                if (lv is ListView list)
                {
                    list.OwnerDraw = false; // Ŀ���� �׸��� ����
                    foreach (ListViewItem it in list.Items)
                    {
                        it.BackColor = darkMode ? Color.FromArgb(40, 40, 40) : Color.White;
                        it.ForeColor = darkMode ? Color.White : Color.Black;
                        foreach (ListViewItem.ListViewSubItem sub in it.SubItems)
                        {
                            sub.ForeColor = it.ForeColor;
                            sub.BackColor = it.BackColor;
                        }
                    }
                }
            }

            form.ResumeLayout(true);
        }

        private static void StyleControl(Control ctrl, bool darkMode)
        {
            if (ctrl is Panel pnl)
            {
                pnl.BackColor = darkMode ? Color.FromArgb(45, 45, 45) : Color.FromArgb(119, 136, 115);
            }

            if (ctrl is Label lbl)
            {
                lbl.BackColor = darkMode ? Color.FromArgb(45, 45, 45) : Color.FromArgb(119, 136, 115);
                lbl.ForeColor = Color.White;
            }

            if (ctrl is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.UseVisualStyleBackColor = false;
                btn.BackColor = darkMode ? Color.FromArgb(60, 60, 60) : Color.FromArgb(119, 136, 115);
                btn.ForeColor = Color.White;
                var accent = darkMode ? Color.FromArgb(60, 60, 60) : Color.FromArgb(119, 136, 115);
                btn.FlatAppearance.MouseOverBackColor = accent;
                btn.FlatAppearance.MouseDownBackColor = accent;
                btn.FlatAppearance.CheckedBackColor = accent;
                btn.FlatAppearance.BorderColor = accent;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            }

            if (ctrl is ListView lv)
            {
                lv.OwnerDraw = false; // ��ũ��� ���� �� �ý��� ������ ���
                lv.BackColor = darkMode ? Color.FromArgb(40, 40, 40) : Color.White;
                lv.ForeColor = darkMode ? Color.White : Color.Black;
                lv.BorderStyle = BorderStyle.FixedSingle;
                lv.FullRowSelect = true;
                lv.HideSelection = false;

                if (lv.Parent != null)
                {
                    lv.Width = Math.Max(lv.Width, lv.Parent.ClientSize.Width - 40);
                }

                foreach (ColumnHeader ch in lv.Columns)
                {
                    var text = (ch.Text ?? string.Empty).Trim();
                    if (text.Equals("�г���") || text.Equals("Nickname", System.StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 160);
                    else if (text.Equals("��") || text.Equals("Team", System.StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 140);
                    else if (text.Equals("�μ�") || text.Equals("Dept", System.StringComparison.OrdinalIgnoreCase) || text.Equals("Department", System.StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 200);
                    else if (text.Equals("�̸�") || text.Equals("Name", System.StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 150);
                    else if (text.Equals("ID", System.StringComparison.OrdinalIgnoreCase))
                        ch.Width = Math.Max(ch.Width, 130);
                }
            }

            foreach (Control child in ctrl.Controls)
                StyleControl(child, darkMode);
        }
    }
}
