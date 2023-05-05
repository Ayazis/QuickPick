using System;
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

    private static int SW_SHOWNOACTIVATE = 4;

    public static void ActivateWindow(IntPtr hWnd, int? showCommandInteger = null)
    {
        if (hWnd != IntPtr.Zero)
        {
            ShowWindow(hWnd, showCommandInteger ?? SW_SHOWNOACTIVATE);
            SetForegroundWindow(hWnd);
        }
    }
    private static Process[] GetProcessesByExecutablePath(string executablePath)
    {
        return Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(executablePath));
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
