using QuickPick.Utilities;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MouseAndKeyBoardHooks;

public class MousePosition
{
    public static Point GetCursorPosition()
    {
        Point mousePos = new Point();
        GetCursorPos(ref mousePos);

        IntPtr hMonitor = MonitorFromPoint(mousePos, MonitorOptions.MONITOR_DEFAULTTONEAREST);
        var dpi = MonitorHelper.GetDpi(hMonitor);
        float scale = dpi / 96f;

        int mouseX = (int)(mousePos.X / scale);
        int mouseY = (int)(mousePos.Y / scale);

        return new Point(mouseX, mouseY);
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(ref Point lpPoint);

    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromPoint(Point pt, MonitorOptions dwFlags);

    [Flags]
    private enum MonitorOptions
    {
        MONITOR_DEFAULTTONULL = 0x00000000,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002
    }
}