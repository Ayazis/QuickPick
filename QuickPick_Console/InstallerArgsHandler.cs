using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateInstaller.Updates;
using UpdateInstaller;
using Updates;
using System.Windows;

namespace QuickPick
{
    internal class InstallerArgsHandler
    {
        private static void CheckInputArguments(string[] args)
        {
            InstallerArguments? arguments = TryGetInstallerArguments(args);
            if (arguments == null)
            {
                UpdateIfAvailable();
            }
            else
            {
                UpdateAndRestart((InstallerArguments)arguments);
            }
        }

        private static void UpdateAndRestart(InstallerArguments arguments)
        {
            new ApplicationCloser().CloseApplication(arguments.ProcessIdToKill);
            FileCopier.CopyFiles(arguments.SourceFolder, arguments.TargetFolder);
            Process.Start(arguments.PathToExecutable, arguments.TargetArguments);
            Application.Current.Shutdown();
        }

        private static void UpdateIfAvailable()
        {
            // check for update
            GithubUpdateChecker updateChecker = new GithubUpdateChecker("Ayazis", "QuickPick");
            UpdateDownloader updateDownloader = new UpdateDownloader(eUpdateType.Pre_Release, updateChecker);
            UpdateDownloadManager updateManager = new UpdateDownloadManager(updateDownloader);
            bool UpdateIsAvailable = updateManager.CheckIfUpdateIsAvailableAsync().Result;
            if (UpdateIsAvailable)
            {
                //new StartWindow().Show();
                InstallerParams installerParams = updateManager.DownloadUpdateAndGetInstallerArguments();
                Process.Start(installerParams.InstallerPath, installerParams.Arguments.ToStringArray());
                Application.Current.Shutdown();
            }
        }

        static InstallerArguments? TryGetInstallerArguments(string[] args)
        {
            if (args == null || args.Length == 0)
                return null;

            try
            {
                return InstallerArguments.FromStringArray(args);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
