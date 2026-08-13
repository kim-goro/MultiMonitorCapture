using System;
using System.Threading;
using System.Windows.Forms;

namespace MultiMonitorCapture
{
    // 프로그램 진입점. 단일 인스턴스 보장과 전역 예외 처리를 설정하고 앱을 실행한다.
    internal static class Program
    {
        // 단일 인스턴스 판별용 뮤텍스 이름 (세션 로컬, 권한 문제 회피를 위해 Global 미사용)
        private const string MutexName = "MultiMonitorCapture_SingleInstance_7B1F3C2A";

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 전역 예외 처리기. 어떤 예외도 프로그램/PC 를 다운시키지 않도록 한다.
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            bool createdNew;
            using (Mutex mutex = new Mutex(true, MutexName, out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("멀티모니터캡처가 이미 실행 중입니다.", "멀티모니터캡처",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    AppRunner runner = Bootstrapper.Build();
                    Application.Run(runner);
                }
                catch (Exception ex)
                {
                    ReportFatal(ex);
                }
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ReportFatal(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ReportFatal(e.ExceptionObject as Exception);
        }

        private static void ReportFatal(Exception ex)
        {
            try
            {
                string message = ex == null ? "알 수 없는 오류가 발생했습니다." : ex.Message;
                MessageBox.Show("오류가 발생했습니다: " + message, "멀티모니터캡처",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                // 오류 표시 자체가 실패해도 무시한다
            }
        }
    }
}
