using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using leehaeun.Themes;

namespace leehaeun.UIHelpers
{
    public static class SignUpFormUIHelper
    {
        /// <summary>
        /// 회원가입 폼 스타일 적용
        /// </summary>
        public static void ApplyStyles(SignUpForm form)
        {
            form.BackColor = ColorSchemes.Ivory;
            form.Size = new Size(450, 700);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.None;

            ApplyRoundedCorners(form, 15);
            CreateCustomTitleBar(form);
            StyleAllControls(form);
            AdjustLayout(form);
        }

        /// <summary>
        /// 폼 둥근 모서리
        /// </summary>
        private static void ApplyRoundedCorners(Form form, int radius)
        {
            form.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, form.Width, form.Height, radius, radius));
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// 커스텀 타이틀바 생성
        /// </summary>
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

        /// <summary>
        /// 모든 컨트롤 스타일 적용
        /// </summary>
        private static void StyleAllControls(Form form)
        {
            var textBoxes = new System.Collections.Generic.List<TextBox>();
            var comboBoxes = new System.Collections.Generic.List<ComboBox>();

            foreach (Control control in form.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBoxes.Add(textBox);
                }
                else if (control is ComboBox comboBox)
                {
                    comboBoxes.Add(comboBox);
                }
            }

            foreach (var textBox in textBoxes)
            {
                StyleTextBox(textBox, form);
            }

            foreach (var comboBox in comboBoxes)
            {
                StyleComboBox(comboBox, form);
            }

