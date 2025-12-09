using DBP_Chat;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace kyg
{
    /// <summary>
    /// ChatForm ���� UI �׸� ���� �� ��Ÿ�ϸ� �޼��带 �����ϴ� ���� Ŭ�����Դϴ�.
    /// </summary>
    public static class ChatFormUIHelper
    {
        // 1. Windows API �Լ� Import (PInvoke) - �ձ� �𼭸� ������
        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        [DllImport("user32.dll", EntryPoint = "SetWindowRgn")]
        private static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);


        // 2. �÷� �ȷ�Ʈ ����
        public static readonly Color ColorLightest = ColorTranslator.FromHtml("#f1f3e0"); // ���� ���� �� (rtbChatLog ���)
        public static readonly Color ColorLight = ColorTranslator.FromHtml("#d2dcb6");    // �߰� ���� �� (�� ���, txtInput/txtSearch ���)
        public static readonly Color ColorMedium = ColorTranslator.FromHtml("#a1bc98");   // ��ư ���
        public static readonly Color ColorDarkest = ColorTranslator.FromHtml("#778873");  // �ؽ�Ʈ ����
        public static readonly Color ColorWhite = Color.White; // ��ư �ؽ�Ʈ ����

        // 3. �ձ� �𼭸� ���� �޼���
        public static void ApplyRoundCorners(Control control, int radius = 15)
        {
            if (control == null || control.IsDisposed || !control.IsHandleCreated) 
                return;

            IntPtr rgn = CreateRoundRectRgn(0, 0, control.Width, control.Height, radius, radius);
            SetWindowRgn(control.Handle, rgn, true);

            if (control is TextBoxBase textBox)
            {
                textBox.BorderStyle = BorderStyle.None;
            }
        }

        // 4. ��Ÿ�� ���� �޼���

        /// <summary>
        /// ���� �⺻ �׸� ��Ÿ�� (ColorLightest)�� �����մϴ�. (RichTextBox�� ��� �����ϵ��� Form �����ε�� ����)
        /// </summary>
        public static void ApplyFormStyle(Form form)
        {
            if (form != null)
            {
                form.BackColor = ColorLightest;
            }
        }

        /// <summary>
        /// ��ư�� �׸� ��Ÿ���� �����մϴ�. (���: Medium, �ؽ�Ʈ: White, �ձ� �𼭸� ����)
        /// </summary>
        public static void ApplyButtonStyle(Button button, int radius = 15)
        {
            if (button == null || button.IsDisposed) 
                return;

            button.BackColor = ColorMedium;
            button.ForeColor = ColorWhite;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

            ApplyRoundCorners(button, radius);
        }

        /// <summary>
        /// �ؽ�Ʈ �ڽ� �Ǵ� ��ġ �ؽ�Ʈ �ڽ��� �׸� ��Ÿ���� �����մϴ�. 
        /// (���: ColorLightest, �ؽ�Ʈ: Darkest, �ձ� �𼭸� ����)
        /// </summary>
        public static void ApplyInputStyle(TextBoxBase control, int radius = 15)
        {
            if (control == null || control.IsDisposed) 
                return;

            control.BackColor = ColorLightest;
            control.ForeColor = ColorDarkest;

            ApplyRoundCorners(control, radius);
        }

        /// <summary>
        /// �Ϲ� ��Ʈ�ѿ� �߰� ���� ����(ColorLight)�� �����մϴ�. (�� ����)
        /// </summary>
        public static void ApplyDisplayStyle(Control control)
        {
            if (control == null || control.IsDisposed) 
                return;

            control.BackColor = ColorLight;
            control.ForeColor = ColorDarkest;
        }

        /// <summary>
        /// ��Ʈ�ѿ� ���� ���� ����(ColorLightest)�� Darkest �ؽ�Ʈ ������ �����մϴ�. (rtbChatLog��)
        /// </summary>
        public static void ApplyLightestStyle(Control control)
        {
            if (control == null || control.IsDisposed) 
                return;

            control.BackColor = ColorLightest;
            control.ForeColor = ColorDarkest;
        }
    }
}
/*
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace kyg
{
    /// <summary>
    /// ChatForm ���� UI �׸� ���� �� ��Ÿ�ϸ� �޼��带 �����ϴ� ���� Ŭ�����Դϴ�.
    /// </summary>
    public static class ChatFormUIHelper
    {
        // 1. Windows API �Լ� Import (PInvoke) - �ձ۰� �𼭸� ������
        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        [DllImport("user32.dll", EntryPoint = "SetWindowRgn")]
        private static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);


        // 2. �÷� �ȷ�Ʈ ����
        public static readonly Color ColorLightest = ColorTranslator.FromHtml("#f1f3e0"); // ���� ���� �� (�� ���)
        public static readonly Color ColorLight = ColorTranslator.FromHtml("#d2dcb6");    // �߰� ���� �� (�Է�/�α� ���)
        public static readonly Color ColorMedium = ColorTranslator.FromHtml("#a1bc98");   // ��ư ���
        public static readonly Color ColorDarkest = ColorTranslator.FromHtml("#778873");  // �ؽ�Ʈ ����
        public static readonly Color ColorWhite = Color.White; // ��ư �ؽ�Ʈ ����

        // 3. �ձ۰� �𼭸� ���� �޼���
        /// <summary>
        /// ��Ʈ���� �𼭸��� �ձ۰� ����ϴ�. (TextBoxBase ��Ʈ�ѿ� ���� BorderStyle=None ����)
        /// </summary>
        public static void ApplyRoundCorners(Control control, int radius = 15)
        {
            if (control == null) return;

            IntPtr rgn = CreateRoundRectRgn(0, 0, control.Width, control.Height, radius, radius);
            SetWindowRgn(control.Handle, rgn, true);

            if (control is TextBoxBase textBox)
            {
                // �ձ۰� �𼭸��� ���� BorderStyle�� None���� �����մϴ�.
                textBox.BorderStyle = BorderStyle.None;
            }
        }

        // 4. ��Ÿ�� ���� �޼��� (�ձ۰� �𼭸� ȣ�� �ڵ�� Load �̺�Ʈ���� ���� ȣ���� ���� ���ŵ�)

        /// <summary>
        /// ���� �⺻ �׸� ��Ÿ�� (ColorLightest)�� �����մϴ�.
        /// </summary>
        public static void ApplyFormStyle(Form form)
        {
            if (form != null)
            {
                form.BackColor = ColorLightest;
            }
        }

        /// <summary>
        /// ��ư�� �׸� ��Ÿ���� �����մϴ�. (���: Medium, �ؽ�Ʈ: White, �ձ۰� �𼭸� ����)
        /// </summary>
        public static void ApplyButtonStyle(Button button, int radius = 15)
        {
            if (button != null)
            {
                button.BackColor = ColorMedium;
                button.ForeColor = ColorWhite;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;

                // �ձ۰� �𼭸� ����
                ApplyRoundCorners(button, radius);
            }
        }

        /// <summary>
        /// �ؽ�Ʈ �ڽ� �Ǵ� ��ġ �ؽ�Ʈ �ڽ��� �׸� ��Ÿ���� �����մϴ�. 
        /// (���: ColorLight, �ؽ�Ʈ: Darkest. �ձ۰� �𼭸��� Load �̺�Ʈ���� ���� ȣ�� �ʿ�)
        /// </summary>
        public static void ApplyInputStyle(TextBoxBase control, int radius = 15)
        {
            if (control != null)
            {
                control.BackColor = ColorLight;
                control.ForeColor = ColorDarkest;

                // ApplyRoundCorners(control, radius); // Load �̺�Ʈ���� ���� ȣ�� �ʿ�
            }
        }

        /// <summary>
        /// �Ϲ� ��Ʈ�ѿ� �߰� ���� ����(ColorLight)�� �����մϴ�. 
        /// (rtbChatLog�� ���� �������� ���. �ձ۰� �𼭸��� Load �̺�Ʈ���� ���� ȣ�� �ʿ�)
        /// </summary>
        public static void ApplyDisplayStyle(Control control)
        {
            if (control != null)
            {
                control.BackColor = ColorLight;
                control.ForeColor = ColorDarkest;

                // ApplyRoundCorners(control); // Load �̺�Ʈ���� ���� ȣ�� �ʿ�
            }
        }
    }
}
*/
