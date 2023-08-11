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
        this.Visibility = System.Windows.Visibility.Hidden;
    }

    public async Task FadeIn()
    {
        this.Dispatcher.Invoke(() =>
        {
            this.Opacity = 0;
            this.Visibility = System.Windows.Visibility.Visible;

        });
        
        Task.Run(() => { FadeInThisThumbnailViewAsync(); });
        Task.Run(() => { ThumbnailCreator.CreateAndFadeInThumbnail(_context.ThumbnailRelation, _context.Rect); });


        return;

        Task taskCreateAndFadeInThumbnail = new Task(() => { });
        Task taskFadeInThisView = new Task(() => { });
        taskFadeInThisView.Start();
        taskCreateAndFadeInThumbnail.Start();
        await Task.WhenAll(taskCreateAndFadeInThumbnail, taskFadeInThisView);
    }
    private async Task FadeInThisThumbnailViewAsync()
    {
       
        // use dispatchers as short as possible to prevent lag.

        // Gradually increase the opacity over time to create a fade-in effect. // Same logic as in ThumbnailCreator
        //for (double i = 0; i <= 255; i += 25)
        //{
        //    double newOpacityValue = i / 255;
        //    double localOpacity = newOpacityValue;
        //    this.Dispatcher.Invoke(() =>
        //    {
        //        this.Opacity = localOpacity;
        //    });
        //    // Sleep for a bit to control the speed of the fade-in. Adjust this value as needed.
        //    Thread.Sleep(20);
        //}
        this.Dispatcher.Invoke(() =>
        {
            // Make sure the opacity ends at 1 for full visibiblity.
            this.Opacity = 1;
        });


    }

    public void Hide()
    {
        this.Visibility = System.Windows.Visibility.Collapsed;
        ThumbnailCreator.DwmUnregisterThumbnail(_context.ThumbnailRelation);
    }
}
