using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        uint result = ExtractIconEx(targetPath, 0, largeIcons, smallIcons, 1);
        if (result == 0)
            return null;

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
