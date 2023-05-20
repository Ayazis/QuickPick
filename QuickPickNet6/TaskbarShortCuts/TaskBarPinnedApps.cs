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


	private static List<TaskbarShortCut> _pinnedApps = new List<TaskbarShortCut>();
	static List<TaskbarShortCut> _activeWindows = new List<TaskbarShortCut>();

	/* Todo:
	 * Refactor as follows:
	 * Get all Pinned Apps,
	 * Foreach get active window opn current desktop
	 * Get all the other active windows on current Desktop, skip the pinned app ones
	 * Save list to field, use field next time?	 * 
	 * */



	public static List<TaskbarShortCut> GetPinnedAppsAndActiveWindows()
	{
		_pinnedApps.Clear();
        _activeWindows.Clear();

        List<TaskbarShortCut> allShortCuts = new List<TaskbarShortCut>();
		_pinnedApps = GetPinnedTaskbarApps();
		_activeWindows = GetTaskBarAppsForActiveApplications();

		allShortCuts.AddRange(_pinnedApps);
		allShortCuts.AddRange(_activeWindows);
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
				IntPtr windowHandle = ActiveWindows.GetActiveWindowOnCurentDesktop(targetPath);

				TaskbarShortCut appInfo = new TaskbarShortCut()
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


	private static List<TaskbarShortCut> GetTaskBarAppsForActiveApplications()
	{
		List<TaskbarShortCut> activeWindows = new List<TaskbarShortCut>();

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

				TaskbarShortCut activeWindow = new TaskbarShortCut()
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
