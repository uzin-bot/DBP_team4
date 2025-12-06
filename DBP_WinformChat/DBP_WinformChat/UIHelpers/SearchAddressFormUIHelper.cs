using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using leehaeun.Themes;

namespace leehaeun.UIHelpers
{
    public static class SearchAddressFormUIHelper
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// 주소 검색 폼 스타일 적용
        /// </summary>
        public static void ApplyStyles(SearchAddressForm form)
        {
            form.BackColor = ColorSchemes.Ivory;
            form.Size = new Size(600, 400);
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.None;

            ApplyRoundedCorners(form, 15);
            CreateCustomTitleBar(form);
            StyleControls(form);
            AdjustLayout(form);
        }

        /// <summary>
        /// 폼 둥근 모서리
        /// </summary>
        private static void ApplyRoundedCorners(Form form, int radius)
        {
            form.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, form.Width, form.Height, radius, radius));
        }

        /// <summary>
        /// 커스텀 타이틀바 생성
        /// </summary>
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

        /// <summary>
        /// 컨트롤 스타일 적용
        /// </summary>
        private static void StyleControls(Form form)
        {
            foreach (Control control in form.Controls)
            {
                if (control is TextBox tb)
                {
                    StyleTextBox(tb);
                }
                else if (control is Button btn && btn.Name != "closeButton")
                {
                    StyleButton(btn);
                }
                else if (control is ListBox lb)
                {
                    StyleListBox(lb);
                }
            }
        }

        /// <summary>
        /// TextBox 스타일
        /// </summary>
        private static void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("맑은 고딕", 10F);
            textBox.ForeColor = ColorSchemes.DarkOlive;
            textBox.BackColor = Color.White;

            int wrapperWidth = 440;
            int wrapperHeight = 38;

            Panel wrapper = new Panel
            {
                Size = new Size(wrapperWidth, wrapperHeight),
                Location = textBox.Location,
                BackColor = Color.White,
                Tag = textBox.Name,
                Cursor = Cursors.IBeam
            };

            textBox.Location = new Point(10, 10);
            textBox.Width = wrapperWidth - 20;

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

                GraphicsPath currentPath = GetRoundedRectangle(new Rectangle(0, 0, wrapper.Width, wrapper.Height), 8);
                wrapper.Region = new Region(currentPath);

                using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, wrapper.Width - 1, wrapper.Height - 1), 8))
                {
                    using (Pen pen = new Pen(ColorSchemes.SageGreen, 0.8f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };

            wrapper.SizeChanged += (s, e) =>
            {
                textBox.Width = wrapper.Width - 20;
            };

            // Placeholder 효과
            if (textBox.Text == "주소 입력")
            {
                textBox.ForeColor = Color.Gray;
                textBox.GotFocus += (s, e) =>
                {
                    if (textBox.Text == "주소 입력")
                    {
                        textBox.Text = "";
                        textBox.ForeColor = ColorSchemes.DarkOlive;
                    }
                };
                textBox.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.Text = "주소 입력";
                        textBox.ForeColor = Color.Gray;
                    }
                };
            }
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
            button.MouseEnter += (s, e) => button.BackColor = ColorSchemes.DarkOlive;
            button.MouseLeave += (s, e) => button.BackColor = ColorSchemes.SageGreen;
        }

        /// <summary>
        /// ListBox 스타일
        /// </summary>
        private static void StyleListBox(ListBox listBox)
        {
            listBox.BorderStyle = BorderStyle.None;
            listBox.Font = new Font("맑은 고딕", 9F);
            listBox.ForeColor = ColorSchemes.DarkOlive;
            listBox.BackColor = Color.White;
            listBox.ItemHeight = 30;
            listBox.DrawMode = DrawMode.OwnerDrawFixed;

            int wrapperWidth = 540;
            int wrapperHeight = 212;

            Panel wrapper = new Panel
            {
                Size = new Size(wrapperWidth, wrapperHeight),
                Location = listBox.Location,
                BackColor = Color.White,
                Tag = listBox.Name
            };

            listBox.Location = new Point(4, 4);
            listBox.Size = new Size(wrapperWidth - 8, wrapperHeight - 8);

            var parent = listBox.Parent;
            int tabIndex = parent.Controls.GetChildIndex(listBox);

            parent.Controls.Remove(listBox);
            wrapper.Controls.Add(listBox);
            parent.Controls.Add(wrapper);
            parent.Controls.SetChildIndex(wrapper, tabIndex);

            GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, wrapperWidth, wrapperHeight), 8);
            wrapper.Region = new Region(path);

            wrapper.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                GraphicsPath currentPath = GetRoundedRectangle(new Rectangle(0, 0, wrapper.Width, wrapper.Height), 8);
                wrapper.Region = new Region(currentPath);

                using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, wrapper.Width - 1, wrapper.Height - 1), 8))
                {
                    using (Pen pen = new Pen(ColorSchemes.SageGreen, 0.8f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };

            wrapper.SizeChanged += (s, e) =>
            {
                listBox.Size = new Size(wrapper.Width - 8, wrapper.Height - 8);
            };

            // ListBox 아이템 그리기
            listBox.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // 배경
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                using (SolidBrush bgBrush = new SolidBrush(isSelected ? ColorSchemes.LightOlive : Color.White))
                {
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);
                }

                // 텍스트
                string text = listBox.Items[e.Index].ToString();
                using (SolidBrush textBrush = new SolidBrush(ColorSchemes.DarkOlive))
                {
                    e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds.Left + 10, e.Bounds.Top + 8);
                }

                // 구분선
                if (e.Index < listBox.Items.Count - 1)
                {
                    using (Pen linePen = new Pen(ColorSchemes.LightOlive, 1))
                    {
                        e.Graphics.DrawLine(linePen, e.Bounds.Left + 10, e.Bounds.Bottom - 1, e.Bounds.Right - 10, e.Bounds.Bottom - 1);
                    }
                }
            };

            // 선택 변경 시 전체 다시 그리기
            listBox.SelectedIndexChanged += (s, e) =>
            {
                listBox.Invalidate();
            };

            // 마우스 클릭으로 선택 취소 가능
            listBox.MouseClick += (s, e) =>
            {
                int index = listBox.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches && index == listBox.SelectedIndex)
                {
                    listBox.ClearSelected();
                }
            };
        }

        /// <summary>
        /// 레이아웃 조정
        /// </summary>
        private static void AdjustLayout(Form form)
        {
            int leftMargin = 30;
            int rightMargin = 30;
            int topMargin = 60;
            int spacing = 20;

            int availableWidth = form.Width - leftMargin - rightMargin;
            int textBoxWidth = availableWidth - 100;
            int buttonWidth = 90;

            foreach (Control control in form.Controls)
            {
                if (control is Panel wrapper && wrapper.Tag != null)
                {
                    if (wrapper.Tag.ToString() == "AddressBox")
                    {
                        wrapper.Location = new Point(leftMargin, topMargin);
                        wrapper.Width = textBoxWidth;
                    }
                    else if (wrapper.Tag.ToString() == "ResultBox")
                    {
                        wrapper.Location = new Point(leftMargin, topMargin + 38 + spacing);
                        wrapper.Size = new Size(availableWidth, 212);
                    }
                }
                else if (control is Button btn && btn.Name == "SearchButton")
                {
                    btn.Location = new Point(leftMargin + textBoxWidth + 10, topMargin);
                    btn.Width = buttonWidth;
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
