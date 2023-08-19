using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Utilities.VirtualDesktop;

public class ActiveWindows
{
    private static IVirtualDesktopHelper _virtualDesktopHelper;

    public static void SetVirtualDesktopHelper(IVirtualDesktopHelper virtualDesktopWrapper)
    {
        _virtualDesktopHelper = virtualDesktopWrapper;
    }


    public static IEnumerable<(IntPtr handle, Process process)> GetAllOpenWindows()
    {
        var currentDesktop = _virtualDesktopHelper.CurrentDesktopId;
        foreach (var process in Process.GetProcesses())
        {
            IntPtr[] windows = GetProcessWindows(process.Id);
            foreach (var windowHandle in windows)
            {
                if (windowHandle != IntPtr.Zero)
                {
                    if (_virtualDesktopHelper.IsWindowOnVirtualDesktop(windowHandle, currentDesktop))
                    {
                        yield return (windowHandle, process);
                    }
                }
            }  
        }
    }
    public static IntPtr GetActiveWindowOnCurentDesktop(string filePath)
    {
        try
        {
            var currentDesktop = _virtualDesktopHelper.CurrentDesktopId;

            string fileName = Path.GetFileNameWithoutExtension(filePath);

            Process[] matchingProcesses = Process.GetProcessesByName(fileName);
            if (matchingProcesses == null || matchingProcesses.Length == 0)
                return default;

            foreach (var process in matchingProcesses)
            {
                var processWindows = GetProcessWindows(process.Id);
                foreach (IntPtr hWnd in processWindows)
                {
                    if (_virtualDesktopHelper.IsWindowOnVirtualDesktop(hWnd, currentDesktop))
                    {
                        return hWnd;
                    }
                }
            }

            return IntPtr.Zero;
        }
        catch (Exception)
        {
            return IntPtr.Zero;
        }
    }
    public static void ToggleWindow(IntPtr hWnd)
    {
        // Constants for ShowWindow, from Microsoft documentation.
        const int SW_SHOWNOACTIVATE = 4;
        const int SW_MINIMIZE = 6;

        if (hWnd != IntPtr.Zero)
        {
            if (IsIconic(hWnd))
            {
                // Window is currently minimized - restore it
                ShowWindow(hWnd, SW_SHOWNOACTIVATE);
                SetForegroundWindow(hWnd);
            }
            else
            {
                // Window is currently restored - minimize it
                ShowWindow(hWnd, SW_MINIMIZE);
            }
        }
    }
    public static IntPtr[] GetProcessWindows(int processId)
    {
        var windows = new List<IntPtr>();

        EnumWindows((hWnd, lParam) =>
        {
            GetWindowThreadProcessId(hWnd, out uint windowProcessId);
            if (windowProcessId == processId && IsWindowVisible(hWnd))
            {
                windows.Add(hWnd);
            }
            return true;
        }, IntPtr.Zero);

        return windows.ToArray();
    }


    public static string GetWindowTitle(IntPtr hwnd)
    {
        int length = GetWindowTextLength(hwnd);
        if (length == 0)
            return string.Empty;

        StringBuilder title = new StringBuilder(length + 1);
        GetWindowText(hwnd, title, title.Capacity);

        return title.ToString();
    }

    #region DllImports

    // Import the user32.dll library
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern int GetWindowTextLength(IntPtr hwnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool IsWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindowVisible(IntPtr hWnd);
    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool IsIconic(IntPtr hWnd);
    #endregion
}
