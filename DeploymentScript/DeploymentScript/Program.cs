using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.Reflection;

namespace DeploymentScript
{


    class Program
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        static void Main(string[] args)
        {
            try
            {

                var objDeploy = new Deployment();

                Log.Info("Deleting older log files." + AssemblyDirectory);
                objDeploy.DeleteOldLogFiles($"{AssemblyDirectory}\\logs");
                Log.Info("Older log files deleted successfully");

                Log.Info("**Parameters**");
                foreach (var item in args)
                {
                    Log.Info(item);
                }

                objDeploy.Deploy();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                var exception = $"Exception Occurred. Error Message - {ex.Message}. Error Inner exception - {ex.InnerException}";
                Log.Error(exception);
                throw;
            }
        }
        
        private static void DeleteDirectory(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
        }
    }
}
