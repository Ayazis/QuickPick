using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using ThumbnailLogic;

namespace QuickPick.UI.Views.Thumbnail;


public partial class ThumbnailView : UserControl
{
    public ThumbnailProperties Properties;
    public ThumbnailView()
    {

    }
    public ThumbnailView(ThumbnailProperties context, double dpiScaling)
    {
        var size = CalculateRenderSize(context.Rect);
        context.Width = size.Width * 1.25 / dpiScaling;
        context.Height = size.Height * 1.25 / dpiScaling;

        InitializeComponent();
        this.DataContext = context;
        Properties = context;
    }

    public void FadeIn()
    {
        this.Dispatcher.Invoke(() =>
        {
            this.Opacity = 0;
            this.Visibility = System.Windows.Visibility.Visible;

        });

        Task.Run(() => { ShowThumbnailView(); });
        Task.Run(() => { WindowPreviewCreator.CreateAndFadeInThumbnail(Properties.ThumbnailRelation, Properties.Rect); });

    }
    private void ShowThumbnailView(bool fadeIn = false)
    {
        if (!fadeIn)
        {
            this.Dispatcher.Invoke(() =>
            {
                // Make sure the opacity ends at 1 for full visibiblity.
                this.Opacity = 1;
            });
            return;
        }

        // use dispatchers as short as possible to prevent lag.
        // Gradually increase the opacity over time to create a fade-in effect. // Same logic as in WindowPreviewCreator
        for (double i = 0; i <= 255; i += 25)
        {
            double newOpacityValue = i / 255;
            double localOpacity = newOpacityValue;
            this.Dispatcher.Invoke(() =>
            {
                this.Opacity = localOpacity;
            });
            // Sleep for a bit to control the speed of the fade-in. Adjust this value as needed.
            Thread.Sleep(20);
        }
    }

    public void Hide()
    {
        this.Visibility = System.Windows.Visibility.Collapsed;
        WindowPreviewCreator.DwmUnregisterThumbnail(Properties.ThumbnailRelation);
    }

    private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        var color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#101010");

        // Set the fill color of the rectangle
        SolidColorBrush fillBrush = new SolidColorBrush(color);

        ThumbBackground.Background = fillBrush;
        ClickWindow.ThumbnailTimer.StopTimer();
    }

    private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        var color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#202020");
        SolidColorBrush fillBrush = new SolidColorBrush(color);

        ThumbBackground.Background = fillBrush;
        ClickWindow.ThumbnailTimer.StartTimer();
        // starttimer
    }

    private void UserControl_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        ActiveWindows.ToggleWindow(Properties.WindowHandle);
        // Toggle.
    }
    public static System.Windows.Size CalculateRenderSize(RECT rect)
    {
        int width = rect.Right - rect.Left;
        int height = rect.Bottom - rect.Top;

        // Ensure width and height are positive
        if (width < 0)
            width = -width;

        if (height < 0)
            height = -height;

        return new System.Windows.Size(width, height);
    }
}
