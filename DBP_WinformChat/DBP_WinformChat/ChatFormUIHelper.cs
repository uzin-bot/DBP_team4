using DBP_Chat;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace kyg
{
    /// <summary>
    /// ChatForm 전용 UI 테마 색상 및 스타일링 메서드를 제공하는 헬퍼 클래스입니다.
    /// </summary>
    public static class ChatFormUIHelper
    {
        // 1. Windows API 함수 Import (PInvoke) - 둥근 모서리 구현용
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


        // 2. 컬러 팔레트 정의
        public static readonly Color ColorLightest = ColorTranslator.FromHtml("#f1f3e0"); // 가장 밝은 색 (rtbChatLog 배경)
        public static readonly Color ColorLight = ColorTranslator.FromHtml("#d2dcb6");    // 중간 밝은 색 (폼 배경, txtInput/txtSearch 배경)
        public static readonly Color ColorMedium = ColorTranslator.FromHtml("#a1bc98");   // 버튼 배경
        public static readonly Color ColorDarkest = ColorTranslator.FromHtml("#778873");  // 텍스트 색상
        public static readonly Color ColorWhite = Color.White; // 버튼 텍스트 색상

        // 3. 둥근 모서리 적용 메서드
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

        // 4. 스타일 적용 메서드

        /// <summary>
        /// 폼의 기본 테마 스타일 (ColorLightest)을 적용합니다. (RichTextBox에 사용 가능하도록 Form 오버로드는 유지)
        /// </summary>
        public static void ApplyFormStyle(Form form)
        {
            if (form != null)
            {
                form.BackColor = ColorLightest;
            }
        }

        /// <summary>
        /// 버튼에 테마 스타일을 적용합니다. (배경: Medium, 텍스트: White, 둥근 모서리 적용)
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
        /// 텍스트 박스 또는 리치 텍스트 박스에 테마 스타일을 적용합니다. 
        /// (배경: ColorLightest, 텍스트: Darkest, 둥근 모서리 적용)
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
        /// 일반 컨트롤에 중간 밝은 배경색(ColorLight)을 적용합니다. (폼 배경용)
        /// </summary>
        public static void ApplyDisplayStyle(Control control)
        {
            if (control == null || control.IsDisposed) 
                return;

            control.BackColor = ColorLight;
            control.ForeColor = ColorDarkest;
        }

        /// <summary>
        /// 컨트롤에 가장 밝은 배경색(ColorLightest)과 Darkest 텍스트 색상을 적용합니다. (rtbChatLog용)
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
    /// ChatForm 전용 UI 테마 색상 및 스타일링 메서드를 제공하는 헬퍼 클래스입니다.
    /// </summary>
    public static class ChatFormUIHelper
    {
        // 1. Windows API 함수 Import (PInvoke) - 둥글게 모서리 구현용
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


        // 2. 컬러 팔레트 정의
        public static readonly Color ColorLightest = ColorTranslator.FromHtml("#f1f3e0"); // 가장 밝은 색 (폼 배경)
        public static readonly Color ColorLight = ColorTranslator.FromHtml("#d2dcb6");    // 중간 밝은 색 (입력/로그 배경)
        public static readonly Color ColorMedium = ColorTranslator.FromHtml("#a1bc98");   // 버튼 배경
        public static readonly Color ColorDarkest = ColorTranslator.FromHtml("#778873");  // 텍스트 색상
        public static readonly Color ColorWhite = Color.White; // 버튼 텍스트 색상

        // 3. 둥글게 모서리 적용 메서드
        /// <summary>
        /// 컨트롤의 모서리를 둥글게 만듭니다. (TextBoxBase 컨트롤에 대해 BorderStyle=None 설정)
        /// </summary>
        public static void ApplyRoundCorners(Control control, int radius = 15)
        {
            if (control == null) return;

            IntPtr rgn = CreateRoundRectRgn(0, 0, control.Width, control.Height, radius, radius);
            SetWindowRgn(control.Handle, rgn, true);

            if (control is TextBoxBase textBox)
            {
                // 둥글게 모서리를 위해 BorderStyle을 None으로 설정합니다.
                textBox.BorderStyle = BorderStyle.None;
            }
        }

        // 4. 스타일 적용 메서드 (둥글게 모서리 호출 코드는 Load 이벤트에서 별도 호출을 위해 제거됨)

        /// <summary>
        /// 폼의 기본 테마 스타일 (ColorLightest)을 적용합니다.
        /// </summary>
        public static void ApplyFormStyle(Form form)
        {
            if (form != null)
            {
                form.BackColor = ColorLightest;
            }
        }

        /// <summary>
        /// 버튼에 테마 스타일을 적용합니다. (배경: Medium, 텍스트: White, 둥글게 모서리 적용)
        /// </summary>
        public static void ApplyButtonStyle(Button button, int radius = 15)
        {
            if (button != null)
            {
                button.BackColor = ColorMedium;
                button.ForeColor = ColorWhite;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;

                // 둥글게 모서리 적용
                ApplyRoundCorners(button, radius);
            }
        }

        /// <summary>
        /// 텍스트 박스 또는 리치 텍스트 박스에 테마 스타일을 적용합니다. 
        /// (배경: ColorLight, 텍스트: Darkest. 둥글게 모서리는 Load 이벤트에서 별도 호출 필요)
        /// </summary>
        public static void ApplyInputStyle(TextBoxBase control, int radius = 15)
        {
            if (control != null)
            {
                control.BackColor = ColorLight;
                control.ForeColor = ColorDarkest;

                // ApplyRoundCorners(control, radius); // Load 이벤트에서 별도 호출 필요
            }
        }

        /// <summary>
        /// 일반 컨트롤에 중간 밝은 배경색(ColorLight)을 적용합니다. 
        /// (rtbChatLog의 원래 배경색으로 사용. 둥글게 모서리는 Load 이벤트에서 별도 호출 필요)
        /// </summary>
        public static void ApplyDisplayStyle(Control control)
        {
            if (control != null)
            {
                control.BackColor = ColorLight;
                control.ForeColor = ColorDarkest;

                // ApplyRoundCorners(control); // Load 이벤트에서 별도 호출 필요
            }
        }
    }
}
*/
