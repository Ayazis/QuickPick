using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickPick.Logic
{
    public class MousePosition
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromPoint(Point pt, MonitorOptions dwFlags);

        [DllImport("shcore.dll")]
        public static extern int GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

        [Flags]
        public enum MonitorOptions
        {
            MONITOR_DEFAULTTONULL = 0x00000000,
            MONITOR_DEFAULTTOPRIMARY = 0x00000001,
            MONITOR_DEFAULTTONEAREST = 0x00000002
        }

        public enum MonitorDpiType
        {
            MDT_EFFECTIVE_DPI = 0,
            MDT_ANGULAR_DPI = 1,
            MDT_RAW_DPI = 2,
            MDT_DEFAULT = MDT_EFFECTIVE_DPI
        }

        public static Point GetCursorPosition()
        {
            Point mousePos = new Point();
            GetCursorPos(ref mousePos);

            IntPtr hMonitor = MonitorFromPoint(mousePos, MonitorOptions.MONITOR_DEFAULTTONEAREST);

            uint dpiX, dpiY;
            GetDpiForMonitor(hMonitor, MonitorDpiType.MDT_EFFECTIVE_DPI, out dpiX, out dpiY);

            float scale = (float)dpiX / 96f;

            int mouseX = (int)(mousePos.X / scale);
            int mouseY = (int)(mousePos.Y / scale);

            return new Point(mouseX, mouseY);
        }
    }
}