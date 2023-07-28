using System;
using System.Collections.Generic;
using System.IO;
using Utilities;
using System.Diagnostics;
using IWshRuntimeLibrary;
using System.Linq;

namespace QuickPick.PinnedApps;

public class TaskbarApps
{
	private const string TASKBAR_FOLDERPATH = @"Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar";


	private static List<AppLink> _pinnedApps = new List<AppLink>();
	static List<AppLink> _activeWindows = new List<AppLink>();

	public static List<AppLink> GetPinnedAppsAndActiveWindows()
	{
		_pinnedApps.Clear();
        _activeWindows.Clear();

        List<AppLink> allShortCuts = new List<AppLink>();
		_pinnedApps = GetPinnedTaskbarApps();
		_activeWindows = GetTaskBarAppsForActiveApplications();

		allShortCuts.AddRange(_pinnedApps);
		allShortCuts.AddRange(_activeWindows);

		return allShortCuts;
	}

	private static List<AppLink> GetPinnedTaskbarApps()
	{
		List<AppLink> pinnedApps = new List<AppLink>();

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
				IntPtr windowHandle = ActiveWindows.GetActiveWindowOnCurentDesktop(targetPath);

				AppLink appInfo = new AppLink()
				{
					Name = Path.GetFileNameWithoutExtension(targetPath),
					Arguments = arguments,
					TargetPath = targetPath,
					AppIcon = IconCreator.GetImage(targetPath),
					WindowHandle = windowHandle,
				};


				pinnedApps.Add(appInfo);
			}
		}

		return pinnedApps;
	}


	private static List<AppLink> GetTaskBarAppsForActiveApplications()
	{
		List<AppLink> activeWindows = new List<AppLink>();

		IEnumerable<(IntPtr handle, Process process)> openProcesses = ActiveWindows.GetAllOpenWindows();

		foreach (var activeProcess in openProcesses)
		{
			if (_pinnedApps.Any(a => a.WindowHandle == activeProcess.handle))
				continue;

			var process = activeProcess.process;
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

				AppLink activeWindow = new AppLink()
				{
					Name = process.MainWindowTitle,
					Arguments = string.Empty, // Arguments might not be directly available for running processes
					TargetPath = filePath,
					AppIcon = appIcon,
					WindowHandle = activeProcess.handle
				};

				activeWindows.Add(activeWindow);
			}
		}

		return activeWindows;
	}

}