            foreach (Control control in form.Controls)
            {
                if (control is Button button)
                {
                    if (button.Name == "ChangeProfileButton")
                    {
                        StyleCameraButton(button);
                    }
                    else if (button.Name != "closeButton")
                    {
                        StyleButton(button);
                    }
                }
                else if (control is Label label)
                {
                    StyleLabel(label);
                }
                else if (control is PictureBox pictureBox)
                {
                    StylePictureBox(pictureBox);
                }
            }
        }

        /// <summary>
        /// TextBox 스타일
        /// </summary>
        private static void StyleTextBox(TextBox textBox, Form form)
        {
            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("맑은 고딕", 10F);
            textBox.ForeColor = ColorSchemes.DarkOlive;

            if (textBox.Name == "PwBox")
            {
                textBox.PasswordChar = '●';
            }

            bool isReadOnly = textBox.Name == "ZipCodeBox" || textBox.Name == "AddressBox";
            textBox.BackColor = isReadOnly ? ColorSchemes.LightOlive : Color.White;
            textBox.ReadOnly = isReadOnly;
            textBox.Cursor = isReadOnly ? Cursors.Default : Cursors.IBeam;
            textBox.TabStop = !isReadOnly;

            int targetWidth = 0;
            if (textBox.Name == "IdBox" || textBox.Name == "ZipCodeBox")
            {
                targetWidth = 210;
            }
            else if (textBox.Name == "NickNameBox")
            {
                targetWidth = 210; // 220 → 210
            }
            else
            {
                targetWidth = 320;
            }

            Panel wrapper = new Panel
            {
                Size = new Size(targetWidth, 38),
                Location = textBox.Location,
                BackColor = isReadOnly ? ColorSchemes.LightOlive : Color.White,
                Tag = textBox.Name,
                Cursor = isReadOnly ? Cursors.Default : Cursors.IBeam
            };

            textBox.Location = new Point(10, 10);
            textBox.Width = targetWidth - 20;
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
                    using (Pen pen = new Pen(ColorSchemes.SageGreen, 0.8f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };
        }

        /// <summary>
        /// ComboBox 스타일
        /// </summary>
        private static void StyleComboBox(ComboBox comboBox, Form form)
        {
            if (comboBox.Items.Count > 0 && comboBox.SelectedIndex < 0)
            {
                comboBox.SelectedIndex = 0;
            }

            string GetDisplayText()
            {
                if (comboBox.SelectedItem != null)
                {
                    if (!string.IsNullOrEmpty(comboBox.DisplayMember))
                    {
                        var item = comboBox.SelectedItem;
                        var property = item.GetType().GetProperty(comboBox.DisplayMember);
                        if (property != null)
                        {
                            return property.GetValue(item)?.ToString() ?? "선택하세요";
                        }
                    }
                    else
                    {
                        return comboBox.SelectedItem.ToString();
                    }
                }
                else if (comboBox.Items.Count > 0)
                {
                    return comboBox.GetItemText(comboBox.Items[0]);
                }
                return "선택하세요";
            }

            string currentValue = GetDisplayText();

            comboBox.Visible = false;

            Panel wrapper = new Panel
            {
                Size = new Size(210, 38), // 220 → 210
                Location = comboBox.Location,
                BackColor = Color.White,
                Tag = comboBox.Name,
                Cursor = Cursors.Hand
            };

            Label displayLabel = new Label
            {
                Text = currentValue,
                Font = new Font("맑은 고딕", 10F),
                ForeColor = ColorSchemes.DarkOlive,
                BackColor = Color.Transparent,
                Location = new Point(10, 10),
                AutoSize = false,
                Size = new Size(170, 18), // 180 → 170
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };

            var parent = comboBox.Parent;
            parent.Controls.Add(wrapper);
            wrapper.Controls.Add(displayLabel);

            GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, wrapper.Width, wrapper.Height), 8);
            wrapper.Region = new Region(path);

            wrapper.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, wrapper.Width - 1, wrapper.Height - 1), 8))
                {
                    using (Pen pen = new Pen(ColorSchemes.SageGreen, 0.8f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }

                int arrowX = wrapper.Width - 18;
                int arrowY = wrapper.Height / 2;

                Point[] arrowPoints = new Point[]
                {
            new Point(arrowX - 4, arrowY - 2),
            new Point(arrowX + 4, arrowY - 2),
            new Point(arrowX, arrowY + 3)
                };

                using (SolidBrush brush = new SolidBrush(ColorSchemes.SageGreen))
                {
                    e.Graphics.FillPolygon(brush, arrowPoints);
                }
            };

            EventHandler showMenu = (s, e) =>
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.BackColor = Color.White;
                menu.Font = new Font("맑은 고딕", 10F);
                menu.Renderer = new CustomMenuRenderer();
                menu.ShowImageMargin = false;
                menu.AutoSize = false;
                menu.Width = wrapper.Width;
                menu.Padding = new Padding(0);

                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    var item = comboBox.Items[i];
                    string displayText = comboBox.GetItemText(item);

                    ToolStripMenuItem menuItem = new ToolStripMenuItem(displayText);
                    menuItem.ForeColor = ColorSchemes.DarkOlive;
                    menuItem.Padding = new Padding(12, 0, 12, 0);
                    menuItem.AutoSize = false;
                    menuItem.Width = wrapper.Width;
                    menuItem.Height = 38;
                    menuItem.TextAlign = ContentAlignment.MiddleLeft;

                    int index = i;
                    menuItem.Click += (sender, args) =>
                    {
                        comboBox.SelectedIndex = index;
                        displayLabel.Text = displayText;
                        menu.Close();
                    };

                    menu.Items.Add(menuItem);
                }

                Point location = wrapper.PointToScreen(new Point(0, wrapper.Height + 1));

                int menuHeight = menu.Items.Count * 38 + 4;
                menu.Height = menuHeight;
                menu.MaximumSize = new Size(wrapper.Width, menuHeight);
                menu.Size = new Size(wrapper.Width, menuHeight);

                GraphicsPath menuPath = GetRoundedRectangle(new Rectangle(0, 0, menu.Width, menuHeight), 8);
                menu.Region = new Region(menuPath);

                menu.Show(location);
            };

            comboBox.SelectedIndexChanged += (s, e) =>
            {
                displayLabel.Text = GetDisplayText();
            };

            wrapper.Click += showMenu;
            displayLabel.Click += showMenu;
        }

        /// <summary>
        /// 커스텀 메뉴 렌더러
        /// </summary>
        private class CustomMenuRenderer : ToolStripProfessionalRenderer
        {
            public CustomMenuRenderer() : base(new CustomColorTable())
            {
                RoundedEdges = false;
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                using (Pen pen = new Pen(ColorSchemes.SageGreen, 1f))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1), 8))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Selected)
                {
                    // 호버 시 완전히 채우기 (좌우 1px만 여백)
                    Rectangle rect = new Rectangle(1, 0, e.Item.Width - 2, e.Item.Height);
                    using (SolidBrush brush = new SolidBrush(ColorSchemes.LightOlive))
                    {
                        e.Graphics.FillRectangle(brush, rect);
                    }
                }
                else
                {
                    // 기본 배경
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillRectangle(brush, new Rectangle(0, 0, e.Item.Width, e.Item.Height));
                    }
                }
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = ColorSchemes.DarkOlive;
                e.TextFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.Left;

                // 텍스트 영역 계산 (수직 중앙)
                Rectangle textRect = new Rectangle(
                    e.TextRectangle.X,
                    (e.Item.Height - e.TextRectangle.Height) / 2,
                    e.TextRectangle.Width,
                    e.TextRectangle.Height
                );

                TextRenderer.DrawText(
                    e.Graphics,
                    e.Text,
                    e.TextFont,
                    textRect,
                    e.TextColor,
                    e.TextFormat
                );
            }

            protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
            {
                // 이미지 여백 렌더링 안 함
            }

            protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
            {
                // 구분선 렌더링 안 함
            }
        }

        /// <summary>
        /// 커스텀 컬러 테이블
        /// </summary>
        private class CustomColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected => ColorSchemes.LightOlive;
            public override Color MenuItemSelectedGradientBegin => ColorSchemes.LightOlive;
            public override Color MenuItemSelectedGradientEnd => ColorSchemes.LightOlive;
            public override Color MenuItemBorder => Color.Transparent;
            public override Color MenuBorder => ColorSchemes.SageGreen;
            public override Color ImageMarginGradientBegin => Color.White;
            public override Color ImageMarginGradientMiddle => Color.White;
            public override Color ImageMarginGradientEnd => Color.White;
            public override Color ToolStripDropDownBackground => Color.White;
        }

        /// <summary>
        /// Button 스타일
        /// </summary>
        private static void StyleButton(Button button)
        {
            button.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = ColorSchemes.DarkOlive; // 클릭 시 색상
            button.FlatAppearance.MouseOverBackColor = ColorSchemes.DarkOlive; // 호버 시 색상
            button.Cursor = Cursors.Hand;
            button.Height = 38;
            button.TabStop = false; // Tab 키로 포커스 받지 않음

            if (button.Name == "SignUpButton")
            {
                button.BackColor = ColorSchemes.SageGreen;
                button.Height = 45;
            }
            else
            {
                button.BackColor = ColorSchemes.SageGreen;
            }

            // 둥근 모서리 적용
            button.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, button.Width, button.Height);
                using (GraphicsPath path = GetRoundedRectangle(rect, 8))
                {
                    button.Region = new Region(path);
                }
            };

            button.Resize += (s, e) =>
            {
                button.Invalidate();
            };

            // Focus 이벤트 처리
            button.GotFocus += (s, e) =>
            {
                button.BackColor = ColorSchemes.SageGreen; // 포커스 받아도 색상 유지
                button.Invalidate();
            };

            button.LostFocus += (s, e) =>
            {
                button.BackColor = ColorSchemes.SageGreen; // 포커스 잃어도 색상 유지
                button.Invalidate();
            };

            button.MouseEnter += (s, e) => button.BackColor = ColorSchemes.DarkOlive;
            button.MouseLeave += (s, e) => button.BackColor = ColorSchemes.SageGreen;
        }

        /// <summary>
        /// Label 스타일
        /// </summary>
        private static void StyleLabel(Label label)
        {
            label.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            label.ForeColor = ColorSchemes.DarkOlive;
            label.BackColor = ColorSchemes.Ivory;
        }

        /// <summary>
        /// PictureBox 스타일
        /// </summary>
        private static void StylePictureBox(PictureBox pictureBox)
        {
            if (pictureBox.Name == "ProfileImageBox")
            {
                // 크기 조정 (네모)
                pictureBox.Size = new Size(100, 100);

                // 흰색 배경 패널
                Panel bgPanel = new Panel
                {
                    Size = new Size(100, 100),
                    Location = pictureBox.Location,
                    BackColor = Color.White,
                    Tag = "ProfileBg"
                };

                // 둥근 사각형
                GraphicsPath bgPath = GetRoundedRectangle(new Rectangle(0, 0, 100, 100), 15);
                bgPanel.Region = new Region(bgPath);

                var parent = pictureBox.Parent;
                parent.Controls.Add(bgPanel);
                bgPanel.SendToBack();

                // PictureBox 둥근 사각형
                GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, 100, 100), 15);
                pictureBox.Region = new Region(path);
                pictureBox.BackColor = Color.Transparent;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                // 균일한 테두리
                pictureBox.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, 100 - 1, 100 - 1), 15))
                    {
                        using (Pen pen = new Pen(ColorSchemes.SageGreen, 2.5f))
                        {
                            e.Graphics.DrawPath(pen, borderPath);
                        }
                    }
                };
            }
        }

        /// <summary>
        /// 카메라 버튼 스타일
        /// </summary>
        private static void StyleCameraButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.White;
            button.Cursor = Cursors.Hand;
            button.Size = new Size(36, 36);

            Image originalImage = null;
            Image resizedImage = null;

            // 이미지 크기 조정
            if (button.BackgroundImage != null)
            {
                originalImage = button.BackgroundImage;
                int newSize = 20;
                Bitmap bitmap = new Bitmap(newSize, newSize);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawImage(originalImage, 0, 0, newSize, newSize);
                }

                resizedImage = bitmap;
                button.BackgroundImage = null; // 기본 이미지 제거
            }

            // 둥근 사각형
            GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, button.Width, button.Height), 8);
            button.Region = new Region(path);

            bool isHovered = false;

            // 배경 + 이미지 + 테두리 그리기
            button.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, button.Width, button.Height), 8))
                {
                    // 배경 채우기
                    using (SolidBrush bgBrush = new SolidBrush(button.BackColor))
                    {
                        e.Graphics.FillPath(bgBrush, borderPath);
                    }

                    // 이미지 그리기 (중앙)
                    if (resizedImage != null)
                    {
                        int imgX = (button.Width - resizedImage.Width) / 2;
                        int imgY = (button.Height - resizedImage.Height) / 2;
                        e.Graphics.DrawImage(resizedImage, imgX, imgY);
                    }

                    // 테두리
                    using (Pen pen = new Pen(ColorSchemes.SageGreen, 2f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };
        }

        /// <summary>
        /// 레이아웃 조정
        /// </summary>
        private static void AdjustLayout(Form form)
        {
            int centerX = (form.ClientSize.Width - 320) / 2;
            int startY = 60;

            foreach (Control control in form.Controls)
            {
                if (control.Name == "ProfileImageBox")
                {
                    control.Location = new Point(centerX, startY + 32);
                    control.Size = new Size(100, 100);
                }
                else if (control is Panel && control.Tag?.ToString() == "ProfileBg")
                {
                    control.Location = new Point(centerX, startY + 32);
                    control.Size = new Size(100, 100);
                    control.SendToBack();
                }
                else if (control.Name == "ChangeProfileButton")
                {
                    control.Location = new Point(centerX + 65, startY + 97);
                    control.BringToFront();
                }
                else if (control.Name == "label3")
                {
                    control.Location = new Point(centerX + 110, startY + 10);
                }
                else if (control.Name == "NickNameBox" || (control is Panel && control.Tag?.ToString() == "NickNameBox"))
                {
                    control.Location = new Point(centerX + 110, startY + 30);
                }
                else if (control.Name == "label6")
                {
                    control.Location = new Point(centerX + 110, startY + 80);
                }
                else if (control.Name == "DeptBox" || (control is Panel && control.Tag?.ToString() == "DeptBox"))
                {
                    control.Location = new Point(centerX + 110, startY + 100);
                }
                else if (control.Name == "label1")
                {
                    control.Location = new Point(centerX, startY + 150);
                }
                else if (control.Name == "IdBox" || (control is Panel && control.Tag?.ToString() == "IdBox"))
                {
                    control.Location = new Point(centerX, startY + 170);
                }
                else if (control.Name == "CheckIDButton")
                {
                    control.Location = new Point(centerX + 220, startY + 170);
                    control.Width = 95; // 100 → 95
                }
                else if (control.Name == "IsDuplicate")
                {
                    control.Location = new Point(centerX + 55, startY + 150);
                }
                else if (control.Name == "label2")
                {
                    control.Location = new Point(centerX, startY + 220);
                }
                else if (control.Name == "PwBox" || (control is Panel && control.Tag?.ToString() == "PwBox"))
                {
                    control.Location = new Point(centerX, startY + 240);
                }
                else if (control.Name == "label4")
                {
                    control.Location = new Point(centerX, startY + 290);
                }
                else if (control.Name == "NameBox" || (control is Panel && control.Tag?.ToString() == "NameBox"))
                {
                    control.Location = new Point(centerX, startY + 310);
                }
                else if (control.Name == "label7")
                {
                    control.Location = new Point(centerX, startY + 360);
                }
                else if (control.Name == "ZipCodeBox" || (control is Panel && control.Tag?.ToString() == "ZipCodeBox"))
                {
                    control.Location = new Point(centerX, startY + 380);
                }
                else if (control.Name == "SearchAddressButton")
                {
                    control.Location = new Point(centerX + 220, startY + 380);
                    control.Width = 95; // 100 → 95
                }
                else if (control.Name == "AddressBox" || (control is Panel && control.Tag?.ToString() == "AddressBox"))
                {
                    control.Location = new Point(centerX, startY + 430);
                }
                else if (control.Name == "SignUpButton")
                {
                    control.Location = new Point(centerX, startY + 490);
                    control.Width = 320;
                }
            }
        }

        /// <summary>
        /// 둥근 사각형 경로 생성
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
