using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThumbnailLogic;

namespace QuickPick.UI.Views.Thumbnail;


public partial class ThumbnailView : UserControl
{
    ThumbnailDataContext _context;
    public ThumbnailView()
    {
            
    }

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
    private void FadeInThisThumbnailView()
    {
        // Gradually increase the opacity over time to create a fade-in effect. // Same logic as in ThumbnailCreator
        for (int i = 0; i <= 255; i += 25)
        {
            this.Opacity = i / 255;
            // Sleep for a bit to control the speed of the fade-in. Adjust this value as needed.
            Thread.Sleep(20);
        }
        // Make sure the opacity ends at 1 for full visibiblity.
        this.Opacity = 1;
    }

    public void Hide()
    {
        this.Visibility = System.Windows.Visibility.Collapsed;
        ThumbnailCreator.DwmUnregisterThumbnail(_context.ThumbnailRelation);
    }
}
