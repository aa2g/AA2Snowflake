﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AA2Snowflake
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if RELEASE
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif
            Application.Run(new formMain());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (saveLog(e.Exception))
            {
                var crash = new formCrash(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace, "AA2Install crash " + DateTime.Now.ToString("d-M-yyyy hh-mm-ss") + ".dmp");
                crash.ShowDialog();
            }
            else
            {
                var crash = new formCrash(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
                crash.ShowDialog();
            }
            Application.Exit();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (saveLog(e.ExceptionObject as Exception))
            {
                var crash = new formCrash((e.ExceptionObject as Exception).Message + Environment.NewLine + (e.ExceptionObject as Exception).StackTrace, "AA2Install crash " + DateTime.Now.ToString("d-M-yyyy hh-mm-ss") + ".dmp");
                crash.ShowDialog();
            }
            else
            {
                var crash = new formCrash((e.ExceptionObject as Exception).Message + Environment.NewLine + (e.ExceptionObject as Exception).StackTrace);
                crash.ShowDialog();
            }
            Application.Exit();
        }

        static bool saveLog(Exception ex)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                SerializableDictionary<string, string> log = new SerializableDictionary<string, string>();
                
                log["message"] = ex.Message;
                log["stacktrace"] = ex.StackTrace;
                log["logger"] = Logger.Export();
                File.WriteAllText(Environment.CurrentDirectory + @"\AA2Snowflake crash " + DateTime.Now.ToString("d-M-yyyy hh-mm-ss") + ".dmp", log.SerializeObject());
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
