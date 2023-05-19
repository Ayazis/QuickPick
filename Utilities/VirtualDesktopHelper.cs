namespace Utilities.VirtualDesktop;

public static class VirtualDesktopWrapper
{
    public static bool IsWindowOnVirtualDesktop(IntPtr hwnd)
    {
        bool isOnCurrentDesktop = false;
        Thread thread = new Thread(() =>
        {
            isOnCurrentDesktop = WindowsDesktop.VirtualDesktop.IsCurrentVirtualDesktop(hwnd);            
        });

        thread.SetApartmentState(ApartmentState.STA); 
        thread.Start();

        thread.Join(); // Wait for the thread to complete

        return isOnCurrentDesktop;
    }
}