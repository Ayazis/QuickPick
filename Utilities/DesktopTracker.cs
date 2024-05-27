using QuickPick.Utilities.DesktopInterops;
using System.Diagnostics;


namespace QuickPick.Utilities.VirtualDesktop;
public interface IDesktopTracker
{
    event EventHandler DesktopChanged;

    void Dispose();
    void StartTracking();
    void StopTracking();
}
public class DesktopTracker : IDisposable, IDesktopTracker
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

