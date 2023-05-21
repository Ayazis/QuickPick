using PInvoke;
using System.Runtime.InteropServices;
using System;
using System.Windows;

namespace ThumbnailLogic;

public class Thumbnails
{
    private const int DWM_TNP_VISIBLE = 0x8;
    private const int DWM_TNP_RECTDESTINATION = 0x1;
    private const int DWM_TNP_OPACITY = 0x4;    
    private static DWM_THUMBNAIL_PROPERTIES thumbnailProperties;

    [DllImport("dwmapi.dll")]
    public static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

    [DllImport("dwmapi.dll")]
    public static extern int DwmUnregisterThumbnail(IntPtr thumb);

    public static IntPtr GetThumbnailRelations(IntPtr hwndSource, IntPtr hwndDestination)
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

    internal static void CreateThumbnail(IntPtr thumbHandle, RECT target)
    {
        // Set the properties of the thumbnail.
        thumbnailProperties = new DWM_THUMBNAIL_PROPERTIES();
        thumbnailProperties.dwFlags = DWM_TNP_VISIBLE | DWM_TNP_RECTDESTINATION | DWM_TNP_OPACITY;
        thumbnailProperties.opacity = 255;
        thumbnailProperties.fVisible = true;
        thumbnailProperties.rcDestination = target; // Set the size and position of the thumbnail here.

        int result = DwmUpdateThumbnailProperties(thumbHandle, ref thumbnailProperties);
    }


    [DllImport("dwmapi.dll")]
    private static extern int DwmUpdateThumbnailProperties(IntPtr hThumbnailId, ref DWM_THUMBNAIL_PROPERTIES ptnProperties);
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
