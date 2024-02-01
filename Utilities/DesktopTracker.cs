using QuickPick.Utilities.DesktopInterops;
using System.Diagnostics;


namespace Utilities.VirtualDesktop;

public class DesktopTracker : IDisposable
{
    private Timer _timer;
    Guid _lastDesktopId = Guid.Empty;

    public event EventHandler DesktopChanged;

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
        Guid newDesktopId = DesktopInterop.GetCurrentDesktopGuid();
        if (newDesktopId == _lastDesktopId)
            return;

        Debug.WriteLine($"old {_lastDesktopId}, new: {newDesktopId}");
        _lastDesktopId = newDesktopId;
        OnDesktopChanged();
    }



    protected virtual void OnDesktopChanged()
    {
        DesktopChanged?.Invoke(this, null);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

