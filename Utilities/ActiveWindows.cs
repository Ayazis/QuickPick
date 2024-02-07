using QuickPick.Utilities.DesktopInterops;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

public class ActiveWindows
{
    // Constants for ShowWindow, from Microsoft documentation.
    const int SW_SHOWNOACTIVATE = 4;
    const int SW_MINIMIZE = 6;
    private const int SW_RESTORE = 9;
    private static IntPtr _quickPickHandle;

    static ActiveWindows()
    {        
    }

    static void setQuickPickHandleAndDisableAeroPeek()
    {
        _quickPickHandle = Process.GetCurrentProcess().MainWindowHandle;
        DisableAeroPeekOnQuickPick();
    }
    private static void DisableAeroPeekOnQuickPick()
    {        
        // Create a value that represents the DWMWA_EXCLUDED_FROM_PEEK attribute
        int attrValue = (int)DwmWindowAttribute.DWMWA_EXCLUDED_FROM_PEEK;
        // Set the attribute for your window
        DwmSetWindowAttribute(_quickPickHandle, attrValue, ref attrValue, sizeof(int));
    }

    // A method to activate the window peek effect
    public static void ActivatePeek(IntPtr  handle)
    {
        if (_quickPickHandle == default)
            setQuickPickHandleAndDisableAeroPeek();

        // Check if DWM composition is enabled
        if (DwmIsCompositionEnabled())
        {
            // Get the handle of the main form            
            // Activate the window peek effect
            DwmpActivateLivePreview(1, handle, _quickPickHandle, 3, 0);
        }
    }

    // A method to deactivate the window peek effect
    public static void DeactivatePeek(IntPtr handle)
    {
        // Check if DWM composition is enabled
        if (DwmIsCompositionEnabled())
        {
            // Deactivate the window peek effect
            DwmpActivateLivePreview(0, handle, _quickPickHandle, 3, 0);
        }
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


    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern bool DwmIsCompositionEnabled(); // To Check if AeroPeek is Enabled

    [DllImport("dwmapi.dll", EntryPoint = "#113", SetLastError = true)]
    internal static extern uint DwmpActivateLivePreview(uint peekOn, IntPtr hPeekWindow, IntPtr hTopmostWindow, uint peekType1or3, uint newForWin10); // To Activate LivePreview
    
    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize); // used to prevent mainApp from dissapearing when aeropeeking
    
    [Flags] // used by DwmSetWindowAttribute
    public enum DwmWindowAttribute : uint
    {
        DWMWA_NCRENDERING_ENABLED = 1,
        DWMWA_NCRENDERING_POLICY,
        DWMWA_TRANSITIONS_FORCEDISABLED,
        DWMWA_ALLOW_NCPAINT,
        DWMWA_CAPTION_BUTTON_BOUNDS,
        DWMWA_NONCLIENT_RTL_LAYOUT,
        DWMWA_FORCE_ICONIC_REPRESENTATION,
        DWMWA_FLIP3D_POLICY,
        DWMWA_EXTENDED_FRAME_BOUNDS,
        DWMWA_HAS_ICONIC_BITMAP,
        DWMWA_DISALLOW_PEEK,
        DWMWA_EXCLUDED_FROM_PEEK,
        DWMWA_LAST
    }

    public static void CloseWindow(IntPtr windowHandle)
    {
        try
        {
            const UInt32 WM_CLOSE = 0x0010;
            SendMessage(windowHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        catch (Exception)
        {
            // Lowlevel call exception.
            // Todo: Log but functionally ignore.
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

    #endregion
}
