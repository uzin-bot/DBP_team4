using System;

namespace DBP_WinformChat
{
    internal static class ThemeService
    {
        private static bool _isDarkMode;

        public static event Action<bool>? ThemeChanged;

        public static bool IsDarkMode => _isDarkMode;

        public static void SetDarkMode(bool enable)
        {
            if (_isDarkMode == enable) return;
            _isDarkMode = enable;
            ThemeChanged?.Invoke(_isDarkMode);
        }

        // 모든 폼에서 공통 구독/해제 헬퍼
        public static void Subscribe(Form form, Action<bool> handler)
        {
            ThemeChanged += handler;
            form.FormClosed += (_, __) => ThemeChanged -= handler;
        }
    }
}