using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using leehaeun.Themes;

namespace leehaeun.UIHelpers
{
    public static class SearchUserFormUIHelper
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// 사용자 검색 폼 스타일 적용
        /// </summary>
        public static void ApplyStyles(SearchUserForm form)
        {
            form.BackColor = ColorSchemes.Ivory;
            form.FormBorderStyle = FormBorderStyle.None;

            form.Width = 500;
            form.Height = 450;
            form.MinimumSize = new Size(500, 450);
            form.MaximumSize = new Size(500, 450);

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
                BackColor = ColorSchemes.Ivory
            };

            Button closeButton = new Button
            {
                Name = "closeButton",
                Text = "✕",
                Font = new Font("맑은 고딕", 11F, FontStyle.Bold),
                Size = new Size(40, 30),
                Location = new Point(form.Width - 45, 5),
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

        private static void StyleControls(Form form)
        {
            foreach (Control control in form.Controls)
            {
                if (control is Panel panel && panel.Name == "UserScrollPanel")
                {
                    StyleScrollPanel(panel);
                }
                else if (control is Button btn && btn.Name != "closeButton")
                {
                    StyleButton(btn);
                }
            }
        }

        private static void StyleScrollPanel(Panel panel)
        {
            panel.BorderStyle = BorderStyle.None;
            panel.BackColor = Color.White;
            panel.AutoScroll = true;

            int wrapperWidth = 440;
            int wrapperHeight = 290;

            Panel wrapper = new Panel
            {
                Size = new Size(wrapperWidth, wrapperHeight),
                Location = panel.Location,
                BackColor = Color.White,
                Tag = "UserScrollPanelWrapper"
            };

            var parent = panel.Parent;
            int tabIndex = parent.Controls.GetChildIndex(panel);

            parent.Controls.Remove(panel);
            panel.Location = new Point(4, 4);
            panel.Size = new Size(wrapperWidth - 8, wrapperHeight - 8);
            wrapper.Controls.Add(panel);
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

            // 스크롤바 가리기용 흰색 패널
            Panel scrollCover = new Panel
            {
                BackColor = Color.White,
                Size = new Size(20, wrapperHeight - 8),
                Location = new Point(wrapperWidth - 24, 4)
            };
            wrapper.Controls.Add(scrollCover);
            scrollCover.BringToFront();
        }

        /// <summary>
        /// 사용자 아이템 추가
        /// </summary>
        public static Panel CreateUserItem(int userId, string name, string nickname, string dept, bool isChecked = false)
        {
            Panel itemPanel = new Panel
            {
                Width = 398,
                Height = 50,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 0),
                Tag = userId
            };

            // 체크박스 (PictureBox로 커스텀)
            PictureBox checkBox = new PictureBox
            {
                Location = new Point(12, 15),
                Size = new Size(20, 20),
                BackColor = Color.Transparent,
                Tag = new { UserId = userId, Checked = isChecked },
                Cursor = Cursors.Hand
            };

            // 체크박스 그리기
            checkBox.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                bool isChecked = false;
                if (checkBox.Tag is object tagObj)
                {
                    var tagType = tagObj.GetType();
                    var checkedProp = tagType.GetProperty("Checked");
                    if (checkedProp != null)
                    {
                        isChecked = (bool)checkedProp.GetValue(tagObj);
                    }
                }

                Rectangle checkRect = new Rectangle(0, 0, 19, 19);

                using (GraphicsPath checkPath = GetRoundedRectangle(checkRect, 5))
                {
                    if (isChecked)
                    {
                        using (SolidBrush fillBrush = new SolidBrush(ColorSchemes.SageGreen))
                        {
                            e.Graphics.FillPath(fillBrush, checkPath);
                        }

                        using (Pen checkPen = new Pen(Color.White, 2.5f))
                        {
                            checkPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                            checkPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                            e.Graphics.DrawLine(checkPen, 4, 10, 8, 14);
                            e.Graphics.DrawLine(checkPen, 8, 14, 15, 6);
                        }
                    }
                    else
                    {
                        using (SolidBrush bgBrush = new SolidBrush(Color.White))
                        {
                            e.Graphics.FillPath(bgBrush, checkPath);
                        }
                    }

                    using (Pen borderPen = new Pen(ColorSchemes.SageGreen, 2f))
                    {
                        e.Graphics.DrawPath(borderPen, checkPath);
                    }
                }
            };

            // 체크박스 클릭 이벤트
            checkBox.Click += (s, e) =>
            {
                if (checkBox.Tag is object tagObj)
                {
                    var tagType = tagObj.GetType();
                    var userIdProp = tagType.GetProperty("UserId");
                    var checkedProp = tagType.GetProperty("Checked");

                    if (userIdProp != null && checkedProp != null)
                    {
                        int id = (int)userIdProp.GetValue(tagObj);
                        bool currentChecked = (bool)checkedProp.GetValue(tagObj);

                        checkBox.Tag = new { UserId = id, Checked = !currentChecked };
                        checkBox.Invalidate();
                    }
                }
            };

