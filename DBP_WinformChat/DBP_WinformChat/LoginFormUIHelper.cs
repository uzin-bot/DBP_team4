using DBP_Chat;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace leehaeun.UIHelpers
{
    public static class LoginFormUIHelper
    {
        /// <summary>
        /// �̴ϸ� ��Ÿ�� ����
        /// </summary>
        public static void ApplyStyles(LoginForm form)
        {
            // �� �⺻ ����
            form.BackColor = ThemeManager.ColorScheme.Ivory; // #F1F3E0
            form.Size = new Size(350, 600);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.None;

            // �ձ� �𼭸�
            ApplyRoundedCorners(form, 15);

            // Ŀ���� Ÿ��Ʋ�� ����
            CreateCustomTitleBar(form);

            // �� ��Ʈ�� ��Ÿ�� ����
            StyleAllControls(form);

            // ���̾ƿ� ����
            AdjustLayout(form);
        }

        /// <summary>
        /// �� �ձ� �𼭸�
        /// </summary>
        private static void ApplyRoundedCorners(Form form, int radius)
        {
            form.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, form.Width, form.Height, radius, radius));
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// Ŀ���� Ÿ��Ʋ�� ����
        /// </summary>
        private static void CreateCustomTitleBar(Form form)
        {
            Panel titleBar = new Panel
            {
                Name = "titleBar",
                Height = 50,
                Dock = DockStyle.Top,
                BackColor = ThemeManager.ColorScheme.Ivory
            };

            Button closeButton = new Button
            {
                Name = "closeButton",
                Text = "×",
                Font = new Font("맑은 고딕", 11F, FontStyle.Bold),
                Size = new Size(50, 40),
                Location = new Point(form.Width - 55, 5),
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

            // Ÿ��Ʋ�� �巡��
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

        /// <summary>
        /// ��� ��Ʈ�� ��Ÿ�� ����
        /// </summary>
        private static void StyleAllControls(Form form)
        {
            var textBoxes = new System.Collections.Generic.List<TextBox>();
            foreach (Control control in form.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBoxes.Add(textBox);
                }
            }

            foreach (var textBox in textBoxes)
            {
                StyleTextBox(textBox, form);
            }

            foreach (Control control in form.Controls)
            {
                if (control is Button button && button.Name != "closeButton")
                {
                    StyleButton(button);
                }
                else if (control is CheckBox checkBox)
                {
                    StyleCheckBox(checkBox);
                }
                else if (control is LinkLabel linkLabel)
                {
                    StyleLinkLabel(linkLabel);
                }
            }
        }

        /// <summary>
        /// TextBox ��Ÿ��
        /// </summary>
        private static void StyleTextBox(TextBox textBox, Form form)
        {
            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("맑은 고딕", 10F);
            textBox.BackColor = ThemeManager.ColorScheme.White;

            string placeHolder = "";
            bool isPasswordBox = false;

            if (textBox.Name == "IdBox")
            {
                placeHolder = "아이디";
            }
            else if (textBox.Name == "PwBox")
            {
                placeHolder = "비밀번호";
                isPasswordBox = true;
                textBox.PasswordChar = '\0';
            }

            if (!string.IsNullOrEmpty(placeHolder))
            {
                textBox.Text = placeHolder;
                textBox.ForeColor = ThemeManager.ColorScheme.SageGreen;

                textBox.Enter += (s, e) =>
                {
                    if (textBox.Text == placeHolder)
                    {
                        textBox.Text = "";
                        textBox.ForeColor = ThemeManager.ColorScheme.DarkOlive;

                        // ��й�ȣ �ڽ��� �Է� ������ �� PasswordChar Ȱ��ȭ
                        if (isPasswordBox)
                        {
                            textBox.PasswordChar = '●';
                        }
                    }
                };

                textBox.Leave += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.Text = placeHolder;
                        textBox.ForeColor = ThemeManager.ColorScheme.SageGreen;

                        // ��й�ȣ �ڽ��� PlaceHolder ǥ�� �� PasswordChar ��Ȱ��ȭ
                        if (isPasswordBox)
                        {
                            textBox.PasswordChar = '\0';
                        }
                    }
                };

                // ��й�ȣ �ڽ� �ؽ�Ʈ ���� ����
                if (isPasswordBox)
                {
                    textBox.TextChanged += (s, e) =>
                    {
                        // PlaceHolder�� �ƴϰ� �ؽ�Ʈ�� ������ PasswordChar Ȱ��ȭ
                        if (textBox.Text != placeHolder && !string.IsNullOrEmpty(textBox.Text))
                        {
                            if (textBox.PasswordChar != '●')
                            {
                                textBox.PasswordChar = '●';
                            }
                        }
                    };
                }
            }
            else
            {
                textBox.ForeColor = ThemeManager.ColorScheme.DarkOlive;
            }

            Panel wrapper = new Panel
            {
                Size = new Size(230, 45),
                Location = textBox.Location,
                BackColor = ThemeManager.ColorScheme.White,
                Tag = textBox.Name
            };

            textBox.Location = new Point(15, 13);
            textBox.Width = wrapper.Width - 30;
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            var parent = textBox.Parent;
            parent.Controls.Remove(textBox);
            wrapper.Controls.Add(textBox);
            parent.Controls.Add(wrapper);

            GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, wrapper.Width, wrapper.Height), 8);
            wrapper.Region = new Region(path);

            wrapper.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, wrapper.Width - 1, wrapper.Height - 1), 8))
                {
                    using (Pen pen = new Pen(ThemeManager.ColorScheme.SageGreen, 0.8f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };
        }


        /// <summary>
        /// Button ��Ÿ��
        /// </summary>
        private static void StyleButton(Button button)
        {
            button.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
            button.BackColor = ThemeManager.ColorScheme.SageGreen;
            button.ForeColor = ThemeManager.ColorScheme.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Height = 50;
            button.Cursor = Cursors.Hand;

            button.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = GetRoundedRectangle(button.ClientRectangle, 10))
                {
                    button.Region = new Region(path);
                }
            };

            button.MouseEnter += (s, e) => button.BackColor = ThemeManager.ColorScheme.DarkOlive;
            button.MouseLeave += (s, e) => button.BackColor = ThemeManager.ColorScheme.SageGreen;
        }

        /// <summary>
        /// CheckBox ��Ÿ��
        /// </summary>
        private static void StyleCheckBox(CheckBox checkBox)
        {
            // ���� �ؽ�Ʈ ����
            string originalText = checkBox.Text;
            checkBox.Text = "";

            checkBox.Appearance = Appearance.Normal;
            checkBox.FlatStyle = FlatStyle.Flat;
            checkBox.FlatAppearance.BorderSize = 0;
            checkBox.BackColor = ThemeManager.ColorScheme.Ivory;
            checkBox.Font = new Font("맑은 고딕", 9F);
            checkBox.ForeColor = ThemeManager.ColorScheme.DarkOlive;
            checkBox.Cursor = Cursors.Hand;
            checkBox.AutoSize = false;
            checkBox.Width = 230;
            checkBox.Height = 22;

            // Ŀ���� �׸���
            checkBox.Paint += (s, e) =>
            {
                CheckBox cb = s as CheckBox;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.Clear(ThemeManager.ColorScheme.Ivory); // ��� �����

                // üũ�ڽ� �׸� ��ġ
                Rectangle checkBoxRect = new Rectangle(0, 3, 16, 16);

                // ���콺 ȣ�� Ȯ��
                bool isHover = cb.ClientRectangle.Contains(cb.PointToClient(Cursor.Position));

                // ���� ����
                Color bgColor;
                if (isHover)
                {
                    bgColor = ThemeManager.ColorScheme.LightOlive;
                }
                else
                {
                    bgColor = cb.Checked ? ThemeManager.ColorScheme.SageGreen : ThemeManager.ColorScheme.White;
                }

                // �ձ� �׸� �׸���
                using (GraphicsPath path = GetRoundedRectangle(checkBoxRect, 4))
                {
                    using (SolidBrush brush = new SolidBrush(bgColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    using (Pen pen = new Pen(ThemeManager.ColorScheme.SageGreen, 1.5f))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }

                // üũ ǥ��
                if (cb.Checked)
                {
                    using (Pen pen = new Pen(ThemeManager.ColorScheme.White, 2f))
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        e.Graphics.DrawLines(pen, new Point[]
                        {
                    new Point(4, 11),
                    new Point(7, 14),
                    new Point(13, 7)
                        });
                    }
                }

                // �ؽ�Ʈ �׸���
                TextRenderer.DrawText(
                    e.Graphics,
                    originalText,
                    cb.Font,
                    new Rectangle(22, 0, cb.Width - 22, cb.Height),
                    cb.ForeColor,
                    ThemeManager.ColorScheme.Ivory, // ����
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left
                );
            };

            // ȣ�� �� �ٽ� �׸���
            checkBox.MouseEnter += (s, e) => checkBox.Invalidate();
            checkBox.MouseLeave += (s, e) => checkBox.Invalidate();
            checkBox.MouseMove += (s, e) => checkBox.Invalidate();
            checkBox.CheckedChanged += (s, e) => checkBox.Invalidate();
        }

        /// <summary>
        /// LinkLabel ��Ÿ��
        /// </summary>
        private static void StyleLinkLabel(LinkLabel linkLabel)
        {
            linkLabel.Font = new Font("맑은 고딕", 8.5F);
            linkLabel.LinkColor = ThemeManager.ColorScheme.SageGreen;
            linkLabel.ActiveLinkColor = ThemeManager.ColorScheme.DarkOlive;
            linkLabel.VisitedLinkColor = ThemeManager.ColorScheme.SageGreen;
            linkLabel.BackColor = ThemeManager.ColorScheme.Ivory;
            linkLabel.Cursor = Cursors.Hand;
            linkLabel.LinkBehavior = LinkBehavior.AlwaysUnderline;

            linkLabel.MouseEnter += (s, e) => linkLabel.LinkColor = ThemeManager.ColorScheme.DarkOlive;
            linkLabel.MouseLeave += (s, e) => linkLabel.LinkColor = ThemeManager.ColorScheme.SageGreen;
        }

        /// <summary>
        /// ���̾ƿ� ����
        /// </summary>
        private static void AdjustLayout(Form form)
        {
            int centerX = (form.ClientSize.Width - 230) / 2;
            int startY = 158;

            foreach (Control control in form.Controls)
            {
                if (control.Name == "IdBox" || (control is Panel && control.Tag?.ToString() == "IdBox"))
                {
                    control.Location = new Point(centerX, startY);
                    control.Width = 230;
                }
                else if (control.Name == "PwBox" || (control is Panel && control.Tag?.ToString() == "PwBox"))
                {
                    control.Location = new Point(centerX, startY + 60);
                    control.Width = 230;
                }
                else if (control.Name == "RememberMeCheckBox")
                {
                    control.Location = new Point(centerX, startY + 110);
                    control.Width = 230;
                }
                else if (control.Name == "SaveInfoCheckBox")
                {
                    control.Location = new Point(centerX, startY + 135);
                    control.Width = 230;
                }
                else if (control.Name == "LoginButton")
                {
                    control.Location = new Point(centerX, startY + 175);
                    control.Width = 230;
                }
                else if (control.Name == "SignUpLInkLabel")
                {
                    control.Location = new Point(centerX + 85, startY + 245);
                }
            }
        }

        /// <summary>
        /// �ձ� �簢�� ��� ����
        /// </summary>
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
