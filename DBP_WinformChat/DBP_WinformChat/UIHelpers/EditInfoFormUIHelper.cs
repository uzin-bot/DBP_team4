using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using leehaeun.Themes;

namespace leehaeun.UIHelpers
{
    public static class EditInfoFormUIHelper
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private static int selectedTab = 0;
        private static Panel[] cachedPages = null;
        private static Panel cachedTabBar = null;
        private static Panel cachedContentBox = null;
        private static Action onTabChanged = null;

        public static void ApplyStyles(EditInfoForm form)
        {
            form.BackColor = ColorSchemes.Ivory;
            form.Size = new Size(450, 700);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.None;

            ApplyRoundedCorners(form, 15);
            CreateCustomTitleBar(form);
            CreateCustomTabs(form);
            StyleAllControls(form);
            AdjustLayout(form);
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
                Height = 50,
                Dock = DockStyle.Top,
                BackColor = ColorSchemes.Ivory
            };

            Button closeButton = new Button
            {
                Name = "closeButton",
                Text = "✕",
                Font = new Font("맑은 고딕", 11F, FontStyle.Bold),
                Size = new Size(50, 40),
                Location = new Point(form.Width - 55, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorSchemes.Ivory,
                ForeColor = ColorSchemes.LightOlive,
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatAppearance.MouseOverBackColor = ColorSchemes.Ivory;
            closeButton.FlatAppearance.MouseDownBackColor = ColorSchemes.Ivory;
            closeButton.Click += (s, e) => form.Close();
            closeButton.MouseEnter += (s, e) => closeButton.ForeColor = ColorSchemes.DarkOlive;
            closeButton.MouseLeave += (s, e) => closeButton.ForeColor = ColorSchemes.LightOlive;

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

        private static void CreateCustomTabs(Form form)
        {
            TabControl originalTab = null;
            foreach (Control c in form.Controls)
            {
                if (c is TabControl tc)
                {
                    originalTab = tc;
                    break;
                }
            }

            if (originalTab == null) return;
            originalTab.Visible = false;

            Panel mainContainer = new Panel
            {
                Name = "mainContainer",
                Location = new Point(25, 50),
                Size = new Size(form.ClientSize.Width - 50, form.ClientSize.Height - 75),
                BackColor = ColorSchemes.Ivory
            };

            Panel tabBar = new Panel
            {
                Name = "tabBar",
                Location = new Point(0, 0),
                Size = new Size(mainContainer.Width, 50),
                BackColor = ColorSchemes.Ivory
            };

            Panel contentBox = new Panel
            {
                Name = "contentBox",
                Location = new Point(0, 49),
                Size = new Size(mainContainer.Width, mainContainer.Height - 49),
                BackColor = ColorSchemes.Ivory
            };

            contentBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                int w = contentBox.Width - 2;
                int h = contentBox.Height - 2;
                int r = 12;

                using (GraphicsPath bgPath = new GraphicsPath())
                {
                    bgPath.AddLine(1, 0, 1, h - r);
                    bgPath.AddArc(1, h - r * 2, r * 2, r * 2, 90, 90);
                    bgPath.AddLine(r + 1, h, w - r, h);
                    bgPath.AddArc(w - r * 2, h - r * 2, r * 2, r * 2, 0, 90);
                    bgPath.AddLine(w, h - r, w, 0);
                    bgPath.AddLine(w, 0, 1, 0);
                    bgPath.CloseFigure();

                    using (SolidBrush bgBrush = new SolidBrush(Color.White))
                    {
                        g.FillPath(bgBrush, bgPath);
                    }
                }

                using (Pen pen = new Pen(ColorSchemes.SageGreen, 1.5f))
                {
                    g.DrawLine(pen, 1, 0, 1, h - r);
                    g.DrawArc(pen, 1, h - r * 2, r * 2, r * 2, 90, 90);
                    g.DrawLine(pen, r + 1, h, w - r, h);
                    g.DrawArc(pen, w - r * 2, h - r * 2, r * 2, r * 2, 0, 90);
                    g.DrawLine(pen, w, h - r, w, 0);
                }
            };

            string[] tabNames = { "프로필", "멀티프로필", "계정 정보" };
            Panel[] pages = new Panel[3];

            for (int i = 0; i < 3; i++)
            {
                pages[i] = new Panel
                {
                    Name = "customPage" + i,
                    Location = new Point(20, 20),
                    Size = new Size(contentBox.Width - 40, contentBox.Height - 40),
                    BackColor = Color.White,
                    Visible = (i == 0),
                    AutoScroll = false
                };

                TabPage oldPage = originalTab.TabPages[i];
                while (oldPage.Controls.Count > 0)
                {
                    Control ctrl = oldPage.Controls[0];
                    oldPage.Controls.Remove(ctrl);
                    pages[i].Controls.Add(ctrl);
                }

                contentBox.Controls.Add(pages[i]);
            }

            cachedPages = pages;
            cachedTabBar = tabBar;
            cachedContentBox = contentBox;

            for (int i = 0; i < 3; i++)
            {
                int index = i;
                int tabWidth = tabBar.Width / 3;
                int xPos = (i * tabWidth) - i;

                int actualWidth;
                if (i == 0)
                    actualWidth = tabWidth + 1;
                else if (i == 1)
                    actualWidth = tabWidth + 2;
                else
                    actualWidth = (tabBar.Width - ((tabWidth * 2) - 2)) - 1;

                Panel tabButton = new Panel
                {
                    Name = "tabButton" + i,
                    Location = new Point(xPos, 0),
                    Size = new Size(actualWidth, 50),
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand,
                    Tag = i
                };

                Label tabLabel = new Label
                {
                    Text = tabNames[i],
                    Font = new Font("맑은 고딕", 10F, FontStyle.Bold),
                    ForeColor = ColorSchemes.DarkOlive,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand
                };

                tabButton.Paint += (s, e) =>
                {
                    Panel btn = (Panel)s;
                    Graphics g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    bool isSelected = ((int)btn.Tag == selectedTab);
                    int r = 12;

                    using (GraphicsPath bgPath = new GraphicsPath())
                    {
                        bgPath.AddArc(1, 1, r * 2, r * 2, 180, 90);
                        bgPath.AddLine(r + 1, 1, btn.Width - r - 1, 1);
                        bgPath.AddArc(btn.Width - r * 2 - 1, 1, r * 2, r * 2, 270, 90);
                        bgPath.AddLine(btn.Width - 1, r + 1, btn.Width - 1, btn.Height);
                        bgPath.AddLine(btn.Width - 1, btn.Height, 1, btn.Height);
                        bgPath.AddLine(1, btn.Height, 1, r + 1);
                        bgPath.CloseFigure();

                        using (SolidBrush brush = new SolidBrush(isSelected ? Color.White : ColorSchemes.Ivory))
                        {
                            g.FillPath(brush, bgPath);
                        }
                    }

                    using (Pen pen = new Pen(ColorSchemes.SageGreen, 1.5f))
                    {
                        g.DrawArc(pen, 1, 1, r * 2, r * 2, 180, 90);
                        g.DrawLine(pen, r + 1, 1, btn.Width - r - 1, 1);
                        g.DrawArc(pen, btn.Width - r * 2 - 1, 1, r * 2, r * 2, 270, 90);
                        g.DrawLine(pen, 1, r + 1, 1, btn.Height);
                        g.DrawLine(pen, btn.Width - 1, r + 1, btn.Width - 1, btn.Height);

                        if (!isSelected)
                        {
                            g.DrawLine(pen, 1, btn.Height, btn.Width - 1, btn.Height);
                        }
                    }
                };

                EventHandler clickHandler = (s, e) =>
                {
                    selectedTab = index;

                    for (int j = 0; j < 3; j++)
                    {
                        pages[j].Visible = (j == index);
                    }

                    foreach (Control c in tabBar.Controls)
                    {
                        c.Invalidate();
                    }

                    contentBox.Invalidate();
                    onTabChanged?.Invoke();
                };

                tabButton.Click += clickHandler;
                tabLabel.Click += clickHandler;

                tabButton.Controls.Add(tabLabel);
                tabBar.Controls.Add(tabButton);
            }

            mainContainer.Controls.Add(tabBar);
            mainContainer.Controls.Add(contentBox);
            form.Controls.Add(mainContainer);
            mainContainer.BringToFront();

            form.Resize += (s, e) =>
            {
                mainContainer.Location = new Point(25, 50);
                mainContainer.Size = new Size(form.ClientSize.Width - 50, form.ClientSize.Height - 75);

                tabBar.Size = new Size(mainContainer.Width, 50);
                contentBox.Location = new Point(0, 49);
                contentBox.Size = new Size(mainContainer.Width, mainContainer.Height - 49);

                int newTabWidth = tabBar.Width / 3;
                for (int i = 0; i < 3; i++)
                {
                    Control tab = tabBar.Controls["tabButton" + i];
                    if (tab != null)
                    {
                        int xPos = (i * newTabWidth) - i;

                        int actualWidth;
                        if (i == 0)
                            actualWidth = newTabWidth + 1;
                        else if (i == 1)
                            actualWidth = newTabWidth + 2;
                        else
                            actualWidth = (tabBar.Width - ((newTabWidth * 2) - 2)) - 1;

                        tab.Location = new Point(xPos, 0);
                        tab.Width = actualWidth;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    Control page = contentBox.Controls["customPage" + i];
                    if (page != null)
                    {
                        page.Size = new Size(contentBox.Width - 40, contentBox.Height - 40);
                    }
                }

                contentBox.Invalidate();
            };
        }

        public static void RegisterTabChangedCallback(Action callback)
        {
            onTabChanged = callback;
        }

        public static void SwitchToTab(int tabIndex)
        {
            if (cachedPages == null || cachedTabBar == null || cachedContentBox == null)
                return;

            selectedTab = tabIndex;

            for (int j = 0; j < cachedPages.Length; j++)
            {
                cachedPages[j].Visible = (j == tabIndex);
            }

            foreach (Control c in cachedTabBar.Controls)
            {
                c.Invalidate();
            }

            cachedContentBox.Invalidate();
            onTabChanged?.Invoke();
        }

        public static int GetCurrentTab()
        {
            return selectedTab;
        }

        public static void SetMemberPanelVisible(Form form, bool visible)
        {
            foreach (Control c in form.Controls)
            {
                if (c is Panel mainContainer && mainContainer.Name == "mainContainer")
                {
                    foreach (Control tc in mainContainer.Controls)
                    {
                        if (tc is Panel contentBox && contentBox.Name == "contentBox")
                        {
                            Panel page = contentBox.Controls["customPage0"] as Panel;
                            if (page != null)
                            {
                                foreach (Control ctrl in page.Controls)
                                {
                                    if (ctrl.Name == "MemberFLPBorder")
                                    {
                                        ctrl.Visible = visible;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void StyleAllControls(Form form)
        {
            Panel mainContainer = null;
            foreach (Control c in form.Controls)
            {
                if (c is Panel p && p.Name == "mainContainer")
                {
                    mainContainer = p;
                    break;
                }
            }
            if (mainContainer == null) return;

            Panel contentBox = null;
            foreach (Control c in mainContainer.Controls)
            {
                if (c is Panel p && p.Name == "contentBox")
                {
                    contentBox = p;
                    break;
                }
            }
            if (contentBox == null) return;

            for (int i = 0; i < 3; i++)
            {
                Panel page = contentBox.Controls["customPage" + i] as Panel;
                if (page != null)
                {
                    StylePageControls(page);
                }
            }
        }

        private static void StylePageControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is TextBox tb)
                {
                    StyleTextBox(tb);
                }
                else if (ctrl is Button btn)
                {
                    if (btn.Name == "ChangeProfileImageButton")
                    {
                        StyleCameraButton(btn);
                    }
                    else
                    {
                        StyleButton(btn);
                    }
                }
                else if (ctrl is Label lbl)
                {
                    StyleLabel(lbl);
                }
                else if (ctrl is PictureBox pb)
                {
                    StylePictureBox(pb);
                }
                else if (ctrl is Panel p && p.HasChildren)
                {
                    StylePageControls(p);
                }
            }
        }

        private static void StyleTextBox(TextBox textBox)
        {
            if (textBox.Parent is Panel && textBox.Parent.Tag?.ToString() == textBox.Name)
                return;

            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("맑은 고딕", 10F);
            textBox.ForeColor = ColorSchemes.DarkOlive;

            if (textBox.Name == "PwBox")
            {
                textBox.PasswordChar = '●';
            }

            bool isReadOnly = textBox.ReadOnly;
            textBox.BackColor = isReadOnly ? ColorSchemes.LightOlive : Color.White;
            textBox.Cursor = isReadOnly ? Cursors.Default : Cursors.IBeam;
            textBox.TabStop = !isReadOnly;

            int originalWidth = textBox.Width;
            int originalHeight = textBox.Height;
            Point originalLocation = textBox.Location;

            if (textBox.Name == "IdBox" || textBox.Name == "PwBox" || textBox.Name == "NameBox" ||
                textBox.Name == "AddressBox" || textBox.Name == "DeptBox")
            {
                originalWidth = 320;
            }
            else if (textBox.Name == "ZipCodeBox")
            {
                originalWidth = 210;
            }
            else if (textBox.Name == "NicknameBox")
            {
                originalWidth = 210;
            }
            else if (textBox.Name == "StatusBox")
            {
                originalWidth = 320;
                if (textBox.Multiline)
                {
                    originalHeight = 70;
                }
            }

            int wrapperWidth = originalWidth;
            int wrapperHeight = textBox.Multiline ? originalHeight : 38;

            Panel wrapper = new Panel
            {
                Size = new Size(wrapperWidth, wrapperHeight),
                Location = originalLocation,
                BackColor = textBox.BackColor,
                Tag = textBox.Name,
                Cursor = textBox.Cursor
            };

            textBox.Location = new Point(10, textBox.Multiline ? 10 : 10);
            textBox.Width = wrapperWidth - 20;
            if (textBox.Multiline)
            {
                textBox.Height = wrapperHeight - 20;
            }

            var parent = textBox.Parent;
            int tabIndex = parent.Controls.GetChildIndex(textBox);

            parent.Controls.Remove(textBox);
            wrapper.Controls.Add(textBox);
            parent.Controls.Add(wrapper);
            parent.Controls.SetChildIndex(wrapper, tabIndex);

            GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, wrapperWidth, wrapperHeight), 8);
            wrapper.Region = new Region(path);

            wrapper.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, wrapperWidth - 1, wrapperHeight - 1), 8))
                {
                    using (Pen pen = new Pen(ColorSchemes.SageGreen, 0.8f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };
        }

        private static void StyleButton(Button button)
        {
            if (button.Name == "EditMButton" || button.Name == "AddMulProfileButton" || button.Name == "EditButton")
            {
                button.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
                button.ForeColor = ColorSchemes.DarkOlive;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseDownBackColor = ColorSchemes.SageGreen;
                button.FlatAppearance.MouseOverBackColor = ColorSchemes.SageGreen;
                button.Cursor = Cursors.Hand;
                button.Height = 30;
                button.TabStop = false;
                button.BackColor = ColorSchemes.LightOlive;

                button.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Rectangle rect = new Rectangle(0, 0, button.Width, button.Height);

                    using (GraphicsPath path = GetRoundedRectangle(rect, 8))
                    {
                        button.Region = new Region(path);
                    }

                    using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, button.Width - 1, button.Height - 1), 8))
                    {
                        using (Pen pen = new Pen(ColorSchemes.SageGreen, 1.5f))
                        {
                            e.Graphics.DrawPath(pen, borderPath);
                        }
                    }
                };

                button.Resize += (s, e) => button.Invalidate();
                button.MouseEnter += (s, e) => button.BackColor = ColorSchemes.SageGreen;
                button.MouseLeave += (s, e) => button.BackColor = ColorSchemes.LightOlive;
            }
            else
            {
                button.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
                button.ForeColor = Color.White;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseDownBackColor = ColorSchemes.DarkOlive;
                button.FlatAppearance.MouseOverBackColor = ColorSchemes.DarkOlive;
                button.Cursor = Cursors.Hand;
                button.Height = 38;
                button.TabStop = false;
                button.BackColor = ColorSchemes.SageGreen;

                button.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Rectangle rect = new Rectangle(0, 0, button.Width, button.Height);
                    using (GraphicsPath path = GetRoundedRectangle(rect, 8))
                    {
                        button.Region = new Region(path);
                    }
                };

                button.Resize += (s, e) => button.Invalidate();
                button.GotFocus += (s, e) =>
                {
                    button.BackColor = ColorSchemes.SageGreen;
                    button.Invalidate();
                };
                button.LostFocus += (s, e) =>
                {
                    button.BackColor = ColorSchemes.SageGreen;
                    button.Invalidate();
                };
                button.MouseEnter += (s, e) => button.BackColor = ColorSchemes.DarkOlive;
                button.MouseLeave += (s, e) => button.BackColor = ColorSchemes.SageGreen;
            }
        }

        private static void StyleLabel(Label label)
        {
            label.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            label.ForeColor = ColorSchemes.DarkOlive;
            label.BackColor = Color.Transparent;
        }

        private static void StylePictureBox(PictureBox pictureBox)
        {
            if (pictureBox.Name == "ProfileImagePBox")
            {
                pictureBox.Size = new Size(100, 100);

                Panel bgPanel = new Panel
                {
                    Size = new Size(100, 100),
                    Location = pictureBox.Location,
                    BackColor = Color.White,
                    Tag = "ProfileBg"
                };

                GraphicsPath bgPath = GetRoundedRectangle(new Rectangle(0, 0, 100, 100), 15);
                bgPanel.Region = new Region(bgPath);

                var parent = pictureBox.Parent;
                parent.Controls.Add(bgPanel);
                bgPanel.SendToBack();

                GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, 100, 100), 15);
                pictureBox.Region = new Region(path);
                pictureBox.BackColor = Color.Transparent;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                pictureBox.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, 99, 99), 15))
                    {
                        using (Pen pen = new Pen(ColorSchemes.SageGreen, 2.5f))
                        {
                            e.Graphics.DrawPath(pen, borderPath);
                        }
                    }
                };
            }
            else if (pictureBox.Name == "ProfileImageMBox")
            {
                pictureBox.Size = new Size(50, 50);

                GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, 50, 50), 10);
                pictureBox.Region = new Region(path);
                pictureBox.BackColor = Color.Transparent;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                pictureBox.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, 49, 49), 10))
                    {
                        using (Pen pen = new Pen(ColorSchemes.SageGreen, 2f))
                        {
                            e.Graphics.DrawPath(pen, borderPath);
                        }
                    }
                };
            }
        }

        private static void StyleCameraButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.White;
            button.Cursor = Cursors.Hand;
            button.Size = new Size(36, 36);

            Image originalImage = null;
            Image resizedImage = null;

            if (button.BackgroundImage != null)
            {
                originalImage = button.BackgroundImage;
                int newSize = 20;
                Bitmap bitmap = new Bitmap(newSize, newSize);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawImage(originalImage, 0, 0, newSize, newSize);
                }

                resizedImage = bitmap;
                button.BackgroundImage = null;
            }

            GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, button.Width, button.Height), 8);
            button.Region = new Region(path);

            button.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, button.Width, button.Height), 8))
                {
                    using (SolidBrush bgBrush = new SolidBrush(button.BackColor))
                    {
                        e.Graphics.FillPath(bgBrush, borderPath);
                    }

                    if (resizedImage != null)
                    {
                        int imgX = (button.Width - resizedImage.Width) / 2;
                        int imgY = (button.Height - resizedImage.Height) / 2;
                        e.Graphics.DrawImage(resizedImage, imgX, imgY);
                    }

                    using (Pen pen = new Pen(ColorSchemes.SageGreen, 2f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };

            button.MouseEnter += (s, e) => button.BackColor = Color.White;
            button.MouseLeave += (s, e) => button.BackColor = Color.White;
        }

        private static void AdjustLayout(Form form)
        {
            Panel mainContainer = null;
            foreach (Control c in form.Controls)
            {
                if (c is Panel p && p.Name == "mainContainer")
                {
                    mainContainer = p;
                    break;
                }
            }
            if (mainContainer == null) return;

            Panel contentBox = null;
            foreach (Control c in mainContainer.Controls)
            {
                if (c is Panel p && p.Name == "contentBox")
                {
                    contentBox = p;
                    break;
                }
            }
            if (contentBox == null) return;

            for (int i = 0; i < 3; i++)
            {
                Panel page = contentBox.Controls["customPage" + i] as Panel;
                if (page != null)
                {
                    if (i == 0)
                        AdjustDefaultProfilePage(page);
                    else if (i == 1)
                        AdjustMulProfilePage(page);
                    else
                        AdjustUserInfoPage(page);
                }
            }
        }

        private static void AdjustDefaultProfilePage(Panel page)
        {
            int centerX = (page.Width - 320) / 2;
            int startY = 20;

            Panel borderPanel = null;

            foreach (Control control in page.Controls)
            {
                if (control.Name == "ProfileImagePBox")
                {
                    control.Location = new Point(centerX, startY);
                    control.Size = new Size(100, 100);
                }
                else if (control is Panel && control.Tag?.ToString() == "ProfileBg")
                {
                    control.Location = new Point(centerX, startY);
                    control.Size = new Size(100, 100);
                    control.SendToBack();
                }
                else if (control.Name == "ChangeProfileImageButton")
                {
                    control.Location = new Point(centerX + 65, startY + 65);
                    control.Size = new Size(36, 36);
                    control.BringToFront();
                }
                else if (control.Name == "NicknameBox" || (control is Panel && control.Tag?.ToString() == "NicknameBox"))
                {
                    control.Location = new Point(centerX + 110, startY + 20);
                    if (control is Panel) control.Width = 210;
                }
                else if (control.Name == "DeptLabel")
                {
                    control.Location = new Point(centerX + 110, startY + 70);
                }
                else if (control.Name == "label1")
                {
                    control.Location = new Point(centerX, startY + 130);
                }
                else if (control.Name == "StatusBox" || (control is Panel && control.Tag?.ToString() == "StatusBox"))
                {
                    control.Location = new Point(centerX, startY + 150);
                    if (control is Panel)
                    {
                        control.Width = 320;
                        control.Height = 70;
                    }
                }
                else if (control.Name == "label2")
                {
                    control.Location = new Point(centerX, startY + 240);
                }
                else if (control.Name == "EditMButton")
                {
                    control.Location = new Point(centerX + 270, startY + 235);
                    control.Width = 50;
                }
                else if (control.Name == "SavePButton")
                {
                    control.Location = new Point(centerX, startY + 420);
                    control.Width = 210;
                }
                else if (control.Name == "CancelPButton")
                {
                    control.Location = new Point(centerX + 220, startY + 420);
                    control.Width = 100;
                }
            }

            foreach (Control control in page.Controls)
            {
                if (control.Name == "MemberFLP" && control is FlowLayoutPanel flp)
                {
                    borderPanel = new Panel
                    {
                        Location = new Point(centerX, startY + 275),
                        Size = new Size(320, 130),
                        BackColor = Color.White,
                        Name = "MemberFLPBorder"
                    };

                    borderPanel.Paint += (s, e) =>
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                        int w = borderPanel.Width - 1;
                        int h = borderPanel.Height - 1;
                        int radius = 8;

                        using (GraphicsPath bgPath = new GraphicsPath())
                        {
                            bgPath.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                            bgPath.AddArc(w - radius * 2, 0, radius * 2, radius * 2, 270, 90);
                            bgPath.AddArc(w - radius * 2, h - radius * 2, radius * 2, radius * 2, 0, 90);
                            bgPath.AddArc(0, h - radius * 2, radius * 2, radius * 2, 90, 90);
                            bgPath.CloseFigure();

                            using (SolidBrush bgBrush = new SolidBrush(Color.White))
                            {
                                e.Graphics.FillPath(bgBrush, bgPath);
                            }
                        }

                        using (GraphicsPath borderPath = new GraphicsPath())
                        {
                            borderPath.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                            borderPath.AddArc(w - radius * 2, 0, radius * 2, radius * 2, 270, 90);
                            borderPath.AddArc(w - radius * 2, h - radius * 2, radius * 2, radius * 2, 0, 90);
                            borderPath.AddArc(0, h - radius * 2, radius * 2, radius * 2, 90, 90);
                            borderPath.CloseFigure();

                            using (Pen pen = new Pen(ColorSchemes.SageGreen, 0.8f))
                            {
                                e.Graphics.DrawPath(pen, borderPath);
                            }
                        }
                    };

                    flp.Location = new Point(4, 4);
                    flp.Size = new Size(312, 122);
                    flp.BackColor = Color.White;
                    flp.FlowDirection = FlowDirection.TopDown;
                    flp.WrapContents = false;
                    flp.Padding = new Padding(5, 5, 5, 5);
                    flp.AutoScroll = true;

                    flp.HorizontalScroll.Enabled = false;
                    flp.HorizontalScroll.Visible = false;
                    flp.HorizontalScroll.Maximum = 0;

                    foreach (Control child in flp.Controls)
                    {
                        StyleDynamicControl(child, "MemberFLP");
                    }

                    flp.ControlAdded += (s, e) =>
                    {
                        StyleDynamicControl(e.Control, "MemberFLP");
                    };

                    page.Controls.Remove(flp);
                    borderPanel.Controls.Add(flp);

                    Panel scrollCover = new Panel
                    {
                        Location = new Point(300, 2),
                        Size = new Size(16, 126),
                        BackColor = Color.White,
                        Name = "ScrollCover"
                    };
                    borderPanel.Controls.Add(scrollCover);
                    scrollCover.BringToFront();

                    page.Controls.Add(borderPanel);

                    break;
                }
            }
        }

        private static void AdjustMulProfilePage(Panel page)
        {
            int centerX = (page.Width - 320) / 2;
            int startY = 20;

            foreach (Control control in page.Controls)
            {
                if (control.Name == "label7")
                {
                    control.Location = new Point(centerX, startY);
                    control.Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
                }
                else if (control.Name == "AddMulProfileButton")
                {
                    control.Location = new Point(centerX + 270, startY - 5);
                    control.Width = 50;
                }
                else if (control.Name == "ProfileImageMBox")
                {
                    control.Location = new Point(centerX, startY + 40);
                    control.Size = new Size(50, 50);
                }
                else if (control.Name == "NicknameLabel")
                {
                    control.Location = new Point(centerX + 60, startY + 55);
                    control.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
                }
                else if (control.Name == "EditButton")
                {
                    control.Location = new Point(centerX + 270, startY + 50);
                    control.Width = 50;
                }
                else if (control.Name == "ProfileFLP")
                {
                    control.Location = new Point(centerX - 5, startY + 100);
                    control.Size = new Size(355, 355);
                    control.BackColor = Color.White;

                    if (control is FlowLayoutPanel flp)
                    {
                        flp.AutoSize = false;
                        flp.AutoScroll = true;
                        flp.Padding = new Padding(5, 5, 20, 5);
                        flp.WrapContents = false;
                        flp.FlowDirection = FlowDirection.TopDown;

                        flp.HorizontalScroll.Enabled = false;
                        flp.HorizontalScroll.Visible = false;
                        flp.HorizontalScroll.Maximum = 0;

                        foreach (Control child in flp.Controls)
                        {
                            StyleDynamicControl(child, "ProfileFLP");
                        }

                        flp.ControlAdded += (s, e) =>
                        {
                            StyleDynamicControl(e.Control, "ProfileFLP");
                        };
                    }
                }
            }
        }

        private static void AdjustUserInfoPage(Panel page)
        {
            int centerX = (page.Width - 320) / 2;
            int startY = 20;

            foreach (Control control in page.Controls)
            {
                if (control.Name == "label8")
                {
                    control.Location = new Point(centerX, startY);
                }
                else if (control.Name == "IdBox" || (control is Panel && control.Tag?.ToString() == "IdBox"))
                {
                    control.Location = new Point(centerX, startY + 20);
                    if (control is Panel) control.Width = 320;
                }
                else if (control.Name == "label6")
                {
                    control.Location = new Point(centerX, startY + 70);
                }
                else if (control.Name == "PwBox" || (control is Panel && control.Tag?.ToString() == "PwBox"))
                {
                    control.Location = new Point(centerX, startY + 90);
                    if (control is Panel) control.Width = 320;
                }
                else if (control.Name == "label3")
                {
                    control.Location = new Point(centerX, startY + 140);
                }
                else if (control.Name == "NameBox" || (control is Panel && control.Tag?.ToString() == "NameBox"))
                {
                    control.Location = new Point(centerX, startY + 160);
                    if (control is Panel) control.Width = 320;
                }
                else if (control.Name == "label4")
                {
                    control.Location = new Point(centerX, startY + 210);
                }
                else if (control.Name == "ZipCodeBox" || (control is Panel && control.Tag?.ToString() == "ZipCodeBox"))
                {
                    control.Location = new Point(centerX, startY + 230);
                    if (control is Panel) control.Width = 210;
                }
                else if (control.Name == "SearchAddressButton")
                {
                    control.Location = new Point(centerX + 220, startY + 231);
                    control.Width = 100;
                    control.BringToFront();
                }
                else if (control.Name == "AddressBox" || (control is Panel && control.Tag?.ToString() == "AddressBox"))
                {
                    control.Location = new Point(centerX, startY + 280);
                    if (control is Panel)
                    {
                        control.Width = 320;
                        control.Height = 38;
                    }
                }
                else if (control.Name == "label5")
                {
                    control.Location = new Point(centerX, startY + 330);
                }
                else if (control.Name == "DeptBox" || (control is Panel && control.Tag?.ToString() == "DeptBox"))
                {
                    control.Location = new Point(centerX, startY + 350);
                    if (control is Panel) control.Width = 320;
                }
                else if (control.Name == "SaveIButton")
                {
                    control.Location = new Point(centerX, startY + 420);
                    control.Width = 210;
                }
                else if (control.Name == "CancleIButton")
                {
                    control.Location = new Point(centerX + 220, startY + 420);
                    control.Width = 100;
                }
            }
        }
        private static void StyleDynamicControl(Control control, string parentName = "")
        {
            if (control is Panel panel)
            {
                panel.BackColor = Color.White;
                panel.Margin = new Padding(0, 0, 0, 8);

                if (parentName == "ProfileFLP")
                {
                    panel.Size = new Size(330, 60);
                }
                else if (parentName == "MemberFLP")
                {
                    panel.Size = new Size(295, 50);
                }

                foreach (Control child in panel.Controls)
                {
                    if (child is PictureBox pb)
                    {
                        if (parentName == "ProfileFLP")
                        {
                            pb.Location = new Point(0, 5);
                            pb.Size = new Size(50, 50);
                        }
                        else if (parentName == "MemberFLP")
                        {
                            pb.Location = new Point(5, 5);
                            pb.Size = new Size(40, 40);
                        }

                        GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, pb.Width, pb.Height), 10);
                        pb.Region = new Region(path);
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;

                        pb.Paint += (s, e) =>
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            using (GraphicsPath imgBorder = GetRoundedRectangle(new Rectangle(0, 0, pb.Width - 1, pb.Height - 1), 10))
                            {
                                using (Pen pen = new Pen(ColorSchemes.SageGreen, 2f))
                                {
                                    e.Graphics.DrawPath(pen, imgBorder);
                                }
                            }
                        };
                    }
                    else if (child is Label lbl)
                    {
                        if (parentName == "ProfileFLP")
                        {
                            lbl.Location = new Point(60, 22);
                            lbl.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
                            lbl.ForeColor = ColorSchemes.DarkOlive;
                            lbl.BackColor = Color.Transparent;
                            lbl.AutoSize = true;
                        }
                        else if (parentName == "MemberFLP")
                        {
                            lbl.Location = new Point(55, 17);
                            lbl.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
                            lbl.ForeColor = ColorSchemes.DarkOlive;
                            lbl.BackColor = Color.Transparent;
                            lbl.AutoSize = false;
                            lbl.Size = new Size(175, 20);
                            lbl.TextAlign = ContentAlignment.MiddleLeft;
                        }
                    }
                    else if (child is Button btn)
                    {
                        if (btn.Text == "관리")
                        {
                            if (parentName == "ProfileFLP")
                            {
                                btn.Location = new Point(215, 17);
                                btn.Width = 50;
                                btn.Height = 30;
                            }
                        }
                        else if (btn.Text == "삭제")
                        {
                            if (parentName == "ProfileFLP")
                            {
                                btn.Location = new Point(270, 17);
                                btn.Width = 50;
                                btn.Height = 30;
                            }
                            else if (parentName == "MemberFLP")
                            {
                                btn.Location = new Point(240, 13);
                                btn.Width = 50;
                                btn.Height = 26;
                            }
                        }

                        btn.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
                        btn.ForeColor = ColorSchemes.DarkOlive;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 0;
                        btn.FlatAppearance.MouseDownBackColor = ColorSchemes.SageGreen;
                        btn.FlatAppearance.MouseOverBackColor = ColorSchemes.SageGreen;
                        btn.Cursor = Cursors.Hand;
                        btn.TabStop = false;
                        btn.BackColor = ColorSchemes.LightOlive;

                        btn.Paint += (s, e) =>
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            Rectangle rect = new Rectangle(0, 0, btn.Width, btn.Height);

                            using (GraphicsPath path = GetRoundedRectangle(rect, 8))
                            {
                                btn.Region = new Region(path);
                            }

                            using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, btn.Width - 1, btn.Height - 1), 8))
                            {
                                using (Pen pen = new Pen(ColorSchemes.SageGreen, 1.5f))
                                {
                                    e.Graphics.DrawPath(pen, borderPath);
                                }
                            }
                        };

                        btn.Resize += (s, e) => btn.Invalidate();
                        btn.MouseEnter += (s, e) => btn.BackColor = ColorSchemes.SageGreen;
                        btn.MouseLeave += (s, e) => btn.BackColor = ColorSchemes.LightOlive;
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
