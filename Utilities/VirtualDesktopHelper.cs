using System;
using System.Runtime.InteropServices;

public static class VirtualDesktopHelper
{
    // The GUID of the virtual desktop interface
    private static readonly Guid _virtualDesktopGuid = new Guid("aa509086-5ca9-4c25-8f95-589d3c07b48a");

    // Declare the interface for the virtual desktop manager
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
    private interface IVirtualDesktopManager
    {
        int IsWindowOnCurrentVirtualDesktop(IntPtr hWnd);
        Guid GetWindowDesktopId(IntPtr hWnd);
        void MoveWindowToDesktop(IntPtr hWnd, ref Guid desktopId);
    }

    // Declare the virtual desktop manager
    [ComImport]
    [Guid("aa509086-5ca9-4c25-8f95-589d3c07b48a")]
    private class VirtualDesktopManager
    {
    }

    public static Guid GetCurrentVirtualDesktop()
    {
        IVirtualDesktopManager virtualDesktopManager = (IVirtualDesktopManager)new VirtualDesktopManager();
        IntPtr hwnd = IntPtr.Zero;

        // Get the current window handle
        hwnd = GetForegroundWindow();

        // Get the desktop ID for the current window
        Guid desktopId = virtualDesktopManager.GetWindowDesktopId(hwnd);

        return desktopId;
    }

    public static bool IsWindowOnVirtualDesktop(IntPtr hWnd, Guid currentVirtualDesktopId)
    {
        try
        {
            IVirtualDesktopManager virtualDesktopManager = (IVirtualDesktopManager)new VirtualDesktopManager();
            Guid windowDesktopId = virtualDesktopManager.GetWindowDesktopId(hWnd);

            return windowDesktopId == currentVirtualDesktopId;
        }
        catch (Exception)
        {
            return false;
            
        }
    }

    // Declare the Windows API functions for getting the foreground window handle
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
}
