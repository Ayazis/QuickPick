namespace Utilities.VirtualDesktop;

public static class VirtualDesktopHelper
{
	public static bool IsWindowOnVirtualDesktop(IntPtr hwnd)
	{
		return WindowsDesktop.VirtualDesktop.IsCurrentVirtualDesktop(hwnd);
	}
}