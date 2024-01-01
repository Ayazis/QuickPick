using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

namespace QuickPick.PinnedApps;

public class AppLinkRetriever
{
    private const string TASKBAR_FOLDERPATH = @"Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar";
    static string _taskbarFolder;
    static AppLinkRetriever()
    {
        string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        // In this folder, shortcuts to pinned apps are stored by Windows.
        _taskbarFolder = Path.Combine(appDataFolder, TASKBAR_FOLDERPATH);
    }

    static List<AppLink> _allShortCuts = new List<AppLink>();

    public static List<AppLink> GetPinnedAppsAndActiveWindows(bool includePinnedApps)
    {
        _allShortCuts.Clear();

        if (includePinnedApps)
            GetPinnedTaskbarApps();

        GetAllActiveApplications();

        return _allShortCuts;
    }

    private static void GetPinnedTaskbarApps()
    {
        if (!Directory.Exists(_taskbarFolder))
            return;

        string[] pinnedAppPaths = Directory.GetFiles(_taskbarFolder, "*.lnk", new EnumerationOptions { RecurseSubdirectories = true });

        foreach (string pinnedAppPath in pinnedAppPaths)
        {
            WshShell shell = new WshShell();
            var shortcut = shell.CreateShortcut(pinnedAppPath);
            string targetPath = shortcut.TargetPath;
            string arguments = shortcut.Arguments;
            string startinDir = shortcut.WorkingDirectory;

            if (!string.IsNullOrEmpty(targetPath))
            {
                AppLink appInfo = new AppLink()
                {
                    Name = Path.GetFileNameWithoutExtension(targetPath),
                    Arguments = arguments,
                    TargetPath = targetPath,
                    AppIcon = IconCreator.GetImage(targetPath),
                    StartInDirectory = startinDir,
                };

                _allShortCuts.Add(appInfo);
            }
        }
    }


    private static void GetAllActiveApplications()
    {
        List<AppLink> activeWindows = new List<AppLink>();

        IEnumerable<(IntPtr handle, Process process)> openProcesses = ActiveWindows.GetAllOpenWindows();

        foreach (var activeProcess in openProcesses)
        {
            var applicationAlreadyFound = _allShortCuts.FirstOrDefault(a => a.Name.Equals(activeProcess.process.ProcessName, StringComparison.OrdinalIgnoreCase));
            if (applicationAlreadyFound is not null)
            {
                if (activeProcess.process.MainWindowHandle != IntPtr.Zero)
                    applicationAlreadyFound.WindowHandles.Add(activeProcess.handle);

                continue;
            }

            var process = activeProcess.process;
            string filePath = string.Empty;

            if (process.MainWindowHandle != IntPtr.Zero)  // Exclude processes without a main window
            {
                try
                {
                    filePath = process.MainModule?.FileName;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    Console.WriteLine($"Unable to access {process.ProcessName}");
                    continue;
                }
                var appIcon = IconCreator.GetImage(filePath);
                if (appIcon == null)
                {
                    Console.WriteLine($"Could not get icon for {filePath}");
                    continue;
                }

                AppLink activeWindow = new AppLink()
                {
                    Name = process.ProcessName,
                    Arguments = string.Empty, // Arguments might not be directly available for running processes
                    TargetPath = filePath,
                    AppIcon = appIcon,
                    WindowHandles = new List<IntPtr> { activeProcess.handle }
                };

                _allShortCuts.Add(activeWindow);
            }
        }
    }

}
