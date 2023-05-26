using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace Utilities;
public static class IconCreator
{
	public static ImageSource GetImage(string path)
	{
		var icon = ExtractIcon(path);
		if (icon == null)
			return null;
		return IconToImageSource(icon);
	}
	public static BitmapImage IconToImageSource(Icon icon)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			icon.ToBitmap().Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
			memoryStream.Position = 0;
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = memoryStream;
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.EndInit();
			bitmapImage.Freeze(); // Optional, but recommended for better performance
			return bitmapImage;
		}
	}

	private static Icon ExtractIcon(string targetPath)
	{
		IntPtr[] largeIcons = new IntPtr[1];
		IntPtr[] smallIcons = new IntPtr[1];

		ExtractIconEx(targetPath, 0, largeIcons, smallIcons, 1);

		if (largeIcons[0] != IntPtr.Zero)
		{
			Icon icon = Icon.FromHandle(largeIcons[0]);
			return icon;
		}
		else if (smallIcons[0] != IntPtr.Zero)
		{
			Icon icon = Icon.FromHandle(smallIcons[0]);
			return icon;
		}

		return null;
	}
	[DllImport("shell32.dll", CharSet = CharSet.Auto)]
	private static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
}
