using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

namespace SummerPractice
{
  static class Program
  {
    /// <summary>
    /// Главная точка входа для приложения.
    /// </summary>
    [STAThread]
    static void Main()
    {
      try
      {
        log = LogManager.GetCurrentClassLogger();

        log.Trace("Version: {0}", Environment.Version.ToString());
        log.Trace("OS: {0}", Environment.OSVersion.ToString());
        log.Trace("CommandLine: {0}", Environment.CommandLine.ToString());
      }
      catch (Exception e)
      {
        MessageBox.Show("Ошибка работы с логом\n" + e.Message);
        Application.Exit();
        throw;
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new LoadingScreen());
      Application.Exit();
    }

    public static Logger log;
  }
}
