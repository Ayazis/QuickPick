using System;
using System.Collections.Generic;
using System.IO;
using Utilities;
using System.Diagnostics;
using IWshRuntimeLibrary;

namespace QuickPick.PinnedApps;

public class TaskbarApps
{
    private const string TASKBAR_FOLDERPATH = @"Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar";

    public static List<TaskbarShortCut> GetPinnedAppsAndActiveWindows()
    {
        List<TaskbarShortCut> allShortCuts = new List<TaskbarShortCut>();
        var pinnedApps = GetPinnedTaskbarApps();
        var activeWindows = GetTaskBarAppsForActiveApplications();

        // Todo: Filter 

      //  allShortCuts.AddRange(pinnedApps);
        allShortCuts.AddRange(activeWindows);
        return allShortCuts;
	}

    private static List<TaskbarShortCut> GetPinnedTaskbarApps()
    {
        List<TaskbarShortCut> pinnedApps = new List<TaskbarShortCut>();

        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string taskbarFolder = Path.Combine(appData, TASKBAR_FOLDERPATH);

        if (!Directory.Exists(taskbarFolder))
            return null;

		string[] pinnedAppPaths = Directory.GetFiles(taskbarFolder, "*.lnk", new EnumerationOptions { RecurseSubdirectories = true });

        foreach (string pinnedAppPath in pinnedAppPaths)
        {
			WshShell shell = new WshShell();
            var shortcut = shell.CreateShortcut(pinnedAppPath);
            string targetPath = shortcut.TargetPath;
            string arguments = shortcut.Arguments;           

            if (!string.IsNullOrEmpty(targetPath))
            {
                TaskbarShortCut appInfo = new TaskbarShortCut()
                {
                    Name = Path.GetFileNameWithoutExtension(targetPath),
                    Arguments = arguments,
                    TargetPath = targetPath,
                    AppIcon = IconCreator.GetImage(targetPath),                    
            };

                pinnedApps.Add(appInfo);
            }
        }

        return pinnedApps;
    }


	private static List<TaskbarShortCut> GetTaskBarAppsForActiveApplications()
	{
		List<TaskbarShortCut> activeWindows = new List<TaskbarShortCut>();

		IEnumerable<Process> openProcesses = ActiveWindows.GetAllOpenWindows();

		foreach (var process in openProcesses)
		{
            string filePath = string.Empty;
			if (process.MainWindowHandle != IntPtr.Zero)  // Exclude processes without a main window
			{
                try
                {
                    filePath = process.MainModule?.FileName;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Unable to acces {process.ProcessName}");                
                    continue;
                }
                var appIcon = IconCreator.GetImage(filePath);
                if (appIcon == null)
                {
                    Console.WriteLine($"Could not get icon for {filePath}");
                    continue;
                }

				TaskbarShortCut activeWindow = new TaskbarShortCut()
				{
					Name = process.MainWindowTitle,
					Arguments = string.Empty, // Arguments might not be directly available for running processes
					TargetPath = filePath,
					AppIcon = appIcon,
				};

				activeWindows.Add(activeWindow);
			}
		}

		return activeWindows;
	}

}
