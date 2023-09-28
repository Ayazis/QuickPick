using System.Runtime.InteropServices;

namespace QuickPick.Utilities
{
    public static class MonitorDpi
    {
        // Dictionary to store dpiX values for any hmonitor
        private static Dictionary<IntPtr, int> _monitorDpiCache = new Dictionary<IntPtr, int>();

        public static int GetDpi(IntPtr hmonitor)
        {
            if (_monitorDpiCache.ContainsKey(hmonitor))
            {
                return _monitorDpiCache[hmonitor];
            }
            else
            {
                uint dpiX, dpiY;
                int result = GetDpiForMonitor(hmonitor, MonitorDpiType.EffectiveDpi, out dpiX, out dpiY);

                if (result == 0) // S_OK
                {
                    _monitorDpiCache[hmonitor] = (int)dpiX;
                    return (int)dpiX;
                }
                else
                {
                    throw new InvalidOperationException("Failed to get DPI for monitor.");
                }
            }
        }

        [DllImport("shcore.dll")]
        private static extern int GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

        private enum MonitorDpiType
        {
            EffectiveDpi = 0,
            AngularDpi = 1,
            RawDpi = 2,
            Default = EffectiveDpi,
        }
    }
}
