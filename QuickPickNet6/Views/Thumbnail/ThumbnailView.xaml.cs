using QuickPick.PinnedApps;
using System;
using System.ComponentModel;
using System.Diagnostics;
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
    Color SemiGray = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#333333");
    Color almostBlack = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#202020");
    public PreviewImageProperties Properties;
    public readonly AppLink ParentApp;

    public IntPtr PreviewPointer { get; set; }
    public ThumbnailView()
    {

    }
    public ThumbnailView(PreviewImageProperties previewImageProperties, AppLink pinnedApp)
    {
        ParentApp = pinnedApp;
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
        // Set the fill color of the rectangle
        SolidColorBrush fillBrush = new SolidColorBrush(SemiGray);

        ThumbBackground.Background = fillBrush;
        ClickWindow.ThumbnailTimer.StopTimer();
        btnClose.Visibility= Visibility.Visible;
        tbWindowTitle.Margin = new Thickness(tbWindowTitle.Margin.Left, tbWindowTitle.Margin.Top, tbWindowTitle.Margin.Right + 10, tbWindowTitle.Margin.Bottom);
    }

    private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
    
        SolidColorBrush fillBrush = new SolidColorBrush(almostBlack);

        ThumbBackground.Background = fillBrush;
        ClickWindow.ThumbnailTimer.StartTimer();
        btnClose.Visibility = Visibility.Collapsed;
        tbWindowTitle.Margin = new Thickness(tbWindowTitle.Margin.Left, tbWindowTitle.Margin.Top, tbWindowTitle.Margin.Right - 10, tbWindowTitle.Margin.Bottom);
        // starttimer
    }

    private void UserControl_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        ActiveWindows.ToggleWindow(Properties.WindowHandle);
        // Toggle.
    }
    private void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        Debug.WriteLine("EVENT; Thumbnail close button clicked.");
        ClickWindow.Instance.SetCurrentTimeOnTimeStamp();
        ActiveWindows.CloseWindow(Properties.WindowHandle);
        this.Visibility = System.Windows.Visibility.Collapsed;
        CloseThumbnailEvent?.Invoke(this, new ThumbnailViewEventArgs(this));
    }


    // create new eventargs with thumbnailview as parameter
    public class ThumbnailViewEventArgs : EventArgs
    {
        public ThumbnailViewEventArgs(ThumbnailView thumbnailView)
        {
            ThumbnailView = thumbnailView;
        }

        public ThumbnailView ThumbnailView { get; set; }
    }


    // add ThumbnailView as parameter to eventhandler
    public event EventHandler<ThumbnailViewEventArgs> CloseThumbnailEvent;

}
