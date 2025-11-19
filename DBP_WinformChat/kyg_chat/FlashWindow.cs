using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;

public static class FlashWindow
{
    // WinAPI 함수 선언
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

    // 깜빡임 정보 구조체 정의
    [StructLayout(LayoutKind.Sequential)]
    public struct FLASHWINFO
    {
        public uint cbSize;
        public IntPtr hwnd;
        public uint dwFlags;
        public uint uCount;
        public uint dwTimeout;
    }

    // 깜빡임 플래그 정의
    public const uint FLASHW_ALL = 3;       // 창의 캡션과 작업 표시줄을 모두 깜빡이게 함
    public const uint FLASHW_TIMERNOFG = 12; // 활성화되지 않은 상태에서만 깜빡임 (가장 적합)

    public static void Flash(Form form)
    {
        // 폼이 활성화 상태가 아니거나 최소화되어 있을 때만 실행
        if (form.WindowState == FormWindowState.Minimized || !form.ContainsFocus)
        {
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = form.Handle;
            fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fInfo.uCount = uint.MaxValue; // 무한 반복 (사용자가 클릭할 때까지)
            fInfo.dwTimeout = 0;

            FlashWindowEx(ref fInfo);
        }
    }
}