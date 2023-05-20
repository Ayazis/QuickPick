namespace Utilities.VirtualDesktop;

public interface IVirtualDesktopWrapper
{
    bool IsWindowOnVirtualDesktop(IntPtr hwnd);
}

public class VirtualDesktopWrapper : IVirtualDesktopWrapper
{
    /// <summary>
    /// Wrapper for VirtualDesktop. These calls need to be done in an STA thread to avoid exceptions when handling events.
    /// </summary>
    /// <param name="hwnd"></param>
    /// <returns></returns>
    public bool IsWindowOnVirtualDesktop(IntPtr hwnd)
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