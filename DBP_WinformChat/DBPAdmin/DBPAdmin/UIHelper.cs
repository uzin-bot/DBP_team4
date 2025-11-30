using System;
using System.Drawing;
using System.Windows.Forms;

namespace DBPAdmin
{
    /// <summary>
    /// UI 생성 및 스타일 관리 헬퍼 클래스
    /// </summary>
    public static class UIHelper
    {
        // ==================== 색상 팔레트 ====================
        public static class Colors
        {
            public static readonly Color Primary = Color.FromArgb(49, 130, 206);
            public static readonly Color DarkBg = Color.FromArgb(45, 55, 72);
            public static readonly Color LightBg = Color.FromArgb(237, 242, 247);
            public static readonly Color CardBg = Color.White;
            public static readonly Color TextPrimary = Color.FromArgb(45, 55, 72);
            public static readonly Color TextSecondary = Color.Gray;
            public static readonly Color TextLight = Color.FromArgb(203, 213, 224);
            public static readonly Color Border = Color.FromArgb(226, 232, 240);
            public static readonly Color AccentLight = Color.FromArgb(247, 250, 252);
        }

        // ==================== 폰트 ====================
        public static class Fonts
        {
            public static readonly Font Title = new Font("맑은 고딕", 16F, FontStyle.Bold);
            public static readonly Font Heading = new Font("맑은 고딕", 11F, FontStyle.Bold);
            public static readonly Font ButtonLarge = new Font("맑은 고딕", 10F, FontStyle.Bold);
            public static readonly Font Normal = new Font("맑은 고딕", 9F);
            public static readonly Font Small = new Font("맑은 고딕", 8F);
            public static readonly Font Large = new Font("맑은 고딕", 28F, FontStyle.Bold);
        }

        // ==================== 간격 및 크기 ====================
        public static class Spacing
        {
            public const int Small = 10;
            public const int Medium = 20;
            public const int Large = 30;
        }

        // ==================== 타이틀 레이블 생성 ====================
        public static Label CreateTitle(string text)
        {
            return new Label
            {
                Text = text,
                Font = Fonts.Title,
                ForeColor = Colors.TextPrimary,
                AutoSize = true
            };
        }

        // ==================== 일반 레이블 생성 ====================
        public static Label CreateLabel(string text, int x, int y, int fontSize, Color color, bool bold = false)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Font = new Font("맑은 고딕", fontSize, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = color,
                AutoSize = true
            };
        }

        // ==================== 카드 패널 생성 ====================
        public static Panel CreateCard(int x, int y, int width, int height)
        {
            return new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Colors.CardBg,
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        // ==================== 텍스트박스 생성 ====================
        public static TextBox CreateTextBox(int x, int y, int width, int height, string name, string placeholder = "")
        {
            var txt = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = Fonts.Normal,
                Name = name
            };

            if (!string.IsNullOrEmpty(placeholder))
            {
                txt.Text = placeholder;
                txt.ForeColor = Color.Gray;

                txt.GotFocus += (s, e) =>
                {
                    if (txt.Text == placeholder)
                    {
                        txt.Text = "";
                        txt.ForeColor = Colors.TextPrimary;
                    }
                };

                txt.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txt.Text))
                    {
                        txt.Text = placeholder;
                        txt.ForeColor = Color.Gray;
                    }
                };
            }

            return txt;
        }

        // ==================== 콤보박스 생성 ====================
        public static ComboBox CreateComboBox(int x, int y, int width, int height, string name)
        {
            return new ComboBox
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = Fonts.Normal,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = name
            };
        }

        // ==================== DateTimePicker 생성 ====================
        public static DateTimePicker CreateDateTimePicker(int x, int y, int width, int height, string name)
        {
            return new DateTimePicker
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = Fonts.Normal,
                Format = DateTimePickerFormat.Short,
                Name = name
            };
        }

        // ==================== 버튼 생성 ====================
        public static Button CreateBlueButton(string text, int x, int y, int width, int height)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Colors.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = Fonts.ButtonLarge,
                Cursor = Cursors.Hand
            };
        }

        // ==================== DataGridView 생성 ====================
        public static DataGridView CreateDGV(int x, int y, int width, int height, string name)
        {
            var dgv = new DataGridView
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                Name = name,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                BackgroundColor = Colors.CardBg,
                BorderStyle = BorderStyle.FixedSingle,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 35,
                RowTemplate = { Height = 35 },
                EnableHeadersVisualStyles = false
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Colors.DarkBg;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = Fonts.Heading;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.DefaultCellStyle.Font = Fonts.Normal;
            dgv.DefaultCellStyle.SelectionBackColor = Colors.Primary;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            return dgv;
        }

        // ==================== 센터 정렬 계산 ====================
        public static int CalculateCenterX(int containerWidth, int elementWidth)
        {
            return (containerWidth - elementWidth) / 2;
        }

        public static int CalculateCenterY(int containerHeight, int elementHeight)
        {
            return (containerHeight - elementHeight) / 2;
        }

        public static int CalculateMultiElementStartX(int containerWidth, int elementWidth, int count, int spacing)
        {
            int totalWidth = (elementWidth * count) + (spacing * (count - 1));
            return (containerWidth - totalWidth) / 2;
        }
    }
}