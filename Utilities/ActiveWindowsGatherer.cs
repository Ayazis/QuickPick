using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Utilities
{
    public static class ActiveApps
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool IsWindow(IntPtr hWnd);

        public static IEnumerable<Process> GetAllOpenWindows()
        {
            foreach (var process in Process.GetProcesses()
                   .Where(w => IsWindow(w.MainWindowHandle)
                && !string.IsNullOrEmpty(w.MainWindowTitle)))
            {
                IntPtr hWnd = process.MainWindowHandle;
                if (hWnd != IntPtr.Zero)
                {
                    if (VirtualDesktop.VirtualDesktopWrapper.IsWindowOnVirtualDesktop(hWnd))
                    {
                        yield return process;
                    }
                }
            }
        }
    }
}
