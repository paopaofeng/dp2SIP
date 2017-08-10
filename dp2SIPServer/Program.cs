using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DigitalPlatform;
using DigitalPlatform.SIP2;

namespace dp2SIPServer
{
    static class Program
    {
#if NO
        static bool glExitApp = false;
#endif
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if NO
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.ThreadException += Application_ThreadException;
#endif
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.Run(new MainForm());
#if NO
            glExitApp = true;//标志应用程序可以退出
#endif
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogManager.Logger.Error("系统异常");

            Exception ex = e.ExceptionObject as Exception;
            LogManager.Logger.Error(ExceptionUtil.GetDebugText(ex));
        }
#if NO
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(@"F:\dp2SIPServerNew\output2.txt",
                    true,	// append
                    Encoding.UTF8))
            {
                sw.WriteLine("Application_ThreadException:" + e.Exception.Message);
                sw.WriteLine(e.Exception);
            }
        }
#endif
    }
}
