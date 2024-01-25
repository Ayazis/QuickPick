using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using ThumbnailLogic;

namespace QuickPick.UI.Views.Thumbnail;


public partial class ThumbnailView : UserControl
{
    public PreviewImageProperties Properties;
    public IntPtr PreviewPointer { get; set; }
    public ThumbnailView()
    {

    }
    public ThumbnailView(PreviewImageProperties previewImageProperties)
    {
        InitializeComponent();
        this.DataContext = previewImageProperties;
        Properties = previewImageProperties;
    }

    public void FadeIn(IntPtr parentHandle)
    {
        this.Dispatcher.Invoke(() =>
        {
            this.Opacity = 0;
            this.Visibility = System.Windows.Visibility.Visible;

        });

        Task.Run(() => { ShowThumbnailView(); });

        PreviewPointer = WindowPreviewCreator.GetPreviewImagePointer(Properties.WindowHandle, parentHandle);

        RECT rect = GetRectForPreviewImage();

        Task.Run(() => { WindowPreviewCreator.CreateAndFadeInPreviewImage(PreviewPointer, rect); });

    }

    private RECT GetRectForPreviewImage()
    {
        const int MARGIN = 15;
        int dpiAdjustedMargin = (int)(MARGIN * Properties.DpiScaling);
        int dpiAdjustedWidth = (int)(Properties.Width * Properties.DpiScaling);
        int dpiAdjustedHeight = (int)(Properties.Height * Properties.DpiScaling);

        var rect = new RECT()
        {
            Left = dpiAdjustedMargin,
            Top = 2 * dpiAdjustedMargin,
            Bottom = dpiAdjustedHeight - dpiAdjustedMargin,
            Right = dpiAdjustedWidth - dpiAdjustedMargin,
        };
        return rect;
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
        WindowPreviewCreator.DwmUnregisterThumbnail(PreviewPointer);
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

}
