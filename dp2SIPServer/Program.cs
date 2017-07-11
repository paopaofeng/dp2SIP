using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
            Application.Run(new MainForm());
#if NO
            glExitApp = true;//标志应用程序可以退出
#endif
        }
#if NO
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(@"F:\dp2SIPServerNew\output.txt",
                    true,	// append
                    Encoding.UTF8))
            {
                sw.WriteLine("CurrentDomain_UnhandledException");
                sw.WriteLine("IsTerminating : " + e.IsTerminating.ToString());
                sw.WriteLine(e.ExceptionObject.ToString());

                while (true)
                {
                    //循环处理，否则应用程序将会退出
                    if (glExitApp)
                    {//标志应用程序可以退出，否则程序退出后，进程仍然在运行
                        sw.WriteLine("ExitApp");
                        return;
                    }
                    System.Threading.Thread.Sleep(2 * 1000);
                };
            }
        }

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
