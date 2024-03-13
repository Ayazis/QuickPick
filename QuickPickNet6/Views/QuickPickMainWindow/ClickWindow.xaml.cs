using Ayazis.Utilities;
using MouseAndKeyBoardHooks;
using QuickPick.PinnedApps;
using QuickPick.UI;
using QuickPick.UI.Views.Thumbnail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Xps.Serialization;
using static QuickPick.UI.Views.Thumbnail.ThumbnailView;

namespace QuickPick;

public interface IClickWindow
{
    Storyboard HideAnimation { get; }
    Storyboard ShowAnimation { get; }

    void DisableMouseScroll();
    void EnableMouseScroll();
    void HandleFocusLost(object sender, EventArgs e);
    void HideUI();
    void InitializeComponent();
    void SetCurrentTimeOnTimeStamp();
    void ShowWindow();
    void UpdateTaskbarShortCuts();
    void HandleMouseButtonClicked(object sender, EventArgs e);

}

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window, IClickWindow
{
    public static ClickWindow Instance;
    private QuickPickMainWindowModel _qpm = new();
    private IntPtr _quickPickWindowHandle;
    public static ThumbnailTimer MouseLeftTimer;
    static DateTime _timeStampLastShown;
    private Dictionary<IntPtr, Popup> _currentPopups = new();
    readonly ILogger _logger;
    readonly ISettingsManager _settingsManager;

    public Storyboard HideAnimation { get; private set; }
    public Storyboard ShowAnimation { get; private set; }
    public ClickWindow(ILogger logger, ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        _logger = logger;
        MouseLeftTimer = new(HideThumbnails);
        InitializeComponent();
        EnableMouseScroll();
        DataContext = _qpm;

        HideAnimation = TryFindResource("hideMe") as Storyboard;
        ShowAnimation = TryFindResource("showMe") as Storyboard;

        SetQuickPicksMainWindowHandle();
        UpdateLayout();
        Instance = this;
    }

    public void EnableMouseScroll()
    {
#if DEBUG
        this.PreviewMouseWheel += ClickWindow_PreviewMouseWheel;
#endif
    }
    public void DisableMouseScroll()
    {
        this.PreviewMouseWheel -= ClickWindow_PreviewMouseWheel;
    }


    public void HandleFocusLost(object sender, EventArgs e)
    {
        HideUI();
    }

    private void ClickWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var collection = _qpm.PinnedApps;
        if (e.Delta > 0)
        {       // Scrolled up
            if (collection.Count > 1)
            {
                collection.Move(0, collection.Count - 1);
            }
        }
        else
        {
            // Scrolled down
            if (collection.Count > 1)
            {
                collection.Move(collection.Count - 1, 0);
            }
        }
    }

    public void UpdateTaskbarShortCuts()
    {
        bool includePinnedApps = _settingsManager.Settings.ActiveAppSetting == UI.Views.Settings.ActiveAppSetting.IncludePinnedTaskBarApps;
        List<AppLink> apps;
        try
        {
            apps = AppLinkRetriever.GetPinnedAppsAndActiveWindows(includePinnedApps);

            foreach (var app in apps)
            {
                var handle = ActiveWindows.GetActiveWindowOnCurentDesktop(app.TargetPath);
                if (handle != default)
                    app.HasWindowActiveOnCurrentDesktop = true;
            }
        }
        catch (InvalidOperationException)
        {
            // can happen when a user clicks too fast on the hotkey combination when starting the application.
            return;
        }


        // Group the apps by filePath
        var grouped = apps.GroupBy(g => g.TargetPath);
        foreach (var group in grouped)
        {
            // Foreach 
            var openWindows = group.ToList();
            var main = openWindows.First();
            main.WindowHandles = openWindows.SelectMany(s => s.WindowHandles).Distinct().ToList();
        }


        Dispatcher.Invoke(() =>
        {
            _qpm.PinnedApps.Clear();
            foreach (var item in apps)
            {
                _qpm.PinnedApps.Add(item);
            }
        });
    }
    public void SetCurrentTimeOnTimeStamp()
    {
        _timeStampLastShown = DateTime.Now;
    }

    public void HideUI()
    {
        try
        {
            // If the Ui has been shown in the last couple of millieseconds, we should not hide the window.
            // This often happens when a user clicks the hotkey combination whilst the ui is showing.
            TimeSpan timeSinceUIShown = DateTime.Now - _timeStampLastShown;
            if (timeSinceUIShown < TimeSpan.FromMilliseconds(200))
                return;


            Deactivated -= HandleFocusLost;
            LostFocus -= HandleFocusLost;
            HideAnimation.Begin(this);
            HideThumbnails();

        }
        catch (Exception ex)
        {
            _logger.Log(ex);
        }
    }

    public void ShowWindow()
    {
        try
        {
            _timeStampLastShown = DateTime.Now;
            Visibility = Visibility.Visible;
            var mousePosition = MousePosition.GetCursorPosition();
            Left = mousePosition.X - ActualWidth / 2;
            Top = mousePosition.Y - ActualHeight / 2;
            ShowAnimation.Begin(this);
        }
        catch (Exception ex)
        {
            _logger.Log(ex);
        }
    }

    private void SetQuickPicksMainWindowHandle()
    {
        // Getting the window handle only works when the app is shown in the taskbar.
        // hHe handle remains usable after setting this to false.
        ShowInTaskbar = true;
        Show();
        Process currentProcess = Process.GetCurrentProcess();
        _quickPickWindowHandle = currentProcess.MainWindowHandle;
        ShowInTaskbar = false;
        Hide();

    }


    private IEnumerable<ThumbnailView> CreateThumbnails(AppLink pinnedApp, System.Windows.Controls.Button button)
    {
        // Get the center of the button relative to its container (the window)
        var buttonCenter = button.TransformToAncestor(this)
                                    .Transform(new Point(button.ActualWidth / 2, button.ActualHeight / 2));

        // Get the center of the window
        double windowCenterX = this.ActualWidth / 2;
        double windowCenterY = this.ActualHeight / 2;

        // Calculate the button's position relative to the window's center
        double xToWindowCenter = buttonCenter.X - windowCenterX;
        double ytoWindowCenter = buttonCenter.Y - windowCenterY;

        // Get DPI information
        PresentationSource source = PresentationSource.FromVisual(this);
        double dpiScaling = source.CompositionTarget.TransformToDevice.M11;

        for (int i = 0; i < pinnedApp.WindowHandles.Count; i++)
        {
            var thumbnail = ProcessThumbnail(i);
            if (thumbnail != null)
                yield return thumbnail;
        };

        ThumbnailView ProcessThumbnail(int i)
        {
            IntPtr currentWindowHandle = pinnedApp.WindowHandles[i];
            string windowTitle = ActiveWindows.GetWindowTitle(currentWindowHandle);
            var thumbnailProperties = new PreviewImageProperties(currentWindowHandle, windowTitle, dpiScaling, pinnedApp.AppIcon);
            var thumbnailView = new ThumbnailView(thumbnailProperties, pinnedApp);

            // subscribe to close event and handle it in pinnedApp
            thumbnailView.CloseThumbnailEvent += ThumbnailView_CloseThumbnailEvent;
            return thumbnailView;
        }
    }



    void ShowThumbnails(List<ThumbnailView> thumbnails, Button button)
    {
        // Get the center of the button relative to its container (the window)
        Point buttonCenter = button.TransformToAncestor(this)
                                    .Transform(new Point(button.ActualWidth / 2, button.ActualHeight / 2));

        // Get absolute screen coordinates for the button.
        Point buttonLocation = PointToScreen(buttonCenter);

        for (int i = 0; i < thumbnails.Count; i++)
        {
            var thumbnailView = thumbnails[i];
            Popup popup = new();
            // store the windowhandle as key in the dictionary for this popup. This way we can find the popup later.    
            _currentPopups[thumbnailView.Properties.WindowHandle] = popup;
            popup.Placement = PlacementMode.AbsolutePoint;

            // get x and y offset to the center            
            double xToWindowCenter = buttonCenter.X - ActualWidth / 2;
            double yToWindowCenter = buttonCenter.Y - ActualHeight / 2;

            // Get DPI information
            PresentationSource source = PresentationSource.FromVisual(this);
            double dpiScaling = source.CompositionTarget.TransformToDevice.M11;
            double dpiAdjustedWidth = thumbnailView.Properties.Width * dpiScaling;
            double dpiAdjustedHeight = thumbnailView.Properties.Height * dpiScaling;

            // Adjust the thumbnail's position according to it's relative position to the center of the window.
            var thumbnailPositionCalculator = new DpiSafeThumbnailPositioner();
            Point adjustedPosition = thumbnailPositionCalculator.CalculatePositionForThumbnailView(buttonLocation, xToWindowCenter, yToWindowCenter, i, dpiAdjustedWidth, dpiAdjustedHeight);

            // set the popups offset, taking the DPI into account.   
            popup.HorizontalOffset = adjustedPosition.X / thumbnailView.Properties.DpiScaling;
            popup.VerticalOffset = adjustedPosition.Y / thumbnailView.Properties.DpiScaling;

            popup.Child = thumbnailView;
            ShowThumbnail(thumbnailView, popup);
        }
    }

    private static void ShowThumbnail(ThumbnailView thumbnailView, Popup popup)
    {
        popup.IsOpen = true;

        IntPtr handle = ((HwndSource)PresentationSource.FromVisual(popup.Child)).Handle;

        thumbnailView.FadeIn(handle);
    }
    private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        MouseLeftTimer.StopTimer();
        HideThumbnails();
        // todo: Move logic out of xaml.xs
        Button button = (System.Windows.Controls.Button)sender;

        // slightly enlarge the buttons
        var parent = button.Parent as Grid;
        var children = parent.Children;
        foreach (var child in children)
        {
            if (child is not Button btn)
                continue;

            btn.Width += 5;
            btn.Height += 5;
        }


        if (button != null)
        {
            var container = ItemsControl.ContainerFromElement(Applinks, button) as UIElement;
            if (container != null)
            {
                Panel.SetZIndex(container, 1);
            }
        }

        AppLink pinnedApp = button.DataContext as AppLink;

        var thumbnails = CreateThumbnails(pinnedApp, button).ToList();

        ShowThumbnails(thumbnails, button);
    }
    private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        Button button = (System.Windows.Controls.Button)sender;

        // reset size of the buttons
        var parent = button.Parent as Grid;
        var children = parent.Children;
        foreach (var child in children)
        {
            if (child is not Button)
                continue;

            var userControl = child as FrameworkElement;
            userControl.Width -= 5;
            userControl.Height -= 5;
        }

        var container = ItemsControl.ContainerFromElement(Applinks, button) as UIElement;
        if (container != null)
        {
            Panel.SetZIndex(container, 0);
        }
        MouseLeftTimer.StartTimer();
    }


    private void HideThumbnails()
    {
        foreach (var item in _currentPopups)
        {
            var popup = item.Value;
            popup.IsOpen = false;
            (popup.Child as ThumbnailView)?.Hide();
        }
        _currentPopups.Clear();
    }

    private void ThumbnailView_CloseThumbnailEvent(object sender, ThumbnailViewEventArgs e)
    {
        var closedWindowHandle = e.ThumbnailView.Properties.WindowHandle;
        bool popupFound = _currentPopups.TryGetValue(closedWindowHandle, out Popup popup);
        if (!popupFound)
            return;

        popup.IsOpen = false;
        popup = null; // set to null to allow garbage collection
        _currentPopups.Remove(closedWindowHandle);

        e.ThumbnailView.ParentApp.RemoveThumbnail(e.ThumbnailView.Properties.WindowHandle);
    }
    public void HandleMouseButtonClicked(object sender, EventArgs e)
    {
        bool mouseOverMainWindow = IsMouseOver;
        bool mouseOverPopups = _currentPopups.Any(p => p.Value.IsMouseOver);

        if (!mouseOverMainWindow && !mouseOverPopups)
            HideUI();        
    }
}