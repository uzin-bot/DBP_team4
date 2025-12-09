using DBP_Chat;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DBP_WinformChat
{
    internal class DeptUIHelper
    {


        public static void Apply(Form form)
        {
            form.BackColor = Color.White;
            // ��Ʈ�� �����̳� ��ġ�� ������ �ֹǷ� �������� ����

            foreach (Control c in form.Controls)
                StyleControl(c);
        }

        private static void StyleControl(Control ctrl)
        {
            // ========== ��� ��� �г� ==========
            if (ctrl is Panel pnl && pnl.Name == "headerPanel")
            {
                pnl.BackColor = Color.FromArgb(119, 136, 115);
                return;
            }

            // ========== ��� ����� �� ==========
            if (ctrl is Label lbl && lbl.Name == "headerLabel")
            {
                lbl.ForeColor = Color.White;
                lbl.BackColor = Color.Transparent;
            }


            // ===================== LABEL =====================
            if (ctrl is Label lbl2)
            {
                lbl2.ForeColor = Color.FromArgb(119, 136, 115);
            }

            // ===================== GROUPBOX =====================
            if (ctrl is GroupBox gb)
            {
                gb.BackColor = Color.FromArgb(241, 243, 224);
                gb.ForeColor = Color.FromArgb(119, 136, 115);
            }

            // ===================== PANEL =====================
            if (ctrl is Panel pnl2)
            {
                pnl2.BackColor = Color.FromArgb(241, 243, 224);
            }

            // ===================== TEXTBOX =====================
            if (ctrl is TextBox tb)
            {
                tb.BackColor = Color.White;
                tb.ForeColor = Color.Black;
                tb.BorderStyle = BorderStyle.FixedSingle;
            }

            // ===================== LISTBOX =====================
            if (ctrl is ListBox lb)
            {
                lb.BackColor = Color.White;
                lb.ForeColor = Color.Black;
                lb.BorderStyle = BorderStyle.FixedSingle;
            }

            // ===================== TREEVIEW =====================
            if (ctrl is TreeView tv)
            {
                tv.BackColor = Color.FromArgb(241, 243, 224);
                tv.ForeColor = Color.Black;
                tv.BorderStyle = BorderStyle.None;
            }

            // ===================== COMBOBOX =====================
            if (ctrl is ComboBox cb)
            {
                cb.BackColor = Color.White;                 // ��� ���
                cb.ForeColor = Color.Black;                 // ������ �ؽ�Ʈ
                cb.FlatStyle = FlatStyle.Standard;
                cb.DrawMode = DrawMode.Normal;              // ����Ʈ ��忡���� �⺻ �����
                cb.DrawItem -= null;                        // �̺�Ʈ �ڵ鷯 ���� (�ִٸ�)
            }

            // ===================== BUTTON =====================
            if (ctrl is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                btn.BackColor = Color.FromArgb(161, 188, 152);
                btn.ForeColor = Color.White;
                btn.Cursor = Cursors.Hand;

                Color ClickColor = Color.FromArgb(190, 200, 160);

                btn.MouseDown += (s, e) =>
                {
                    btn.BackColor = ClickColor;  //������ �� �÷�
                };

                btn.MouseUp += (s, e) =>
                {
                    btn.BackColor = Color.FromArgb(161, 188, 152);       //Ŭ������ �� ���� ���� �� ����
                };

            }

            // ===================== ��� ���� =====================
            foreach (Control child in ctrl.Controls)
                StyleControl(child);
        }
    }
}
