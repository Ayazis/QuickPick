using System.Runtime.InteropServices;

namespace Utilities.VirtualDesktop;



public interface IVirtualDesktopHelper
{
    Guid CurrentDesktopId { get; set; }
    bool IsWindowOnVirtualDesktop(IntPtr hWnd, Guid currentVirtualDesktopId);
    void Dispose();
}

public class VirtualDesktopHelper : IVirtualDesktopHelper
{
    private IVirtualDesktopManager _virtualDesktopManager;
    bool _isDisposed;

    public VirtualDesktopHelper()
    {

        _virtualDesktopManager = (IVirtualDesktopManager)new VirtualDesktopManager();

    }

    public void Dispose()
    {

        if (_isDisposed)
            return;
        Marshal.ReleaseComObject(_virtualDesktopManager);
        _virtualDesktopManager = null;
        GC.SuppressFinalize(this);
        _isDisposed = true;

    }
    public Guid CurrentDesktopId { get; set; }
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
    /*
    * documentation: https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-ivirtualdesktopmanager
     */

    // Declare the interface for the virtual desktop manager
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
    private interface IVirtualDesktopManager
    {
        int IsWindowOnCurrentVirtualDesktop(IntPtr hWnd);
        Guid GetWindowDesktopId(IntPtr hWnd);
    }

    // Declare the virtual desktop manager
    [ComImport]
    [Guid("aa509086-5ca9-4c25-8f95-589d3c07b48a")]
    private class VirtualDesktopManager
    {
    }


    #endregion
}
