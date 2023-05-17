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

    private static IVirtualDesktopManager _virtualDesktopManager = (IVirtualDesktopManager)new VirtualDesktopManager();

    public static Guid GetCurrentVirtualDesktop()
    {
        Guid currentDeskTopGuid = Guid.Empty;
        Thread newThread = new Thread(() =>
        {            
            IntPtr hwnd = IntPtr.Zero;

            // Get the current window handle
            hwnd = GetForegroundWindow();

            // Get the desktop ID for the current window
            try
            {
                Guid desktopId = _virtualDesktopManager.GetWindowDesktopId(hwnd);
                currentDeskTopGuid = desktopId;
            }
            catch (Exception e)
            {
                // when  HWND is not an active  window?
                currentDeskTopGuid = Guid.Empty;                
            }

            
        });

        newThread.SetApartmentState(ApartmentState.STA); // Set the thread to STA, needed for some COM objects, and necessary in this case.
        newThread.Start();
        newThread.Join();
        return currentDeskTopGuid;
        
    }



    public static bool IsWindowOnVirtualDesktop(IntPtr hWnd, Guid currentVirtualDesktopId)
    {
        if (currentVirtualDesktopId == default)
            return false;

        Guid windowDesktopId = Guid.Empty;
		Thread newThread = new Thread(() =>
		{			
			try
			{
				windowDesktopId = _virtualDesktopManager.GetWindowDesktopId(hWnd);
			}
			catch (Exception e)
			{
                // Random guid will return false.
				windowDesktopId = Guid.NewGuid();
			}
		});

		newThread.SetApartmentState(ApartmentState.STA); // Set the thread to STA, needed for some COM objects, and necessary in this case.
		newThread.Start();
		newThread.Join();
		return windowDesktopId == currentVirtualDesktopId;	
    }

    // Declare the Windows API functions for getting the foreground window handle
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();


}
