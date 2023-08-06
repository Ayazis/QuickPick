using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.VirtualDesktop;

namespace Utilities
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    namespace Utilities.VirtualDesktop
    {
        public class DesktopTracker : IDisposable
        {
            private readonly IVirtualDesktopHelper _virtualDesktopHelper;
            private Timer _timer;

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
                Guid oldId = _virtualDesktopHelper.CurrentDesktopId;
                Guid newDesktopId = _virtualDesktopHelper.UpdateCurrentDesktopID();

                if (newDesktopId != oldId)
                {
                    Debug.WriteLine($"old {oldId}, new: {newDesktopId}");
                    _virtualDesktopHelper.CurrentDesktopId = newDesktopId;
                    OnDesktopChanged();
                }
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
    }
}
