using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThumbnailLogic;

namespace QuickPick.UI.Views.Thumbnail;


public partial class ThumbnailView : UserControl
{
    ThumbnailDataContext _context;

    public ThumbnailView(ThumbnailDataContext context)
    {
        InitializeComponent();
        this.DataContext = context;
        _context = context;
    }

    public async void FadeIn()
    {
        Task taskCreateAndFadeInThumbnail = new Task(() => { ThumbnailCreator.CreateAndFadeInThumbnail(_context.ThumbnailRelation, _context.Rect); });

        Task taskFadeInThisView = new Task(() => { this.FadeInThisThumbnailView(); });

        await Task.WhenAll(taskCreateAndFadeInThumbnail, taskFadeInThisView);
    }
    void FadeInThisThumbnailView()
    {
        
    }

    public void Hide()
    {

    }
}
