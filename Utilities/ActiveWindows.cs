using QuickPick.Utilities.DesktopInterops;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public class ActiveWindows
{

    static ActiveWindows()
    {

    }

    public static IEnumerable<(IntPtr handle, Process process)> GetAllOpenWindows()
    {
        var currentDesktop = DesktopInterop.GetCurrentDesktopGuid();
        foreach (var process in Process.GetProcesses())
        {
            IntPtr[] windows = GetProcessWindows(process.Id);
            foreach (var windowHandle in windows)
            {
                if (windowHandle != IntPtr.Zero)
                {
                    if (DesktopInterop.IsWindowOnCurrentVirtualDesktop(windowHandle))
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
            var currentDesktop = DesktopInterop.GetCurrentDesktopGuid();
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            Process[] matchingProcesses = Process.GetProcessesByName(fileName);
            if (matchingProcesses == null || matchingProcesses.Length == 0)
                return default;

            foreach (var process in matchingProcesses)
            {
                var processWindows = GetProcessWindows(process.Id);
                foreach (IntPtr hWnd in processWindows)
                {
                    if (DesktopInterop.IsWindowOnCurrentVirtualDesktop(hWnd))
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
                var parent = GetParent(hWnd);
                if (parent != default)
                    return false;  // dont show childwindows ?

                if (HasIgnorableClassName(hWnd))
                    return false;

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
    static bool HasIgnorableClassName(IntPtr hWnd)
    {
        string className = GetClassName(hWnd);

        return _classnamesToIgnore.Contains(className);
    }
    static readonly string[] _classnamesToIgnore = new string[] { "WorkerW", "Progman", "SysShadow" };
    static string GetClassName(IntPtr hWnd)
    {
        const int maxClassNameLength = 256;
        StringBuilder className = new StringBuilder(maxClassNameLength);

        // Get the window class name
        int result = GetClassName(hWnd, className, className.Capacity);

        if (result == 0)
        {
            // An error occurred
            return null;
        }

        return className.ToString();
    }

    #region DllImports
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr GetParent(IntPtr hWnd);

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
