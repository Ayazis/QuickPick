using System.Runtime.InteropServices;
using System.Text;

namespace QuickPick.Utilities.DesktopInterops
{
    public class DesktopInterop
    {
       static bool VirtualDesktopManagerFailure = false;
        public static Guid GetCurrentDesktopGuid()
        {
            if(VirtualDesktopManagerFailure)
                return default;

            try
            {
                if (OsVersionChecker.IsWindows11Eligable)
                    return DesktopWin11.VirtualDesktopManagerInternal.GetCurrentDesktop().GetId();
                else
                    return DesktopWin10.VirtualDesktopManagerInternal.GetCurrentDesktop().GetId();
            }
            catch (TypeInitializationException)
            {
                VirtualDesktopManagerFailure = true;
                // most probably a mismatch between pre-defined interface and actual interface in local .dll.
                return default;
                
            }
        }
        public static bool IsWindowOnCurrentVirtualDesktop(IntPtr windowHandle)
        {
            string title = GetWindowTitle(windowHandle);
            if (string.IsNullOrWhiteSpace(title))
                return false;

            try
            {
                if (VirtualDesktopManagerFailure)
                    return true;

                if (OsVersionChecker.IsWindows11Eligable)
                    return DesktopWin11.VirtualDesktopManager.IsWindowOnCurrentVirtualDesktop(windowHandle);
                else
                    return DesktopWin10.VirtualDesktopManager.IsWindowOnCurrentVirtualDesktop(windowHandle);
            }
            catch (TypeInitializationException)
            {
                VirtualDesktopManagerFailure = true;
                // interface-local .dll mismatch
                return true; // at least we keep functionality.
            }
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public static string GetWindowTitle(IntPtr hWnd)
        {
            // Get the length of the window text
            int length = GetWindowTextLength(hWnd);
            if (length == 0) return null;

            StringBuilder sb = new StringBuilder(length + 1);

            // Get the window text
            GetWindowText(hWnd, sb, sb.Capacity);

            return sb.ToString();
        }

    }
}
