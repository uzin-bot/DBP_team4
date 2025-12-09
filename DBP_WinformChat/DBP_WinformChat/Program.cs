using leehaeun;
using DBP_Chat;
using DBP_WinformChat;

namespace DBP_WinformChat
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Settings에서 다크모드 설정 불러와서 테마 적용
            bool isDarkMode = Properties.Settings.Default.IsDarkMode;
            ThemeManager.SetDarkMode(isDarkMode);
            
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
            //Application.Run(new leehaeun.Login());
        }
    }
}