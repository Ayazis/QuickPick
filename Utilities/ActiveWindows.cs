﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Utilities.VirtualDesktop;
using WindowsDesktop;

public class ActiveWindows
{
    private static IVirtualDesktopWrapper _virtualDesktopWrapper;

    public ActiveWindows(IVirtualDesktopWrapper virtualDesktopWrapper)
    {
        _virtualDesktopWrapper = virtualDesktopWrapper;
    } 
    
    static ActiveWindows()
    {
        
        _virtualDesktopWrapper = new VirtualDesktopWrapper();   
    }


    public static IEnumerable<Process> GetAllOpenWindows()
    {
        foreach (var process in Process.GetProcesses()
               .Where(w => IsWindow(w.MainWindowHandle)
            && !string.IsNullOrEmpty(w.MainWindowTitle)))
        {
            IntPtr hWnd = process.MainWindowHandle;
            if (hWnd != IntPtr.Zero)
            {
                if (_virtualDesktopWrapper.IsWindowOnVirtualDesktop(hWnd))
                {
                    yield return process;
                }
            }
        }
    }
    public static IntPtr GetActiveWindowOnCurentDesktop(string filePath)
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
                if (_virtualDesktopWrapper.IsWindowOnVirtualDesktop(hWnd))
                {
                    return hWnd;
                }
            }
        }

        return default;
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


    #region DllImports
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
