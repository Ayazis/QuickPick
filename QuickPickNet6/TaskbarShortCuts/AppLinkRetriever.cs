using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Utilities;
using System.Windows.Automation;
using System.Windows.Media;
using FontAwesome5;
using FontAwesome5.Extensions;
using System.Windows;

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

        GetCustomApps();

        return _allShortCuts;
    }
    static void GetCustomApps()
    {
        string[] customExecutablePaths = new string[]
        {
            @"C:\Program Files\WindowsApps\5319275A.WhatsAppDesktop_2.2414.8.0_x64__cv1g1gvanyjgm\WhatsApp.exe"
        };

        foreach (var path in customExecutablePaths)
        {
            IWshShortcut shortCutLink = CreateNewLnkFileForExecutable(path);
            CreateAppLinkFromShortCutFile(shortCutLink);
        }

    }
    private static void GetPinnedTaskbarApps()
    {
        if (!Directory.Exists(_taskbarFolder))
            return;

        string[] pinnedAppPaths = Directory.GetFiles(_taskbarFolder, "*.lnk", new EnumerationOptions { RecurseSubdirectories = true });

        foreach (string pinnedAppPath in pinnedAppPaths)
        {
            IWshShortcut shortCutLink = CreateShortCutObjectForExistingLink(pinnedAppPath);
            CreateAppLinkFromShortCutFile(shortCutLink);
        }
    }
    static IWshShortcut CreateShortCutObjectForExistingLink(string LnkLocation)
    {
        if (!System.IO.File.Exists(LnkLocation))
            throw new FileNotFoundException("Nonexistent.", LnkLocation);

        WshShell shell = new WshShell();
        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(LnkLocation);
        return shortcut;

    }
    static IWshShortcut CreateNewLnkFileForExecutable(string executablePath)
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string shortCutsFolder = Path.Combine(appDataPath, "QuickPick", "ShortCuts");

        if (!Directory.Exists(shortCutsFolder))
            Directory.CreateDirectory(shortCutsFolder);

        string appName = Path.GetFileNameWithoutExtension(executablePath);
        string LnkLocation = Path.Combine(shortCutsFolder, appName, ".lnk");

        WshShell shell = new WshShell();
        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(LnkLocation);

        shortcut.TargetPath = executablePath;
        shortcut.WorkingDirectory = Path.GetDirectoryName(executablePath);

        return shortcut;
    }





    private static void CreateAppLinkFromShortCutFile(IWshShortcut shortcut)
    {
        ImageSource icon = IconCreator.GetImage(shortcut.TargetPath);
        if (icon == null)
        {
            icon = TryCreateIconWithFontAwesome(shortcut.TargetPath);
        }
        if (!string.IsNullOrEmpty(shortcut.TargetPath))
        {
            AppLink appInfo = new AppLink()
            {
                Name = Path.GetFileNameWithoutExtension(shortcut.TargetPath),
                Arguments = shortcut.Arguments,
                TargetPath = shortcut.TargetPath,
                AppIcon = icon,
                StartInDirectory = shortcut.WorkingDirectory,
            };

            _allShortCuts.Add(appInfo);
        }
    }
    static ImageSource TryCreateIconWithFontAwesome(string targetPath)
    {
        ImageSource imageSource = null;
        if (targetPath.Contains("whatsapp", StringComparison.CurrentCultureIgnoreCase))
        {
            ClickWindow.Instance.Dispatcher.Invoke(() =>
            {
                imageSource = EFontAwesomeIcon.Brands_Whatsapp.CreateImageSource(Brushes.LightGreen);
            });
        }
        return imageSource;
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
                RECT rect;
                if (GetWindowRect(process.MainWindowHandle, out rect))
                {
                    bool windowsIsInsideBounds = rect.Bottom - rect.Top > 0 && rect.Right - rect.Left > 0;
                    if (windowsIsInsideBounds)
                    {
                        // Get the AutomationElement for the main window of the process
                        var element = AutomationElement.FromHandle(process.MainWindowHandle);
                        // skip if the element is not a window. This can happen for UWP apps (and maybe others).
                        if (element.Current.ControlType != ControlType.Window)
                        {
                            continue;
                        }
                    }
                }

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

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }
}
