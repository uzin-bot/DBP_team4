using DBP_Chat;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace leehaeun.UIHelpers
{
    public static class SearchAddressFormUIHelper
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// �ּ� �˻� �� ��Ÿ�� 관리
        /// </summary>
        public static void ApplyStyles(SearchAddressForm form)
        {
            form.BackColor = ThemeManager.ColorScheme.Ivory;
            form.Size = new Size(600, 540);
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.None;

            ApplyRoundedCorners(form, 15);
            CreateCustomTitleBar(form);
            StyleControls(form);
            AdjustLayout(form);
            ReconnectEvents(form);  // �̺�Ʈ �翬�� �߰�
        }

        /// <summary>
        /// �� �ձ� �𼭸�
        /// </summary>
        private static void ApplyRoundedCorners(Form form, int radius)
        {
            form.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, form.Width, form.Height, radius, radius));
        }

        /// <summary>
        /// Ŀ관리 Ÿ��Ʋ�� 관리
        /// </summary>
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
                Text = "×",
                Font = new Font("맑은 고딕", 11F, FontStyle.Bold),
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

        /// <summary>
        /// ��Ʈ�� ��Ÿ�� 관리
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
        /// TextBox ��Ÿ��
        /// </summary>
        private static void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("맑은 고딕", 10F);
            textBox.BackColor = ThemeManager.ColorScheme.White;

            // Placeholder 설정
            string placeHolder = "주소 검색";

            if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == placeHolder)
            {
                textBox.Text = placeHolder;
                textBox.ForeColor = ThemeManager.ColorScheme.SageGreen;
            }
            else
            {
                textBox.ForeColor = ThemeManager.ColorScheme.DarkOlive;
            }

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeHolder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = ThemeManager.ColorScheme.DarkOlive;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeHolder;
                    textBox.ForeColor = ThemeManager.ColorScheme.SageGreen;
                }
            };

            int wrapperWidth = 440;
            int wrapperHeight = 38;

            Panel wrapper = new Panel
            {
                Size = new Size(wrapperWidth, wrapperHeight),
                Location = textBox.Location,
                BackColor = ThemeManager.ColorScheme.White,
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
                    using (Pen pen = new Pen(ThemeManager.ColorScheme.SageGreen, 0.8f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };

            wrapper.SizeChanged += (s, e) =>
            {
                textBox.Width = wrapper.Width - 20;
            };
        }

        /// <summary>
        /// Button ��Ÿ��
        /// </summary>
        private static void StyleButton(Button button)
        {
            button.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            button.ForeColor = ThemeManager.ColorScheme.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = ThemeManager.ColorScheme.DarkOlive;
            button.FlatAppearance.MouseOverBackColor = ThemeManager.ColorScheme.DarkOlive;
            button.Cursor = Cursors.Hand;
            button.Height = 38;
            button.TabStop = false;
            button.BackColor = ThemeManager.ColorScheme.SageGreen;

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
            button.MouseEnter += (s, e) => button.BackColor = ThemeManager.ColorScheme.DarkOlive;
            button.MouseLeave += (s, e) => button.BackColor = ThemeManager.ColorScheme.SageGreen;
        }

        /// <summary>
        /// ListBox ��Ÿ��
        /// </summary>
        private static void StyleListBox(ListBox listBox)
        {
            listBox.BorderStyle = BorderStyle.None;
            listBox.Font = new Font("맑은 고딕", 9F);
            listBox.ForeColor = ThemeManager.ColorScheme.DarkOlive;
            listBox.BackColor = ThemeManager.ColorScheme.White;
            listBox.ItemHeight = 30;
            listBox.DrawMode = DrawMode.OwnerDrawFixed;

            int wrapperWidth = 540;
            int wrapperHeight = 280;

            Panel wrapper = new Panel
            {
                Size = new Size(wrapperWidth, wrapperHeight),
                Location = listBox.Location,
                BackColor = ThemeManager.ColorScheme.White,
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

            // 스크롤바를 가리는 패널 추가 (다크모드에서도 배경색과 동일하게)
            Panel scrollCover = new Panel
            {
                Location = new Point(wrapperWidth - 21, 1),
                Size = new Size(18, wrapperHeight - 2),
                BackColor = ThemeManager.ColorScheme.White,
                Name = "ScrollCover"
            };
            wrapper.Controls.Add(scrollCover);
            scrollCover.BringToFront();

            GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, wrapperWidth, wrapperHeight), 8);
            wrapper.Region = new Region(path);

            wrapper.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                GraphicsPath currentPath = GetRoundedRectangle(new Rectangle(0, 0, wrapper.Width, wrapper.Height), 8);
                wrapper.Region = new Region(currentPath);

                // 다크모드에 따라 테두리 색상 변경
                Color borderColor = ThemeManager.IsDarkMode ? ThemeManager.ColorScheme.White : ThemeManager.ColorScheme.SageGreen;

                using (GraphicsPath borderPath = GetRoundedRectangle(new Rectangle(0, 0, wrapper.Width - 1, wrapper.Height - 1), 8))
                {
                    using (Pen pen = new Pen(borderColor, 0.8f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }

                // ScrollCover 배경색 업데이트
                scrollCover.BackColor = ThemeManager.ColorScheme.White;
            };

            wrapper.SizeChanged += (s, e) =>
            {
                listBox.Size = new Size(wrapper.Width - 8, wrapper.Height - 8);
                // 스크롤바 커버 위치도 업데이트
                scrollCover.Location = new Point(wrapper.Width - 21, 1);
                scrollCover.Size = new Size(18, wrapper.Height - 2);
            };

            // ListBox 프로필 �׸���
            listBox.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // ���
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                Color selectionColor = ThemeManager.IsDarkMode ? Color.DimGray : ThemeManager.ColorScheme.LightOlive;
                using (SolidBrush bgBrush = new SolidBrush(isSelected ? selectionColor : ThemeManager.ColorScheme.White))
                {
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);
                }

                // �ؽ�Ʈ
                string text = listBox.Items[e.Index].ToString();
                using (SolidBrush textBrush = new SolidBrush(ThemeManager.ColorScheme.DarkOlive))
                {
                    e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds.Left + 10, e.Bounds.Top + 8);
                }

                // ���м�
                if (e.Index < listBox.Items.Count - 1)
                {
                    using (Pen linePen = new Pen(ThemeManager.ColorScheme.LightOlive, 1))
                    {
                        e.Graphics.DrawLine(linePen, e.Bounds.Left + 10, e.Bounds.Bottom - 1, e.Bounds.Right - 10, e.Bounds.Bottom - 1);
                    }
                }
            };

            // 맑은 고딕� �� ��ü �ٽ� �׸���
            listBox.SelectedIndexChanged += (s, e) =>
            {
                listBox.Invalidate();
            };
        }

        /// <summary>
        /// �̺�Ʈ �翬��
        /// </summary>
        private static void ReconnectEvents(SearchAddressForm form)
        {
            // SelectButton ã��
            Button selectButton = FindButtonByName(form, "SelectButton");
            if (selectButton != null)
            {
                // SelectButton Ŭ�� �̺�Ʈ 관리
                selectButton.Click += (s, e) =>
                {
                    // SearchAddressForm�� SelectButton_Click �޼��� ȣ��
                    var method = form.GetType().GetMethod("SelectButton_Click",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.Public);

                    if (method != null)
                    {
                        method.Invoke(form, new object[] { s, e });
                    }
                };
            }
        }

        /// <summary>
        /// �̸관리� Button ã��
        /// </summary>
        private static Button FindButtonByName(Control parent, string name)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button btn && btn.Name == name)
                {
                    return btn;
                }
                else if (control is Panel panel)
                {
                    Button found = FindButtonByName(panel, name);
                    if (found != null) return found;
                }
            }
            return null;
        }

        /// <summary>
        /// ���̾ƿ� 관리
        /// </summary>
        private static void AdjustLayout(Form form)
        {
            int leftMargin = 30;
            int rightMargin = 30;
            int topMargin = 60;
            int spacing = 15;
            int buttonHeight = 40;
            int listBoxHeight = 280;

            int availableWidth = form.Width - leftMargin - rightMargin;

            foreach (Control control in form.Controls)
            {
                if (control is Panel wrapper && wrapper.Tag != null)
                {
                    if (wrapper.Tag.ToString() == "AddressBox")
                    {
                        wrapper.Location = new Point(leftMargin, topMargin);
                        wrapper.Width = availableWidth;
                    }
                    else if (wrapper.Tag.ToString() == "ResultBox")
                    {
                        wrapper.Location = new Point(leftMargin, topMargin + buttonHeight + spacing);
                        wrapper.Size = new Size(availableWidth, listBoxHeight);
                    }
                }
                else if (control is Button btn && btn.Name == "SearchButton")
                {
                    // SearchButton�� AddressBox �󺧷� 관리
                    btn.Visible = false;
                }
                else if (control is Button btn2 && btn2.Name == "SelectButton")
                {
                    btn2.Location = new Point(leftMargin, topMargin + buttonHeight + spacing + listBoxHeight + spacing);
                    btn2.Width = availableWidth;
                    btn2.Height = buttonHeight;
                }
            }
        }

        /// <summary>
        /// �ђ� �簢�� ��� 관리
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
