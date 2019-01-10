using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentScript
{
    public static class Log
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
                      (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void ConsoleInfo(string message)
        {
            Console.WriteLine($"INFO - {DateTime.Now} - {message}");
        }

        public static void ConsoleError(string message)
        {
            Console.WriteLine($"ERROR - {DateTime.Now} - {message}");
        }

        public static void FileInfo(string message)
        {
            log.Info(message);
        }

        public static void FileError(string message)
        {
            log.Error(message);
        }

        public static void Info(string message)
        {
            Console.WriteLine($"INFO - {DateTime.Now} - {message}");
            log.Info(message);
        }

        public static void Error(string message)
        {
            Console.WriteLine($"ERROR - {DateTime.Now} - {message}");
            log.Error(message);
        }
    }
}
