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
        /// 미니멀 스타일 적용
        /// </summary>
        public static void ApplyStyles(LoginForm form)
        {
            // 폼 기본 설정
            form.BackColor = ThemeManager.ColorScheme.Ivory; // #F1F3E0
            form.Size = new Size(350, 600);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.None;

            // 둥글은 모서리
            ApplyRoundedCorners(form, 15);

            // 커스텀 타이틀바 생성 (기존 것 제거 후)
            RemoveExistingTitleBar(form);
            CreateCustomTitleBar(form);

            // 폼 컨트롤 스타일 적용
            StyleAllControls(form);

            // 레이아웃 조정
            AdjustLayout(form);
        }

        /// <summary>
        /// 기존 타이틀바 제거
        /// </summary>
        private static void RemoveExistingTitleBar(Form form)
        {
            Panel existingTitleBar = null;
            foreach (Control control in form.Controls)
            {
                if (control is Panel panel && panel.Name == "titleBar")
                {
                    existingTitleBar = panel;
                    break;
                }
            }

            if (existingTitleBar != null)
            {
                form.Controls.Remove(existingTitleBar);
                existingTitleBar.Dispose();
            }
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

            // 타이틀바 드래그
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
        /// TextBox 스타일
        /// </summary>
        private static void StyleTextBox(TextBox textBox, Form form)
        {
            // 기존 wrapper가 있는지 확인
            Panel existingWrapper = textBox.Parent as Panel;
            if (existingWrapper != null && existingWrapper.Tag?.ToString() == textBox.Name)
            {
                // 이미 wrapper가 있으면 색상만 업데이트
                existingWrapper.BackColor = ThemeManager.ColorScheme.White;
                textBox.BackColor = ThemeManager.ColorScheme.White;
                existingWrapper.Invalidate();
                return;
            }

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

                        // 비밀번호 박스일 입력 시작할 때 PasswordChar 활성화
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

                        // 비밀번호 박스일 PlaceHolder 표시 때 PasswordChar 비활성화
                        if (isPasswordBox)
                        {
                            textBox.PasswordChar = '\0';
                        }
                    }
                };

                // 비밀번호 박스 텍스트 변경 처리
                if (isPasswordBox)
                {
                    textBox.TextChanged += (s, e) =>
                    {
                        // PlaceHolder가 아니고 텍스트가 있으면 PasswordChar 활성화
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
        /// Button 스타일
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
        /// CheckBox 스타일
        /// </summary>
        private static void StyleCheckBox(CheckBox checkBox)
        {
            // 이미 스타일이 적용되었는지 확인 (Tag로 체크)
            if (checkBox.Tag != null && checkBox.Tag.ToString() == "styled")
            {
                // 이미 스타일이 적용된 경우 색상만 업데이트
                checkBox.BackColor = ThemeManager.ColorScheme.Ivory;
                checkBox.ForeColor = ThemeManager.ColorScheme.DarkOlive;
                checkBox.Invalidate(); // 다시 그리기
                return;
            }

            // 처음 스타일 적용
            checkBox.Tag = "styled"; // 스타일 적용 표시

            // 원래 텍스트 저장
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

            // 커스텀 그리기
            PaintEventHandler paintHandler = (s, e) =>
            {
                CheckBox cb = s as CheckBox;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.Clear(ThemeManager.ColorScheme.Ivory); // 배경 클리어

                // 체크박스 그릴 위치
                Rectangle checkBoxRect = new Rectangle(0, 3, 16, 16);

                // 마우스 호버 확인
                bool isHover = cb.ClientRectangle.Contains(cb.PointToClient(Cursor.Position));

                // 배경 색상
                Color bgColor;
                if (isHover)
                {
                    bgColor = ThemeManager.ColorScheme.LightOlive;
                }
                else
                {
                    bgColor = cb.Checked ? ThemeManager.ColorScheme.SageGreen : ThemeManager.ColorScheme.White;
                }

                // 둥근 사각 그리기
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

                // 체크 표시
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

                // 텍스트 그리기
                TextRenderer.DrawText(
                    e.Graphics,
                    originalText,
                    cb.Font,
                    new Rectangle(22, 0, cb.Width - 22, cb.Height),
                    ThemeManager.ColorScheme.DarkOlive, // 현재 테마의 색상 사용
                    ThemeManager.ColorScheme.Ivory, // 배경
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left
                );
            };

            checkBox.Paint += paintHandler;

            // 호버 시 다시 그리기
            checkBox.MouseEnter += (s, e) => checkBox.Invalidate();
            checkBox.MouseLeave += (s, e) => checkBox.Invalidate();
            checkBox.MouseMove += (s, e) => checkBox.Invalidate();
            checkBox.CheckedChanged += (s, e) => checkBox.Invalidate();
        }

        /// <summary>
        /// LinkLabel 스타일
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
        /// 레이아웃 조정
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
