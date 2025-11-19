/*
namespace kyg
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
*/

using System;
using System.Windows.Forms;

namespace kyg // 프로젝트의 실제 네임스페이스로 변경하세요
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            // Windows Forms의 시각적 스타일을 적용합니다.
            //Application.EnableVisualStyles();

            // 텍스트 렌더링 호환 모드를 설정합니다.
            //Application.SetCompatibleTextRenderingDefault(false);

            // 처음 실행할 폼을 지정합니다. (여기서는 LoginForm이 되어야 하나,
            // 2주차 목표에 집중하여 MainForm을 바로 실행하는 것으로 가정합니다.)

            // TODO: 실제 프로젝트에서는 LoginForm을 먼저 실행해야 합니다.
            // Application.Run(new LoginForm()); 

            // 곽윤경님이 구현한 서버 시작 메서드 호출
            //ApplicationConfiguration.Initialize();
            // 현재는 MainForm을 바로 실행하여 곽윤경님의 기능 테스트를 용이하게 합니다.


            //kyg.StartServer();
            Application.Run(new MainForm());

            // 서버가 종료되지 않도록 대기
            //Console.WriteLine("Press any key to stop the server...");
            //Console.ReadKey();

        }
    }
}
