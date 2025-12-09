using DBP_Chat;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DBPAdmin
{
    // ==================== 1. ���� �ȷ�Ʈ (�̰͸� �����ϸ� ��ü UI ���� ����) ====================
    public static class AppTheme
    {
        // ?? ���� �ȷ�Ʈ - ���⸸ �����ϼ���!
        public static readonly Color Color1 = ColorTranslator.FromHtml("#F1F3E0");  // ���� ���� �� (���)
        public static readonly Color Color2 = ColorTranslator.FromHtml("#D2DCB6");  // ���� �� (ī��, ����)
        public static readonly Color Color3 = ColorTranslator.FromHtml("#A1BC98");  // �߰� �� (��ư, ����)
        public static readonly Color Color4 = ColorTranslator.FromHtml("#778873");  // ��ο� �� (���̵��, ���)

        // ?? ���Һ� ���� ���� (�ȷ�Ʈ ��� �ڵ� ����)
        public static readonly Color Background = Color1;       // ���� ���
        public static readonly Color CardBg = Color.White;      // ī�� ���
        public static readonly Color SidebarBg = Color4;        // ���̵�� ���
        public static readonly Color Primary = Color3;          // �ֿ� ��ư, ����
        public static readonly Color Secondary = Color2;        // ���� ���
        public static readonly Color HeaderBg = Color4;         // ���̺� ���

        public static readonly Color TextDark = Color.FromArgb(50, 50, 50);     // ��ο� �ؽ�Ʈ
        public static readonly Color TextLight = Color.White;                    // ���� �ؽ�Ʈ
        public static readonly Color TextMuted = Color.FromArgb(120, 120, 120); // �帰 �ؽ�Ʈ

        public static readonly Color Border = Color2;           // �׵θ�
        public static readonly Color ButtonHover = Color2;      // ��ư ȣ��
        public static readonly Color ActiveMenu = Color3;       // Ȱ�� �޴�
        public static readonly Color InactiveMenu = Color4;     // ��Ȱ�� �޴�
    }

    // ==================== 2. UIHelper (������ AppTheme���� ������) ====================
    public static class AdminUIHelper
    {
        // ���� �ڵ� ȣȯ�� Colors Ŭ����
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

        // Ÿ��Ʋ ���̺�
        public static Label CreateTitle(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("���� ���", 14F, FontStyle.Bold),
                ForeColor = AppTheme.TextDark,
                AutoSize = true
            };
        }

        // �Ϲ� ���̺�
        public static Label CreateLabel(string text, int x, int y, int fontSize = 9, Color? color = null, bool bold = false)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Font = new Font("���� ���", fontSize, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = color ?? AppTheme.TextDark,
                AutoSize = true
            };
        }

        // ī�� �г�
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

        // �ؽ�Ʈ�ڽ�
        public static TextBox CreateTextBox(int x, int y, int width, int height, string name, string placeholder = "")
        {
            var txt = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                Name = name,
                Font = new Font("���� ���", 9F)
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

        // �޺��ڽ�
        public static ComboBox CreateComboBox(int x, int y, int width, int height, string name)
        {
            return new ComboBox
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("���� ���", 9F),
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
                Font = new Font("���� ���", 9F),
                Name = name
            };
        }

        // ���� ��ư (Primary ����)
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
                Font = new Font("���� ���", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            // ȣ�� ȿ��
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

            // ��� ��Ÿ��
            dgv.ColumnHeadersDefaultCellStyle.BackColor = AppTheme.HeaderBg;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextLight;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("���� ���", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // �� ��Ÿ��
            dgv.DefaultCellStyle.Font = new Font("���� ���", 9F);
            dgv.DefaultCellStyle.SelectionBackColor = AppTheme.Primary;
            dgv.DefaultCellStyle.SelectionForeColor = AppTheme.TextLight;

            // ���� �� ����
            dgv.AlternatingRowsDefaultCellStyle.BackColor = AppTheme.Secondary;

            return dgv;
        }

        // ���� ���� ���
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
