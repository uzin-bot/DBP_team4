using DBP_Chat;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace leehaeun.UIHelpers
{
    public static class UserInfoFormUIHelper
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// »ç¿ëÀÚ Á¤º¸ Æû ½ºÅ¸ÀÏ Àû¿ë
        /// </summary>
        public static void ApplyStyles(UserInfoForm form)
        {
            form.BackColor = ThemeManager.ColorScheme.Ivory;
            form.FormBorderStyle = FormBorderStyle.None;

            form.Width = 350;
            form.Height = 250;  // 200 ¡æ 250
            form.MinimumSize = new Size(350, 250);
            form.MaximumSize = new Size(350, 250);

            form.StartPosition = FormStartPosition.CenterParent;

            ApplyRoundedCorners(form, 15);
            CreateCustomTitleBar(form);
            StyleControls(form);
            AdjustLayout(form);

            form.Refresh();
        }

        private static void ApplyRoundedCorners(Form form, int radius)
        {
            form.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, form.Width, form.Height, radius, radius));
        }

        private static void CreateCustomTitleBar(Form form)
        {
            Panel titleBar = new Panel
            {
                Name = "titleBar",
                Height = 40,
                Dock = DockStyle.Top,
                BackColor = ThemeManager.ColorScheme.Ivory
            };

            Button closeButton = new Button
            {
                Name = "closeButton",
                Text = "?",
                Font = new Font("¸¼Àº °íµñ", 11F, FontStyle.Bold),
                Size = new Size(40, 30),
                Location = new Point(form.Width - 45, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = ThemeManager.ColorScheme.Ivory,
                ForeColor = ThemeManager.ColorScheme.LightOlive,
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatAppearance.MouseOverBackColor = ThemeManager.ColorScheme.Ivory;
            closeButton.FlatAppearance.MouseDownBackColor = ThemeManager.ColorScheme.Ivory;
            closeButton.Click += (s, e) => form.Close();
            closeButton.MouseEnter += (s, e) => closeButton.ForeColor = ThemeManager.ColorScheme.DarkOlive;
            closeButton.MouseLeave += (s, e) => closeButton.ForeColor = ThemeManager.ColorScheme.LightOlive;

            titleBar.Controls.Add(closeButton);

            bool isDragging = false;
            Point dragStart = Point.Empty;

            titleBar.MouseDown += (s, e) =>
            {
                isDragging = true;
                dragStart = e.Location;
            };

            titleBar.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    Point newLocation = form.Location;
                    newLocation.X += e.X - dragStart.X;
                    newLocation.Y += e.Y - dragStart.Y;
                    form.Location = newLocation;
                }
            };

            titleBar.MouseUp += (s, e) => isDragging = false;

            form.Controls.Add(titleBar);
            titleBar.BringToFront();
        }

        private static void StyleControls(Form form)
        {
            foreach (Control control in form.Controls)
            {
                if (control is PictureBox pb && pb.Name == "ProfileImageBox")
                {
                    StyleProfileImage(pb);
                }
                else if (control is Label label)
                {
                    StyleLabel(label);
                }
            }
        }

        private static void StyleProfileImage(PictureBox pictureBox)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            // µÕ±Ù »ç°¢Çü Å×µÎ¸®
            pictureBox.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, pictureBox.Width, pictureBox.Height), 12))
                {
                    pictureBox.Region = new Region(path);
                }

                using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(1, 1, pictureBox.Width - 3, pictureBox.Height - 3), 12))
                {
                    using (Pen pen = new Pen(ThemeManager.ColorScheme.SageGreen, 1.5f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };
        }

        private static void StyleLabel(Label label)
        {
            label.BackColor = Color.Transparent;
            label.ForeColor = ThemeManager.ColorScheme.DarkOlive;

            if (label.Name == "NicknameLabel")
            {
                label.Font = new Font("¸¼Àº °íµñ", 14F, FontStyle.Bold);
            }
            else if (label.Name == "DeptLabel")
            {
                label.Font = new Font("¸¼Àº °íµñ", 10F);
            }
            else if (label.Name == "StatusMessageLabel")
            {
                label.Font = new Font("¸¼Àº °íµñ", 10F);  // ÀÌÅÅ¸¯Ã¼ Á¦°Å
                label.ForeColor = ThemeManager.ColorScheme.DarkOlive;  // ÁøÇÑ »öÀ¸·Î º¯°æ
            }
        }

        private static void AdjustLayout(Form form)
        {
            int margin = 30;
            int topMargin = 60;

            foreach (Control control in form.Controls)
            {
                if (control is PictureBox pb && pb.Name == "ProfileImageBox")
                {
                    pb.Location = new Point(margin, topMargin);
                    pb.Size = new Size(80, 80);
                }
                else if (control is Label label)
                {
                    if (label.Name == "NicknameLabel")
                    {
                        label.Location = new Point(margin + 100, topMargin + 10);
                        label.AutoSize = true;
                    }
                    else if (label.Name == "DeptLabel")
                    {
                        label.Location = new Point(margin + 100, topMargin + 45);
                        label.AutoSize = true;
                    }
                    else if (label.Name == "StatusMessageLabel")
                    {
                        label.Location = new Point(margin, topMargin + 100);
                        label.MaximumSize = new Size(290, 50);  // 2ÁÙ µé¾î°¥ ³ôÀÌ
                        label.AutoSize = true;
                    }
                }
            }
        }

        private static GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
