﻿using Ayazis.Utilities;
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
using System.Windows.Media;
using System.Windows.Media.Animation;
using ThumbnailLogic;

namespace QuickPick;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window
{
    public static ClickWindow Instance;
    private QuickPickMainWindowModel _qpm = new();
    private IntPtr _quickPickWindowHandle;
    public static ThumbnailTimer ThumbnailTimer;
    static DateTime _timeStampLastShown;
    private List<Popup> _currentPopups = new();

    public Storyboard HideAnimation { get; private set; }
    public Storyboard ShowAnimation { get; private set; }
    public ClickWindow()
    {
        ThumbnailTimer = new(HideThumbnails);
        InitializeComponent();
        this.PreviewMouseWheel += ClickWindow_PreviewMouseWheel;
        DataContext = _qpm;

        HideAnimation = TryFindResource("hideMe") as Storyboard;
        ShowAnimation = TryFindResource("showMe") as Storyboard;

        SetQuickPicksMainWindowHandle();
        UpdateLayout();
        Instance = this;
    }

    public void HandleFocusLost(object sender, EventArgs e)
    {
        HideWindow();
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
        bool includePinnedApps = SettingsManager.Instance.Settings.ActiveAppSetting == UI.Views.Settings.ActiveAppSetting.IncludePinnedTaskBarApps;
        List<AppLink> apps = AppLinkRetriever.GetPinnedAppsAndActiveWindows(includePinnedApps);

        foreach (var app in apps)
        {
            var handle = ActiveWindows.GetActiveWindowOnCurentDesktop(app.TargetPath);
            if (handle != default)
                app.HasWindowActiveOnCurrentDesktop = true;
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

    public void HideWindow()
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

        }
        catch (Exception ex)
        {
            Logs.Logger.Log(ex);
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
            Logs.Logger.Log(ex);
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
    private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        ThumbnailTimer.StopTimer();
        HideThumbnails();
        // todo: Move logic out of xaml.xs
        var button = (System.Windows.Controls.Button)sender;
        AppLink pinnedApp = button.DataContext as AppLink;

        var thumbnails = CreateThumbnails(pinnedApp, button);

        ShowThumbnails(thumbnails, button);
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
            var thumbnailProperties = new PreviewImageProperties(currentWindowHandle, windowTitle, dpiScaling);
            var thumbnailView = new ThumbnailView(thumbnailProperties);

            return thumbnailView;
        }
    }
    void ShowThumbnails(IEnumerable<ThumbnailView> thumbnails, Button button)
    {
        // Get DPI information
        PresentationSource source = PresentationSource.FromVisual(this);
        double dpiScaling = source.CompositionTarget.TransformToDevice.M11;

        // Get the center of the button relative to its container (the window)
        Point buttonCenter = button.TransformToAncestor(this)
                                    .Transform(new Point(button.ActualWidth / 2, button.ActualHeight / 2));



        // Get absolute coordinates.
        Point screenCoordinates = PointToScreen(buttonCenter);





        foreach (var thumbnailView in thumbnails)
        {
            Popup popup = new();
            _currentPopups.Add(popup);
            popup.Placement = PlacementMode.AbsolutePoint;


            popup.HorizontalOffset = screenCoordinates.X / dpiScaling;
            popup.VerticalOffset = screenCoordinates.Y / dpiScaling;

            popup.Child = thumbnailView;
            popup.IsOpen = true;

            IntPtr handle = ((HwndSource)PresentationSource.FromVisual(popup.Child)).Handle;           
            
            
            thumbnailView.FadeIn(handle);

        }
    }

    private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        ThumbnailTimer.StartTimer();
    }


    private void HideThumbnails()
    {
        foreach (var popup in _currentPopups)
        {
            popup.IsOpen = false;
            (popup.Child as ThumbnailView)?.Hide();
        }
        _currentPopups.Clear();
    }
}