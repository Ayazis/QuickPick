using System;
using System.Runtime.InteropServices;

namespace Utilities.VirtualDesktop;
public interface IVirtualDesktopHelper
{
    Guid CurrentDesktopId { get; set; }
    Guid UpdateCurrentDesktopID();
    bool IsWindowOnVirtualDesktop(IntPtr hWnd, Guid currentVirtualDesktopId);
}

public class VirtualDesktopHelper : IVirtualDesktopHelper
{
    private IVirtualDesktopManager _virtualDesktopManager = (IVirtualDesktopManager)new VirtualDesktopManager();


    public Guid CurrentDesktopId { get; set; }    
    public Guid UpdateCurrentDesktopID()
    {        
        Thread newThread = new Thread(() =>
        {
            IntPtr hwnd = IntPtr.Zero;

            // Get the current window handle
            hwnd = GetForegroundWindow();

            // Get the desktop ID for the current window
            try
            {
                Guid desktopId = _virtualDesktopManager.GetWindowDesktopId(hwnd);
                CurrentDesktopId = desktopId;
            }
            catch (Exception e)
            {
                // when  HWND is not an active  window?
                // try again later.
            }


        });

        newThread.SetApartmentState(ApartmentState.STA); // Set the thread to STA, needed for some COM objects, and necessary in this case.
        newThread.Start();
        newThread.Join();
        return CurrentDesktopId;
    }

    public bool IsWindowOnVirtualDesktop(IntPtr hWnd, Guid currentVirtualDesktopId)
    {
        if (currentVirtualDesktopId == default)
            return false;

        Guid desktopIdForWindow = Guid.Empty;
        Thread newThread = new Thread(() =>
        {
            try
            {
                desktopIdForWindow = _virtualDesktopManager.GetWindowDesktopId(hWnd);
            }
            catch (COMException e)
            {
                // Random guid will return false.
                desktopIdForWindow = Guid.NewGuid();
            }
        });

        newThread.SetApartmentState(ApartmentState.STA); // Set the thread to STA, needed for some COM objects, and necessary in this case.
        newThread.Start();
        newThread.Join();
        return desktopIdForWindow == currentVirtualDesktopId;
    }

    #region ComImports
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
    // Declare the Windows API functions for getting the foreground window handle
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    #endregion
}
