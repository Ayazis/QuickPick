using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace QuickPick.Logic.ActiveApps
{

	public class ThumbnailGetter
	{
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

		[DllImport("dwmapi.dll")]
		public static extern int DwmGetWindowAttribute(IntPtr hwnd, uint dwAttribute, out RECT pvAttribute, int cbAttribute);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		public static Image GetThumbnail(IntPtr hWnd)
		{
			if (IsIconic(hWnd) || !IsWindowVisible(hWnd))
			{
				return null;
			}
			RECT rect;
			DwmGetWindowAttribute(hWnd, 9, out rect, Marshal.SizeOf(typeof(RECT)));

			int width = rect.right - rect.left;
			int height = rect.bottom - rect.top;

			if (width == 0 || height == 0)
				return null;

			Bitmap bmp = new Bitmap(width, height);
			Graphics gfx = Graphics.FromImage(bmp);

			IntPtr hdc = gfx.GetHdc();
			bool success = PrintWindow(hWnd, hdc, 0);
			gfx.ReleaseHdc(hdc);

			if (success)
			{
				return bmp;
			}
			else
			{
				bmp.Dispose();
				return null;
			}
		}

		[DllImport("user32.dll")]
		public static extern bool IsIconic(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);
	}

}