namespace Utilities.VirtualDesktop;

public static class VirtualDesktopWrapper
{
    public static bool IsWindowOnVirtualDesktop(IntPtr hwnd)
    {
        bool isOnCurrentDesktop = false;
        Thread thread = new Thread(() =>
        {
            isOnCurrentDesktop = WindowsDesktop.VirtualDesktop.IsCurrentVirtualDesktop(hwnd);
            // Do something with the result
        });

        thread.SetApartmentState(ApartmentState.STA); // Set the thread's apartment state to STA
        thread.Start(); // Start the thread

        thread.Join(); // Wait for the thread to complete

        return isOnCurrentDesktop;
    }
}