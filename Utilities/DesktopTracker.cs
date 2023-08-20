using QuickPick.Utilities.DesktopInterops;
using System.Diagnostics;


namespace Utilities.VirtualDesktop;

public class DesktopTracker : IDisposable
{
    private readonly IVirtualDesktopHelper _virtualDesktopHelper;
    private Timer _timer;
    Guid _lastDesktopId = Guid.Empty;

    public event EventHandler DesktopChanged;

    public DesktopTracker(IVirtualDesktopHelper virtualDesktopHelper)
    {
        _virtualDesktopHelper = virtualDesktopHelper ?? throw new ArgumentNullException(nameof(virtualDesktopHelper));
    }

    public void StartTracking()
    {
        _timer = new Timer(CheckDesktopChange, null, 0, 500);
    }

    public void StopTracking()
    {
        _timer?.Dispose();
    }

    private void CheckDesktopChange(object state)
    {
        Guid newDesktopId = GetCurrentDesktopGuid();
        if (newDesktopId == _lastDesktopId)
            return;

        Debug.WriteLine($"old {_lastDesktopId}, new: {newDesktopId}");
        _lastDesktopId = newDesktopId;
        _virtualDesktopHelper.CurrentDesktopId = newDesktopId;
        OnDesktopChanged();
    }

    private static Guid GetCurrentDesktopGuid()
    {
        Guid newDesktopId;
        if (OsVersionChecker.IsWindows11Eligable)
            newDesktopId = Desktop_Win11.Current.Id;
        else
            newDesktopId = Desktop_Win10.Current.Id;
        return newDesktopId;
    }

    protected virtual void OnDesktopChanged()
    {
        DesktopChanged?.Invoke(this, null);
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _virtualDesktopHelper?.Dispose();
    }
}

