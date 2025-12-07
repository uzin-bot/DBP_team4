using System;
using System.Drawing;
using System.Windows.Forms;

namespace DBPAdmin
{
    // ==================== 1. 색상 팔레트 (이것만 수정하면 전체 UI 색상 변경) ====================
    public static class AppTheme
    {
        // 🎨 메인 팔레트 - 여기만 수정하세요!
        public static readonly Color Color1 = ColorTranslator.FromHtml("#F1F3E0");  // 가장 밝은 색 (배경)
        public static readonly Color Color2 = ColorTranslator.FromHtml("#D2DCB6");  // 밝은 색 (카드, 서브)
        public static readonly Color Color3 = ColorTranslator.FromHtml("#A1BC98");  // 중간 색 (버튼, 강조)
        public static readonly Color Color4 = ColorTranslator.FromHtml("#778873");  // 어두운 색 (사이드바, 헤더)

        // 🔧 역할별 색상 매핑 (팔레트 기반 자동 적용)
        public static readonly Color Background = Color1;       // 메인 배경
        public static readonly Color CardBg = Color.White;      // 카드 배경
        public static readonly Color SidebarBg = Color4;        // 사이드바 배경
        public static readonly Color Primary = Color3;          // 주요 버튼, 강조
        public static readonly Color Secondary = Color2;        // 보조 요소
        public static readonly Color HeaderBg = Color4;         // 테이블 헤더

        public static readonly Color TextDark = Color.FromArgb(50, 50, 50);     // 어두운 텍스트
        public static readonly Color TextLight = Color.White;                    // 밝은 텍스트
        public static readonly Color TextMuted = Color.FromArgb(120, 120, 120); // 흐린 텍스트

        public static readonly Color Border = Color2;           // 테두리
        public static readonly Color ButtonHover = Color2;      // 버튼 호버
        public static readonly Color ActiveMenu = Color3;       // 활성 메뉴
        public static readonly Color InactiveMenu = Color4;     // 비활성 메뉴
    }

    // ==================== 2. UIHelper (색상은 AppTheme에서 가져옴) ====================
    public static class UIHelper
    {
        // 기존 코드 호환용 Colors 클래스
        public static class Colors
        {
            public static readonly Color Primary = AppTheme.Primary;
            public static readonly Color DarkBg = AppTheme.SidebarBg;
            public static readonly Color LightBg = AppTheme.Background;
            public static readonly Color CardBg = AppTheme.CardBg;
            public static readonly Color TextPrimary = AppTheme.TextDark;
            public static readonly Color TextSecondary = AppTheme.TextMuted;
            public static readonly Color TextLight = AppTheme.TextLight;
            public static readonly Color Border = AppTheme.Border;
            public static readonly Color AccentLight = AppTheme.Secondary;
        }

        // 타이틀 레이블
        public static Label CreateTitle(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("맑은 고딕", 14F, FontStyle.Bold),
                ForeColor = AppTheme.TextDark,
                AutoSize = true
            };
        }

        // 일반 레이블
        public static Label CreateLabel(string text, int x, int y, int fontSize = 9, Color? color = null, bool bold = false)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Font = new Font("맑은 고딕", fontSize, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = color ?? AppTheme.TextDark,
                AutoSize = true
            };
        }

        // 카드 패널
        public static Panel CreateCard(int x, int y, int width, int height)
        {
            return new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = AppTheme.CardBg,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        // 텍스트박스
        public static TextBox CreateTextBox(int x, int y, int width, int height, string name, string placeholder = "")
        {
            var txt = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                Name = name,
                Font = new Font("맑은 고딕", 9F)
            };

            if (!string.IsNullOrEmpty(placeholder))
            {
                txt.Text = placeholder;
                txt.ForeColor = AppTheme.TextMuted;

                txt.GotFocus += (s, e) =>
                {
                    if (txt.Text == placeholder)
                    {
                        txt.Text = "";
                        txt.ForeColor = AppTheme.TextDark;
                    }
                };

                txt.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txt.Text))
                    {
                        txt.Text = placeholder;
                        txt.ForeColor = AppTheme.TextMuted;
                    }
                };
            }

            return txt;
        }

        // 콤보박스
        public static ComboBox CreateComboBox(int x, int y, int width, int height, string name)
        {
            return new ComboBox
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("맑은 고딕", 9F),
                Name = name
            };
        }

        // DateTimePicker
        public static DateTimePicker CreateDateTimePicker(int x, int y, int width, int height, string name)
        {
            return new DateTimePicker
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                Format = DateTimePickerFormat.Short,
                Font = new Font("맑은 고딕", 9F),
                Name = name
            };
        }

        // 메인 버튼 (Primary 색상)
        public static Button CreateBlueButton(string text, int x, int y, int width, int height)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = AppTheme.Primary,
                ForeColor = AppTheme.TextLight,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("맑은 고딕", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            // 호버 효과
            btn.MouseEnter += (s, e) => btn.BackColor = AppTheme.ButtonHover;
            btn.MouseLeave += (s, e) => btn.BackColor = AppTheme.Primary;

            return btn;
        }

        // DataGridView
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
                BackgroundColor = AppTheme.CardBg,
                BorderStyle = BorderStyle.FixedSingle,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 35,
                EnableHeadersVisualStyles = false
            };
            dgv.RowTemplate.Height = 35;

            // 헤더 스타일
            dgv.ColumnHeadersDefaultCellStyle.BackColor = AppTheme.HeaderBg;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextLight;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // 셀 스타일
            dgv.DefaultCellStyle.Font = new Font("맑은 고딕", 9F);
            dgv.DefaultCellStyle.SelectionBackColor = AppTheme.Primary;
            dgv.DefaultCellStyle.SelectionForeColor = AppTheme.TextLight;

            // 교대 행 색상
            dgv.AlternatingRowsDefaultCellStyle.BackColor = AppTheme.Secondary;

            return dgv;
        }

        // 센터 정렬 계산
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