using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ThumbnailLogic;

public class WindowPreviewCreator
{
    private const int DWM_TNP_VISIBLE = 0x8;
    private const int DWM_TNP_RECTDESTINATION = 0x1;
    private const int DWM_TNP_OPACITY = 0x4;
    private static DWM_THUMBNAIL_PROPERTIES thumbnailProperties;



    public static IntPtr GetPreviewImagePointer(IntPtr hwndSource, IntPtr hwndDestination)
    {
        IntPtr thumb = IntPtr.Zero;

        int result = DwmRegisterThumbnail(hwndDestination, hwndSource, out thumb);

        if (result == 0 && thumb != IntPtr.Zero)
        {
            // Successfully registered thumbnail, do something with it

            return thumb;

            // Unregister thumbnail when done
            //DwmUnregisterThumbnail(thumb);
        }
        else
        {
            return default;// Failed to register thumbnail
        }
    }

    public static void CreateAndFadeInPreviewImage(IntPtr thumbnailId, RECT rect, bool fadeIn = false)
    {

        DWM_THUMBNAIL_PROPERTIES properties = new DWM_THUMBNAIL_PROPERTIES();
        properties.fVisible = true;
        properties.dwFlags = DWM_TNP_VISIBLE | DWM_TNP_RECTDESTINATION | DWM_TNP_OPACITY;
        properties.opacity = (byte)0;  // Start with a completely transparent thumbnail.
        properties.rcDestination = rect;

        if (fadeIn)
        {
            // Gradually increase the opacity over time to create a fade-in effect.
            for (int i = 0; i <= 255; i += 25)
            {
                properties.opacity = (byte)i;
                DwmUpdateThumbnailProperties(thumbnailId, ref properties);

                // Sleep for a bit to control the speed of the fade-in. Adjust this value as needed.
                Thread.Sleep(20);
            }
        }
        
        // Ensure the thumbnail is fully visible.
        properties.opacity = (byte)255;
        var result = DwmUpdateThumbnailProperties(thumbnailId, ref properties);


    }

    public static double GetWindowAspectRatio(IntPtr currentWindowHandle)
    {
        const int SW_MAXIMIZE = 3;
        WINDOWPLACEMENT windowPlacement = new WINDOWPLACEMENT();
        windowPlacement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));

        if (!GetWindowPlacement(currentWindowHandle, out windowPlacement))
            return 1; // Default aspect ratio

        RECT rect;

        if (windowPlacement.showCmd == SW_MAXIMIZE)
        {
            // If window is maximized, get the current size
            GetWindowRect(currentWindowHandle, out rect);
        }
        else
        {
            // Else use the size from rcNormalPosition
            rect = windowPlacement.rcNormalPosition;
        }

        double windowWidth = rect.Right - rect.Left;
        double windowHeight = rect.Bottom - rect.Top;

        if (windowHeight == 0)
            return 1; // Default aspect ratio

        return windowWidth / windowHeight;
    }

    #region dllImports
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

    [DllImport("dwmapi.dll")]
    private static extern int DwmUpdateThumbnailProperties(IntPtr hThumbnailId, ref DWM_THUMBNAIL_PROPERTIES ptnProperties);


    [DllImport("dwmapi.dll")]
    public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);


    [DllImport("dwmapi.dll")]
    public static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

    [DllImport("dwmapi.dll")]
    public static extern int DwmUnregisterThumbnail(IntPtr thumb);
    #endregion
}
[StructLayout(LayoutKind.Sequential)]
public struct WINDOWPLACEMENT
{
    public int length;
    public int flags;
    public int showCmd;
    public POINT ptMinPosition;
    public POINT ptMaxPosition;
    public RECT rcNormalPosition;
}

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int X;
    public int Y;
}

[StructLayout(LayoutKind.Sequential)]
public struct DWM_THUMBNAIL_PROPERTIES
{
    public int dwFlags;
    public RECT rcDestination;
    public RECT rcSource;
    public byte opacity;
    public bool fVisible;
    public bool fSourceClientAreaOnly;
}

[StructLayout(LayoutKind.Sequential)]
public struct RECT
{
    public RECT()
    {

    }
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public RECT(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
}
