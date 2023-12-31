using CommunityToolkit.Mvvm.ComponentModel;
using System;
using ThumbnailLogic;

namespace QuickPick.UI.Views.Thumbnail;

public partial class ThumbnailProperties : ObservableObject

{
    public double Width { get; set; }
    public double Height { get; set; }

    public RECT Rect { get; private set; }
    [ObservableProperty]
    private string _windowTitle = @" --- ";
    public IntPtr WindowHandle { get; private set; }

    public IntPtr ThumbnailRelation { get; private set; }

    public ThumbnailProperties(IntPtr thumbnailRelation, RECT rect, IntPtr windowHandle, string windowTitle = null)
    {
        WindowTitle = string.IsNullOrEmpty(windowTitle) ? _windowTitle : windowTitle;
        WindowHandle = windowHandle;
        ThumbnailRelation = thumbnailRelation;
        Rect = rect;
    }
    public ThumbnailProperties()
    {

    }
}
