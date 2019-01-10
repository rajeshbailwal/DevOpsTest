using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DeploymentScript
{

    class Deployment
    {
        private List<string> _applications = new List<string>();
        private List<string> _applicationsPath = new List<string>();
        private string _backupPath = string.Empty;
        private string _stagePath = string.Empty;

        public void Deploy()
        {
            _applications = new List<string>()
            {
                "App11",
                "App22",
                "App33",
                "App44"
            };

            _applicationsPath = new List<string>()
            {
                @"C:\Users\Rajesh\Desktop\DevOps\Apps\App1",
                @"C:\Users\Rajesh\Desktop\DevOps\Apps\App2",
                @"C:\Users\Rajesh\Desktop\DevOps\Apps\App3",
                @"C:\Users\Rajesh\Desktop\DevOps\Apps\App4"
            };

            _backupPath = @"C:\Users\Rajesh\Desktop\DevOps\BackUp";

            _stagePath= @"C:\Users\Rajesh\Desktop\DevOps\Temp";

            Log.Info("Matching count - started");
            // 1. check applications are applicationpath count are same
            if (_applications.Count != _applicationsPath.Count)
            {
                Log.Error("Parameter app name and path count doesn't match.");
                return;
            }
            Log.Info("Matching count completed successfully - count matched");

            Log.Info("Backup process started");
            // 2. delete back up folder (if any) and add back up of existing app
            foreach (var path in _applicationsPath)
            {
                var appFolderName = new DirectoryInfo(path).Name; ;
                var appBackUpPath = $"{_backupPath}\\{appFolderName}";

                if (Directory.Exists(appBackUpPath))
                {
                    Directory.Delete(appBackUpPath, true);
                    Directory.CreateDirectory(appBackUpPath);
                    DirectoryCopy(path, appBackUpPath, true);
                }
            }
            Log.Info("Backup process comppleted successfully");

            Log.Info("Deployment started");
            var appIndex = 0;
            foreach (var app in _applications)
            {
                // delete all existing contents
                DeleteDirectoryContents(_applicationsPath[appIndex]);

                //Copy all new items to app
                DirectoryCopy($"{_stagePath}\\{_applications[appIndex]}", _applicationsPath[appIndex], true);
                appIndex++;

            }
        }

        private void DeleteDirectoryContents(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public void DeleteOldLogFiles(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
