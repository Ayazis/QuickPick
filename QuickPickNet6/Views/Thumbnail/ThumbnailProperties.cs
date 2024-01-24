using CommunityToolkit.Mvvm.ComponentModel;
using System;
using ThumbnailLogic;

namespace QuickPick.UI.Views.Thumbnail;

public partial class ThumbnailProperties : ObservableObject
{
    // todo: Set width and height according to the aspect ratio of the applicationWindow..
    public double Width { get; private set; } = 200;
    public double Height { get; private set; } = 200;

    public RECT Rect { get; private set; }
    [ObservableProperty]
    private string _windowTitle = @" --- ";
    public IntPtr WindowHandle { get; private set; }

    

    public ThumbnailProperties(IntPtr windowHandle, string windowTitle = null)
    {   
        double aspectRatio = WindowPreviewCreator.GetWindowAspectRatio(windowHandle);

        // If aspectratio is horizontal, set width to 200 and height to 200/aspectratio
        // If aspectratio is vertical, set height to 200 and width to 200*aspectratio
        Width = aspectRatio > 1 ? 200 : 200 * aspectRatio;
        Height = aspectRatio > 1 ? 200 / aspectRatio : 200;
        
        WindowTitle = string.IsNullOrEmpty(windowTitle) ? _windowTitle : windowTitle;
        WindowHandle = windowHandle;
    }
    public ThumbnailProperties()
    {

    }
}
