using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class WindowActivator
{
    // Windows API imports
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    public static void ActivateWindowOnCurrentVirtualDesktop(string executablePath)
    {
        var matchingProcesses = GetProcessesByExecutablePath(executablePath);
        if (matchingProcesses.Length == 0)
        {
            // Start the process if it is not already running
            Process.Start(executablePath);
            return;
        }


        Guid currentVirtualDesktopId = GetCurrentVirtualDesktop();

        foreach (var process in matchingProcesses)
        {
            var processWindows = GetProcessWindows(process.Id);
            foreach (var hWnd in processWindows)
            {
                if (VirtualDesktopHelper.IsWindowOnVirtualDesktop(hWnd, currentVirtualDesktopId))
                {
                    ActivateWindow(hWnd);
                    return;
                }
            }
        }
    }
    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    // Constants for ShowWindow
    private static int SW_SHOWNOACTIVATE = 4;
    const int SW_MINIMIZE = 6;

    // Declare the IsIconic function from user32.dll
    [DllImport("user32.dll")]
    private static extern bool IsIconic(IntPtr hWnd);
    public static void ActivateWindow(IntPtr hWnd, int? showCommandInteger = null)
    {
        if (hWnd != IntPtr.Zero)
        {
            if (IsIconic(hWnd))
            { 
                // Window is currently minimized - restore it
                ShowWindow(hWnd, showCommandInteger ?? SW_SHOWNOACTIVATE);
                SetForegroundWindow(hWnd);
            }
            else
            {
                // Window is currently restored - minimize it
                ShowWindow(hWnd, SW_MINIMIZE);
            }
        }
    }
    private static Process[] GetProcessesByExecutablePath(string executablePath)
    {
        var processes = Process.GetProcesses();
        var matchingProcesses = new List<Process>();

        foreach (var process in processes)
        {
            try
            {
                if (process.MainModule.FileName.Equals(executablePath, StringComparison.OrdinalIgnoreCase))
                {
                    matchingProcesses.Add(process);
                }
            }
            catch (Win32Exception)
            {
                // Ignore processes that we don't have access to
            }
        }

        return matchingProcesses.ToArray();
    }

    private static IntPtr[] GetProcessWindows(int processId)
    {
        var windows = new System.Collections.Generic.List<IntPtr>();

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

    private static Guid GetCurrentVirtualDesktop()
    {
        return VirtualDesktopHelper.GetCurrentVirtualDesktop();
    }
}
