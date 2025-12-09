using DBP_Chat;
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
            form.Font = new Font("���� ���", 10);

            foreach (Control c in form.Controls)
                StyleControl(c);
        }

        private static void StyleControl(Control ctrl)
        {
            // ========== ��� ���(Label) ==========
            if (ctrl is Label lbl && lbl.Name == "label1")
            {
                lbl.BackColor = Color.FromArgb(119, 136, 115);
                lbl.ForeColor = Color.White;
                lbl.Font = new Font("���� ���", 13, FontStyle.Bold);
                lbl.AutoSize = false;
                lbl.Dock = DockStyle.Top;
                lbl.Height = 80;
                lbl.TextAlign = ContentAlignment.MiddleLeft;
                lbl.Padding = new Padding(15, 0, 0, 0);
            }

            // ========== �г� ==========
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
                lv.HotTracking = false;
                lv.HoverSelection = false;

                lv.BackColor = Color.White;
                lv.ForeColor = Color.FromArgb(119, 136, 115);

                // ? ���콺 �̺�Ʈ ���� ����
                lv.MouseMove += (s, e) => { };

                // --- �÷� ��� ---
                lv.DrawColumnHeader += (s, e) =>
                {
                    using (SolidBrush br = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillRectangle(br, e.Bounds);
                    }

                    TextRenderer.DrawText(
                        e.Graphics,
                        e.Header.Text,
                        new Font("���� ���", 10, FontStyle.Bold),
                        e.Bounds,
                        Color.FromArgb(119, 136, 115),
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left
                    );
                };

                // --- �� ��� (ȣ�� ����) ---
                lv.DrawItem += (s, e) =>
                {
                    e.DrawDefault = false;

                    // ? Selected ���¸� üũ
                    Color bg = e.Item.Selected
                        ? Color.FromArgb(210, 220, 182)
                        : Color.FromArgb(241, 243, 224);

                    using (SolidBrush br = new SolidBrush(bg))
                    {
                        e.Graphics.FillRectangle(br, e.Bounds);
                    }
                };

                // --- �� �ؽ�Ʈ & ������ ---
                lv.DrawSubItem += (s, e) =>
                {
                    // ? ��� ���� �׸��� (Selected ���¸� Ȯ��)
                    Color cellBg = e.Item.Selected
                        ? Color.FromArgb(210, 220, 182)
                        : Color.FromArgb(241, 243, 224);

                    using (SolidBrush bgBrush = new SolidBrush(cellBg))
                    {
                        e.Graphics.FillRectangle(bgBrush, e.Bounds);
                    }

                    // ù ��° �÷�(������)�� chatlist.cs���� ó��
                    if (e.ColumnIndex == 0)
                    {
                        return;
                    }

                    // �⺻ �ؽ�Ʈ ���
                    TextRenderer.DrawText(
                        e.Graphics,
                        e.SubItem.Text,
                        new Font("���� ���", 10),
                        e.Bounds,
                        Color.FromArgb(119, 136, 115),
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left
                    );
                };
            }

            // ========== ��ư ==========
            if (ctrl is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                btn.BackColor = Color.FromArgb(241, 243, 224);
                btn.ForeColor = Color.FromArgb(119, 136, 115);

                btn.Font = new Font("���� ���", 9.5f, FontStyle.Bold);
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
