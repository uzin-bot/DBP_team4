using System;
using System.Drawing;
using System.Windows.Forms;

namespace DBP_Chat
{
    public static class ThemeRadioHelper
    {
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
                Checked = ThemeManager.CurrentMode == ThemeMode.Light
            };
            rbLight.CheckedChanged += (s, e) => { if (rbLight.Checked) ThemeManager.SetTheme(ThemeMode.Light); };

            var rbDark = new RadioButton
            {
                Name = "rbDark",
                Text = "다크",
                AutoSize = true,
                Location = new Point(host.Width - 90, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Checked = ThemeManager.CurrentMode == ThemeMode.Dark
            };
            rbDark.CheckedChanged += (s, e) => { if (rbDark.Checked) ThemeManager.SetTheme(ThemeMode.Dark); };

            host.Controls.Add(rbLight);
            host.Controls.Add(rbDark);
            host.Resize += (s, e) =>
            {
                rbLight.Location = new Point(host.Width - 160, 10);
                rbDark.Location = new Point(host.Width - 90, 10);
            };
        }
    }
}