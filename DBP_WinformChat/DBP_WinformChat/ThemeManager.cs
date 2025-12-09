using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DBP_Chat
{
    public enum ThemeMode { Light, Dark }

    public static class ThemeManager
    {
        public static ThemeMode CurrentMode { get; private set; } = ThemeMode.Light;
        public static event Action<ThemeMode>? ThemeChanged;
        public static event Action<bool>? DarkModeChanged;

        public static bool IsDarkMode => CurrentMode == ThemeMode.Dark;

        public static void SetTheme(ThemeMode mode)
        {
            if (CurrentMode == mode) return;
            CurrentMode = mode;

            foreach (Form f in Application.OpenForms)
                ApplyTheme(f);

            ThemeChanged?.Invoke(mode);
            DarkModeChanged?.Invoke(mode == ThemeMode.Dark);
        }

        public static void SetDarkMode(bool enable)
        {
            SetTheme(enable ? ThemeMode.Dark : ThemeMode.Light);
        }

        public static void Subscribe(Form form, Action<bool> handler)
        {
            DarkModeChanged += handler;
            form.FormClosed += (_, __) => DarkModeChanged -= handler;
        }

        public static void AddThemeRadios(Form form, Control? container = null)
        {
            var host = container ?? form;

            var rbLight = new RadioButton
            {
                Name = "rbLight",
                Text = "라이트",
                AutoSize = true,
                Location = new Point(host.Width - 160, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Checked = CurrentMode == ThemeMode.Light
            };
            rbLight.CheckedChanged += (s, e) => { if (rbLight.Checked) SetTheme(ThemeMode.Light); };

            var rbDark = new RadioButton
            {
                Name = "rbDark",
                Text = "다크",
                AutoSize = true,
                Location = new Point(host.Width - 90, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Checked = CurrentMode == ThemeMode.Dark
            };
            rbDark.CheckedChanged += (s, e) => { if (rbDark.Checked) SetTheme(ThemeMode.Dark); };

            host.Controls.Add(rbLight);
            host.Controls.Add(rbDark);
            host.Resize += (s, e) =>
            {
                rbLight.Location = new Point(host.Width - 160, 10);
                rbDark.Location = new Point(host.Width - 90, 10);
            };
        }

        public static void ApplyTheme(Form form)
        {
            var (bg, fg, card, muted) = GetColors(CurrentMode);
            form.BackColor = bg;
            form.ForeColor = fg;
            ApplyThemeRecursive(form, bg, fg, card, muted);

            foreach (var dgv in form.Controls.OfType<DataGridView>()
             .Concat(form.Controls.Cast<Control>().SelectMany(c => c.Controls.OfType<DataGridView>())))
            {
                dgv.BackgroundColor = bg;
                dgv.GridColor = muted;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = card;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = fg;
                dgv.DefaultCellStyle.BackColor = bg;
                dgv.DefaultCellStyle.ForeColor = fg;
                dgv.DefaultCellStyle.SelectionBackColor = CurrentMode == ThemeMode.Dark ? Color.DimGray : Color.LightGray;
                dgv.DefaultCellStyle.SelectionForeColor = fg;
            }
        }

        private static void ApplyThemeRecursive(Control root, Color bg, Color fg, Color card, Color muted)
        {
            foreach (Control c in root.Controls)
            {
                switch (c)
                {
                    case Button btn:
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.BackColor = CurrentMode == ThemeMode.Dark ? Color.Black : Color.White;
                        btn.ForeColor = fg;
                        btn.FlatAppearance.BorderColor = muted;
                        btn.FlatAppearance.BorderSize = 1;
                        break;
                    case TextBox tb:
                        tb.BackColor = CurrentMode == ThemeMode.Dark ? Color.Black : Color.White;
                        tb.ForeColor = fg;
                        tb.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case ComboBox cb:
                        cb.BackColor = CurrentMode == ThemeMode.Dark ? Color.Black : Color.White;
                        cb.ForeColor = fg;
                        break;
                    case TreeView tv:
                        tv.BackColor = bg;
                        tv.ForeColor = fg;
                        break;
                    case ListBox lb:
                        lb.BackColor = bg;
                        lb.ForeColor = fg;
                        break;
                    case Label:
                    case Panel:
                    case GroupBox:
                        c.BackColor = bg;
                        c.ForeColor = fg;
                        break;
                    case RadioButton rb:
                        rb.BackColor = Color.Transparent;
                        rb.ForeColor = fg;
                        break;
                    default:
                        if (c.Name.ToLower().Contains("card"))
                        {
                            c.BackColor = card;
                            c.ForeColor = fg;
                        }
                        else
                        {
                            c.BackColor = bg;
                            c.ForeColor = fg;
                        }
                        break;
                }
                if (c.HasChildren) ApplyThemeRecursive(c, bg, fg, card, muted);
            }
        }

        private static (Color bg, Color fg, Color card, Color muted) GetColors(ThemeMode mode)
        {
            return mode == ThemeMode.Dark
                ? (Color.Black, Color.White, Color.FromArgb(20, 20, 20), Color.FromArgb(80, 80, 80))
                : (Color.White, Color.Black, Color.FromArgb(245, 245, 245), Color.FromArgb(200, 200, 200));
        }

        // 메인 컬러 팔레트 - 모든 색상 정의
        public static class ColorScheme
        {
            // 배경색
            public static Color Ivory => IsDarkMode ? ColorTranslator.FromHtml("#1E1E1E") : ColorTranslator.FromHtml("#F1F3E0");
            public static Color LightOlive => IsDarkMode ? ColorTranslator.FromHtml("#2D2D2D") : ColorTranslator.FromHtml("#D2DCB6");
            public static Color White => IsDarkMode ? ColorTranslator.FromHtml("#2D2D2D") : Color.White;
            
            // 강조색
            public static Color SageGreen => IsDarkMode ? ColorTranslator.FromHtml("#E0E0E0") : ColorTranslator.FromHtml("#A1BC98");
            public static Color DarkOlive => IsDarkMode ? ColorTranslator.FromHtml("#B0B0B0") : ColorTranslator.FromHtml("#778873");
        }
    }
}