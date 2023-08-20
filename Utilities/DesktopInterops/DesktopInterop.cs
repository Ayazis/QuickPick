using System;
using System.Linq;

namespace QuickPick.Utilities.DesktopInterops
{
    public class DesktopInterop
    {
        public static Guid GetCurrentDesktopGuid()
        {
            if (OsVersionChecker.IsWindows11Eligable)
                return DesktopWin11.VirtualDesktopManagerInternal.GetCurrentDesktop(IntPtr.Zero).GetId();
            else
                return DesktopWin10.VirtualDesktopManagerInternal.GetCurrentDesktop().GetId();
        }
        public static bool IsWindowOnCurrentVirtualDesktop(IntPtr topLevelWindow)
        {
            if (OsVersionChecker.IsWindows11Eligable)
                return DesktopWin11.VirtualDesktopManager.IsWindowOnCurrentVirtualDesktop(topLevelWindow);
            else
                return DesktopWin10.VirtualDesktopManager.IsWindowOnCurrentVirtualDesktop(topLevelWindow);
        }
    }
}
