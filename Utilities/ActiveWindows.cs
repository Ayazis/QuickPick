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
    const UInt32 SWP_NOSIZE = 0x0001;
    const UInt32 SWP_NOMOVE = 0x0002;

    private const int SW_RESTORE = 9;
    static ActiveWindows()
    {
    }


    static void DisableAeroPeekForAllQuickPickWindowHandles()
    {

        var currentProcess = Process.GetCurrentProcess();
        // get all windowhandles for currentprocess
        IntPtr[] qpWindowHandles = GetProcessWindows(currentProcess.Id);

        foreach (var handle in qpWindowHandles)
        {
            DisableAeroPeekOnQuickPick(handle);
        }
    }
    private static void DisableAeroPeekOnQuickPick(IntPtr handleToDisable)
    {
        // Create a value that represents the DWMWA_EXCLUDED_FROM_PEEK attribute
        int exclude = (int)DwmWindowAttribute.DWMWA_EXCLUDED_FROM_PEEK;
        // Set the attribute for your window
        DwmSetWindowAttribute(handleToDisable, exclude, ref exclude, sizeof(int));
    }

    // A method to activate the window peek effect
    public static void ActivatePeek(IntPtr handle)
    {
        Trace.WriteLine("Activating Peek");
        // since everytime we activatea a peek we have new thumbnails that have window handles,
        // we need to disable aeropeek for all quickpick windows every time.

        DisableAeroPeekForAllQuickPickWindowHandles();

        // Check if DWM composition is enabled
        if (DwmIsCompositionEnabled())
        {
            // Get the handle of the main form            
            // Activate the window peek effect
            DwmpActivateLivePreview(1, handle, IntPtr.Zero, 3, 0);
        }
        var currentProcess = Process.GetCurrentProcess();
        // get all windowhandles for currentprocess
        IntPtr[] qpWindowHandles = GetProcessWindows(currentProcess.Id);

        foreach (var whandle in qpWindowHandles)
        {
            // Set the window position and flags
            SetWindowPos(whandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOSIZE | SWP_NOMOVE);
        }
    }

    // A method to deactivate the window peek effect
    public static void DeactivatePeek(IntPtr handle)
    {
        Debug.WriteLine("De-Activating Peek");
        Trace.WriteLine("De-Activating Peek");
        // Check if DWM composition is enabled
        if (DwmIsCompositionEnabled())
        {
            // Deactivate the window peek effect
            DwmpActivateLivePreview(0, handle, IntPtr.Zero, 3, 0);
        }
        //Task.Delay(300).Wait(); // take time to deactivate peek, otherwise peek mode will remain.
    }



    public static IEnumerable<(IntPtr handle, Process process)> GetAllOpenWindows()
    {        
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

    public static void ActivateWindow(IntPtr hWnd)
    {
        if (hWnd != IntPtr.Zero)
        {

            DeactivatePeek(hWnd); // always disactivate peek when activating a window.


            if (IsIconic(hWnd))
            {
                // Window is currently minimized - restore it
                ShowWindow(hWnd, SW_SHOWNOACTIVATE);
                SetForegroundWindow(hWnd);
            }
            else
            {
                // Window is currently restored - minimize it
                SetForegroundWindow(hWnd);
            }
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


    // Used to set application at topmost position.
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    const uint SWP_NOACTIVATE = 0x0010;

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
