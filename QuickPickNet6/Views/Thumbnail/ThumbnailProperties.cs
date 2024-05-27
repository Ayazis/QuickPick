using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Drawing;
using System.Windows.Media;
using ThumbnailLogic;

namespace QuickPick.UI.Views.Thumbnail;

public partial class PreviewImageProperties : ObservableObject
{
    [ObservableProperty]
    ImageSource _icon;
    readonly double MAX_SIZE = 150;
    // todo: Set width and height according to the aspect ratio of the applicationWindow..
    [ObservableProperty]
    private double _width;
    [ObservableProperty]
    public double _height;

    public RECT Rect { get; private set; }
    [ObservableProperty]
    private string _windowTitle = @" --- ";
    public IntPtr WindowHandle { get; private set; }

    public double DpiScaling { get; private set; }


    public PreviewImageProperties(IntPtr windowHandle, string windowTitle, double dpiScaling, ImageSource icon)
    {
        MAX_SIZE = MAX_SIZE / dpiScaling;
        _icon = icon;
        DpiScaling = dpiScaling;
        double aspectRatio = WindowPreviewCreator.GetWindowAspectRatio(windowHandle);

        // Set height to fixedHeight and width to fixedHeight*aspectratio
        const int RoomForTitle = 15;
        Height = MAX_SIZE;
        Width = MAX_SIZE * aspectRatio;

        Height += RoomForTitle;

        WindowTitle = string.IsNullOrEmpty(windowTitle) ? _windowTitle : windowTitle;
        WindowHandle = windowHandle;
        //    MAX_SIZE = MAX_SIZE / dpiScaling;
        //    _icon = icon;
        //    DpiScaling = dpiScaling;
        //    double aspectRatio = WindowPreviewCreator.GetWindowAspectRatio(windowHandle);

        //    // If aspectratio is horizontal, set width to MAX_SIZE and height to MAX_SIZE/aspectratio
        //    // If aspectratio is vertical, set height to MAX_SIZE and width to MAX_SIZE*aspectratio
        //    const int RoomForTitle = 15;
        //    Width = aspectRatio > 1 ? MAX_SIZE : MAX_SIZE * aspectRatio;
        //    Height = aspectRatio > 1 ? MAX_SIZE / aspectRatio : MAX_SIZE;
        //    Height += RoomForTitle; 

        //    WindowTitle = string.IsNullOrEmpty(windowTitle) ? _windowTitle : windowTitle;
        //    WindowHandle = windowHandle;
        //
        }
        public PreviewImageProperties()
    {

    }
}
