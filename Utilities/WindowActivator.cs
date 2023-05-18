using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Utilities.VirtualDesktop;

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


    public static IntPtr GetActiveWindow(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);

        Process[] matchingProcesses = Process.GetProcessesByName(fileName);
        if (matchingProcesses == null || matchingProcesses.Length == 0)
            return default;        

        foreach (var process in matchingProcesses)
        {
            var processWindows = GetProcessWindows(process.Id);
            foreach (IntPtr hWnd in processWindows)
            {             
                if (VirtualDesktopHelper.IsWindowOnVirtualDesktop(hWnd))
                {                    
                    return hWnd;
                }
            }
        }

        return default;
    }    

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


    // Declare the IsIconic function from user32.dll
    [DllImport("user32.dll")]
    private static extern bool IsIconic(IntPtr hWnd);
    public static void ActivateWindow(IntPtr hWnd, int? showCommandInteger = null)
    {
		// Constants for ShowWindow
		const int SW_SHOWNOACTIVATE = 4;
		const int SW_MINIMIZE = 6;

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


    private static IntPtr[] GetProcessWindows(int processId)
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
}
