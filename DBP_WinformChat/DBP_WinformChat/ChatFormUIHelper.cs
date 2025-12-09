using DBP_Chat;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace kyg
{
    /// <summary>
    /// ChatForm 헤더� UI �׸� 헤더� �� ��Ÿ�ϸ� �޼��带 헤더��ϴ� 헤더� Ŭ헤더��Դϴ�.
    /// </summary>
    public static class ChatFormUIHelper
    {
        // 1. Windows API �Լ� Import (PInvoke) - �ձ� �𼭸� 헤더헤더
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


        // 2. 컬럼 �ȷ�Ʈ 헤더�
        public static readonly Color ColorLightest = ColorTranslator.FromHtml("#f1f3e0"); // 맑은 고딕� �� (rtbChatLog 헤더)
        public static readonly Color ColorLight = ColorTranslator.FromHtml("#d2dcb6");    // �߰� 헤더� �� (�� 헤더, txtInput/txtSearch 헤더)
        public static readonly Color ColorMedium = ColorTranslator.FromHtml("#a1bc98");   // 버튼 헤더
        public static readonly Color ColorDarkest = ColorTranslator.FromHtml("#778873");  // 텍스트 헤더�
        public static readonly Color ColorWhite = Color.White; // 버튼 텍스트 헤더�

        // 3. �ձ� �𼭸� 헤더� �޼헤더
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

        // 4. ��Ÿ�� 헤더� �޼헤더

        /// <summary>
        /// 헤더� 기본 �׸� ��Ÿ�� (ColorLightest)�� 헤더��մϴ�. (RichTextBox�� 헤더 헤더��ϵ헤더 Form 헤더��ε�� 헤더�)
        /// </summary>
        public static void ApplyFormStyle(Form form)
        {
            if (form != null)
            {
                form.BackColor = ColorLightest;
            }
        }

        /// <summary>
        /// 버튼�� �׸� ��Ÿ맑은 고딕��մϴ�. (헤더: Medium, 텍스트: White, �ձ� �𼭸� 헤더�)
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
        /// 텍스트 �ڽ� �Ǵ� ��ġ 텍스트 �ڽ헤더 �׸� ��Ÿ맑은 고딕��մϴ�. 
        /// (헤더: ColorLightest, 텍스트: Darkest, �ձ� �𼭸� 헤더�)
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
        /// �Ϲ� ��Ʈ�ѿ� �߰� 맑은 고딕�(ColorLight)�� 헤더��մϴ�. (�� 헤더�)
        /// </summary>
        public static void ApplyDisplayStyle(Control control)
        {
            if (control == null || control.IsDisposed) 
                return;

            control.BackColor = ColorLight;
            control.ForeColor = ColorDarkest;
        }

        /// <summary>
        /// ��Ʈ�ѿ� 맑은 고딕� 헤더�(ColorLightest)�� Darkest 텍스트 ��맑은 고딕��մϴ�. (rtbChatLog��)
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
    /// ChatForm 헤더� UI �׸� 헤더� �� ��Ÿ�ϸ� �޼��带 헤더��ϴ� 헤더� Ŭ헤더��Դϴ�.
    /// </summary>
    public static class ChatFormUIHelper
    {
        // 1. Windows API �Լ� Import (PInvoke) - �ձ۰� �𼭸� 헤더헤더
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


        // 2. 컬럼 �ȷ�Ʈ 헤더�
        public static readonly Color ColorLightest = ColorTranslator.FromHtml("#f1f3e0"); // 맑은 고딕� �� (�� 헤더)
        public static readonly Color ColorLight = ColorTranslator.FromHtml("#d2dcb6");    // �߰� 헤더� �� (�Է�/�α� 헤더)
        public static readonly Color ColorMedium = ColorTranslator.FromHtml("#a1bc98");   // 버튼 헤더
        public static readonly Color ColorDarkest = ColorTranslator.FromHtml("#778873");  // 텍스트 헤더�
        public static readonly Color ColorWhite = Color.White; // 버튼 텍스트 헤더�

        // 3. �ձ۰� �𼭸� 헤더� �޼헤더
        /// <summary>
        /// ��Ʈ헤더� �𼭸헤더 �ձ۰� 헤더�ϴ�. (TextBoxBase ��Ʈ�ѿ� 헤더� BorderStyle=None 헤더�)
        /// </summary>
        public static void ApplyRoundCorners(Control control, int radius = 15)
        {
            if (control == null) return;

            IntPtr rgn = CreateRoundRectRgn(0, 0, control.Width, control.Height, radius, radius);
            SetWindowRgn(control.Handle, rgn, true);

            if (control is TextBoxBase textBox)
            {
                // �ձ۰� �𼭸헤더 헤더� BorderStyle�� None맑은 고딕��մϴ�.
                textBox.BorderStyle = BorderStyle.None;
            }
        }

        // 4. ��Ÿ�� 헤더� �޼헤더 (�ձ۰� �𼭸� ȣ�� �ڵ�� Load 이벤트맑은 고딕� ȣ맑은 고딕� 헤더ŵ�)

        /// <summary>
        /// 헤더� 기본 �׸� ��Ÿ�� (ColorLightest)�� 헤더��մϴ�.
        /// </summary>
        public static void ApplyFormStyle(Form form)
        {
            if (form != null)
            {
                form.BackColor = ColorLightest;
            }
        }

        /// <summary>
        /// 버튼�� �׸� ��Ÿ맑은 고딕��մϴ�. (헤더: Medium, 텍스트: White, �ձ۰� �𼭸� 헤더�)
        /// </summary>
        public static void ApplyButtonStyle(Button button, int radius = 15)
        {
            if (button != null)
            {
                button.BackColor = ColorMedium;
                button.ForeColor = ColorWhite;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;

                // �ձ۰� �𼭸� 헤더�
                ApplyRoundCorners(button, radius);
            }
        }

        /// <summary>
        /// 텍스트 �ڽ� �Ǵ� ��ġ 텍스트 �ڽ헤더 �׸� ��Ÿ맑은 고딕��մϴ�. 
        /// (헤더: ColorLight, 텍스트: Darkest. �ձ۰� �𼭸헤더 Load 이벤트맑은 고딕� ȣ�� �ʿ�)
        /// </summary>
        public static void ApplyInputStyle(TextBoxBase control, int radius = 15)
        {
            if (control != null)
            {
                control.BackColor = ColorLight;
                control.ForeColor = ColorDarkest;

                // ApplyRoundCorners(control, radius); // Load 이벤트맑은 고딕� ȣ�� �ʿ�
            }
        }

        /// <summary>
        /// �Ϲ� ��Ʈ�ѿ� �߰� 맑은 고딕�(ColorLight)�� 헤더��մϴ�. 
        /// (rtbChatLog�� 맑은 고딕�맑은 고딕. �ձ۰� �𼭸헤더 Load 이벤트맑은 고딕� ȣ�� �ʿ�)
        /// </summary>
        public static void ApplyDisplayStyle(Control control)
        {
            if (control != null)
            {
                control.BackColor = ColorLight;
                control.ForeColor = ColorDarkest;

                // ApplyRoundCorners(control); // Load 이벤트맑은 고딕� ȣ�� �ʿ�
            }
        }
    }
}
*/
