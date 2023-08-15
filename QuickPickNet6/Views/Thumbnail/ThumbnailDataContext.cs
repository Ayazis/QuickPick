using System;
using System.ComponentModel;
using ThumbnailLogic;

namespace QuickPick.UI.Views.Thumbnail;

public class ThumbnailDataContext : INotifyPropertyChanged
{

    public RECT Rect { get; private set; }

    private string _windowTitle = @" --- ";
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

    public ThumbnailDataContext(IntPtr thumbnailRelation, RECT rect, string windowTitle = null)
    {
        WindowTitle = string.IsNullOrEmpty(windowTitle) ? _windowTitle : windowTitle;
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
