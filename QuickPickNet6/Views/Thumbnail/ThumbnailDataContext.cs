using System;
using System.ComponentModel;
using ThumbnailLogic;

namespace QuickPick.UI.Views.Thumbnail;

public class ThumbnailDataContext : INotifyPropertyChanged
{

    public RECT Rect { get; private set; }

    private string _windowTitle = @" --- ";
    public IntPtr WindowHandle { get; private set; }
    public string WindowTitle
    {
        get { return _windowTitle; }
        set
        {
            _windowTitle = value;
            NotifyPropertyChanged(nameof(WindowTitle));
        }
    }

    public IntPtr ThumbnailRelation { get; private set; }

    public ThumbnailDataContext(IntPtr thumbnailRelation, RECT rect, IntPtr windowHandle, string windowTitle = null)
    {
        WindowTitle = string.IsNullOrEmpty(windowTitle) ? _windowTitle : windowTitle;
        WindowHandle = windowHandle;
        ThumbnailRelation = thumbnailRelation;
        Rect = rect;
    }
    public ThumbnailDataContext()
    {

    }

    #region Notify Property Changed And other Events
    public event PropertyChangedEventHandler PropertyChanged;

    public void NotifyPropertyChanged(string name)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

    #endregion
}