            // UserId 라벨
            Label userIdLabel = new Label
            {
                Text = userId.ToString(),
                Location = new Point(40, 15),
                Size = new Size(60, 20),
                Font = new Font("맑은 고딕", 11F),
                ForeColor = ColorSchemes.DarkOlive,
                BackColor = Color.Transparent
            };

            // Name 라벨
            Label nameLabel = new Label
            {
                Text = name,
                Location = new Point(110, 15),
                Size = new Size(80, 20),
                Font = new Font("맑은 고딕", 11F),
                ForeColor = ColorSchemes.DarkOlive,
                BackColor = Color.Transparent
            };

            // Nickname 라벨
            Label nicknameLabel = new Label
            {
                Text = nickname,
                Location = new Point(200, 15),
                Size = new Size(130, 20),
                Font = new Font("맑은 고딕", 11F),
                ForeColor = ColorSchemes.DarkOlive,
                BackColor = Color.Transparent
            };

            // Dept 라벨
            Label deptLabel = new Label
            {
                Text = dept,
                Location = new Point(340, 15),
                Size = new Size(55, 20),
                Font = new Font("맑은 고딕", 11F),
                ForeColor = ColorSchemes.DarkOlive,
                BackColor = Color.Transparent
            };

            itemPanel.Controls.Add(checkBox);
            itemPanel.Controls.Add(userIdLabel);
            itemPanel.Controls.Add(nameLabel);
            itemPanel.Controls.Add(nicknameLabel);
            itemPanel.Controls.Add(deptLabel);

            // 클릭 시 체크박스 토글
            Action toggleCheck = () =>
            {
                if (checkBox.Tag is object tagObj)
                {
                    var tagType = tagObj.GetType();
                    var userIdProp = tagType.GetProperty("UserId");
                    var checkedProp = tagType.GetProperty("Checked");

                    if (userIdProp != null && checkedProp != null)
                    {
                        int id = (int)userIdProp.GetValue(tagObj);
                        bool currentChecked = (bool)checkedProp.GetValue(tagObj);

                        checkBox.Tag = new { UserId = id, Checked = !currentChecked };
                        checkBox.Invalidate();
                    }
                }
            };

            itemPanel.Click += (s, e) => toggleCheck();
            userIdLabel.Click += (s, e) => toggleCheck();
            nameLabel.Click += (s, e) => toggleCheck();
            nicknameLabel.Click += (s, e) => toggleCheck();
            deptLabel.Click += (s, e) => toggleCheck();

            return itemPanel;
        }

        /// <summary>
        /// 사용자 목록 로드
        /// </summary>
        public static void LoadUsers(SearchUserForm form, System.Collections.Generic.List<(int userId, string name, string nickname, string dept)> users)
        {
            Panel scrollPanel = null;

            foreach (Control control in form.Controls)
            {
                if (control is Panel wrapper && wrapper.Tag != null && wrapper.Tag.ToString() == "UserScrollPanelWrapper")
                {
                    foreach (Control child in wrapper.Controls)
                    {
                        if (child is Panel panel && panel.Name == "UserScrollPanel")
                        {
                            scrollPanel = panel;
                            break;
                        }
                    }
                    break;
                }
            }

            if (scrollPanel == null) return;

            scrollPanel.SuspendLayout();
            scrollPanel.Controls.Clear();

            int yPos = 0;
            foreach (var user in users)
            {
                Panel userItem = CreateUserItem(user.userId, user.name, user.nickname, user.dept);
                userItem.Location = new Point(0, yPos);
                scrollPanel.Controls.Add(userItem);
                yPos += 50;
            }

            scrollPanel.ResumeLayout();
        }

        private static void StyleButton(Button button)
        {
            button.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = ColorSchemes.DarkOlive;
            button.FlatAppearance.MouseOverBackColor = ColorSchemes.DarkOlive;
            button.Cursor = Cursors.Hand;
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
            button.MouseEnter += (s, e) => button.BackColor = ColorSchemes.DarkOlive;
            button.MouseLeave += (s, e) => button.BackColor = ColorSchemes.SageGreen;
        }

        private static void AdjustLayout(Form form)
        {
            int margin = 30;
            int topMargin = 60;
            int spacing = 15;

            foreach (Control control in form.Controls)
            {
                if (control is Panel wrapper && wrapper.Tag != null && wrapper.Tag.ToString() == "UserScrollPanelWrapper")
                {
                    wrapper.Location = new Point(margin, topMargin);
                    wrapper.Size = new Size(440, 290);
                }
                else if (control is Button btn && btn.Name == "AddButton")
                {
                    btn.Location = new Point(margin, topMargin + 290 + spacing);
                    btn.Width = 440;
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
