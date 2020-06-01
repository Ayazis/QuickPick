using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalHOoks.Logic
{
    public static class Logger
    {
        private static string logPath = @"C:\temp\QuickPicLogs\";

        public static void Log(string logEntry)
        {
            try
            {
                CreateDirectory();
                SetDutchCulture();
                var dateNow = DateTime.Now.ToShortDateString();

                File.AppendAllText($@"{logPath}QpLog{dateNow}.txt", logEntry);
            }
            catch (Exception)
            {
                // do nothing..?

            }


        }

        private static void SetDutchCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
        }
        private static void CreateDirectory()
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
        }

        

    }
}
