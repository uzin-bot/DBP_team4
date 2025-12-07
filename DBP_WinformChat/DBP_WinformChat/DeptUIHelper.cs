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
            // 폰트는 디자이너 배치에 영향을 주므로 변경하지 않음

            foreach (Control c in form.Controls)
                StyleControl(c);
        }

        private static void StyleControl(Control ctrl)
        {
            // ========== 상단 헤더 패널 ==========
            if (ctrl is Panel pnl && pnl.Name == "headerPanel")
            {
                pnl.BackColor = Color.FromArgb(119, 136, 115);
                return;
            }

            // ========== 상단 헤더의 라벨 ==========
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
                cb.BackColor = Color.White;                 // 흰색 배경
                cb.ForeColor = Color.Black;                 // 검은색 텍스트
                cb.FlatStyle = FlatStyle.Standard;
                cb.DrawMode = DrawMode.Normal;              // 라이트 모드에서는 기본 드로잉
                cb.DrawItem -= null;                        // 이벤트 핸들러 제거 (있다면)
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
                    btn.BackColor = ClickColor;  //눌렀을 때 컬러
                };

                btn.MouseUp += (s, e) =>
                {
                    btn.BackColor = Color.FromArgb(161, 188, 152);       //클릭에서 손 떼면 원래 색 복귀
                };

            }

            // ===================== 재귀 적용 =====================
            foreach (Control child in ctrl.Controls)
                StyleControl(child);
        }
    }
}