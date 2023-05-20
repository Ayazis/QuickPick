namespace Utilities.VirtualDesktop;

public interface IVirtualDesktopWrapper
{
    bool IsWindowOnVirtualDesktop(IntPtr hwnd);
}

public class VirtualDesktopWrapper : IVirtualDesktopWrapper
{
    public bool IsWindowOnVirtualDesktop(IntPtr hwnd)
    {        
        bool isOnCurrentDesktop = false;
        //These calls need to be done in an STA thread to avoid exceptions when handling events.
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