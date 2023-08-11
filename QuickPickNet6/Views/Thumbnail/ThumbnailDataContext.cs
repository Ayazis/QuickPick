using System;
using ThumbnailLogic;

namespace QuickPick.UI.Views.Thumbnail;

public class ThumbnailDataContext
{

    public RECT Rect { get; private set; }

    public string WindowTitle { get; private set; } = "SomeWindowText";
    public IntPtr ThumbnailRelation { get; private set; }

    public ThumbnailDataContext(string windowTitle, IntPtr thumbnailRelation, RECT rect)
    {
        WindowTitle = windowTitle;
        ThumbnailRelation = thumbnailRelation;
        Rect = rect;
    }
    public ThumbnailDataContext()
    {
            
    }
}
