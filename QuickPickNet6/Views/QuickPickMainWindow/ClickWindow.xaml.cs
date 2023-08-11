using System;
using System.Collections.Generic;
using System.Windows;
using QuickPick.PinnedApps;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Ayazis.KeyHooks;
using System.Windows.Media.Animation;
using MouseAndKeyBoardHooks;
using Ayazis.Utilities;
using ThumbnailLogic;
using System.Linq;
using System.Windows.Input;
using Microsoft.VisualBasic;

namespace QuickPick;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window
{
    private static ClickWindow _instance;
    private QuickPickMainWindowModel _qpm = new QuickPickMainWindowModel();
    private IntPtr _quickPickWindowHandle;
    private List<IntPtr> _currentThumbnails = new List<IntPtr>();

    public Storyboard HideAnimation { get; private set; }
    public Storyboard ShowAnimation { get; private set; }
    public ClickWindow()
    {
        InitializeComponent();
        this.PreviewMouseWheel += ClickWindow_PreviewMouseWheel; ;
        DataContext = _qpm;

        HideAnimation = TryFindResource("hideMe") as Storyboard;
        ShowAnimation = TryFindResource("showMe") as Storyboard;


        SetQuickPicksMainWindowHandle();
        UpdateLayout();
        _instance = this;
    }

    private void ClickWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (MouseIsOutsideWindow())
            HideAnimation.Begin(this);
    }

    private void ClickWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var collection = _qpm.PinnedApps;
        if (e.Delta > 0)
        {       // Scrolled up
            if (collection.Count > 1)
            {
                collection.Move(0, collection.Count - 1);
                //AppLink firstItem = collection[0];
                //collection.RemoveAt(0);
                //collection.Add(firstItem);
            }
        }
        else
        {
            // Scrolled down
            // Scrolled down
            if (collection.Count > 1)
            {
                collection.Move(collection.Count - 1, 0);
            }
        }
    }

    public bool MouseIsOutsideWindow()
    {
        var mouse = MousePosition.GetCursorPosition();

        bool isOutside = mouse.X < Left || mouse.X > Left + ActualWidth
                        || mouse.Y < Top || mouse.Y > Top + ActualHeight;

        return isOutside;
    }

    public void UpdateTaskbarShortCuts()
    {
        List<AppLink> apps = AppLinkRetriever.GetPinnedAppsAndActiveWindows();

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


        _qpm.PinnedApps = new ObservableCollection<AppLink>(apps);
        _qpm.NotifyPropertyChanged(nameof(_qpm.PinnedApps));
    }

    public static void HideWindow()
    {
        try
        {
            _instance.HideAnimation.Begin(_instance);

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
    private void Button_MouseEnter(object sender, MouseEventArgs e)
    {
        // todo: Move logic out of xaml.xs
        var button = (System.Windows.Controls.Button)sender;
        AppLink pinnedApp = button.DataContext as AppLink;   

        // Get DPI information
        PresentationSource source = PresentationSource.FromVisual(this);
        var m = source.CompositionTarget.TransformToDevice;
        double dpiScaling = m.M11;
        

        // Get the center of the button relative to its container (the window)
        var buttonCenter = button.TransformToAncestor(this)
                                .Transform(new Point(button.ActualWidth / 2, button.ActualHeight / 2));

   

        CreateThumbnails(pinnedApp, dpiScaling, buttonCenter);
    }

    private void CreateThumbnails(AppLink pinnedApp, double dpiScaling, Point buttonCenter)
    {
        // Get the center of the window
        double windowCenterX = this.ActualWidth / 2;
        double windowCenterY = this.ActualHeight / 2;
        // Calculate the button's position relative to the window's center
        double relativeX = buttonCenter.X - windowCenterX;
        double relativeY = buttonCenter.Y - windowCenterY;

        // Scale the relative position by a factor to adjust the thumbnail's position       
        double offsetX = relativeX < 0 ? relativeX * 1.2 - 20 : relativeX + 20 * 1.2;
        double offsetY = relativeY < 0 ? relativeY - 20 : relativeY + 20;

        for (int i = 0; i < pinnedApp.WindowHandles.Count; i++)
        {
            IntPtr currentWindowHandle = pinnedApp.WindowHandles[i];
            double aspectRatio = ThumbnailCreator.GetWindowAspectRatio(currentWindowHandle);


            double height, width;
            if (aspectRatio > 1)
            {
                width = 200;
                height = width / aspectRatio;
            }
            else
            {
                height = 200;
                width = height * aspectRatio;

            }

            // Calculate the thumbnail's position, ensuring it is centered around the button's position
            double thumbnailX = buttonCenter.X + offsetX - width / 2;
            double thumbnailY = buttonCenter.Y + offsetY - height / 2;

            var newThumbnail = ThumbnailCreator.GetThumbnailRelations(currentWindowHandle, _quickPickWindowHandle);
            if (newThumbnail == default)
                continue;
            _currentThumbnails.Add(newThumbnail);


            int left = (int)(thumbnailX * dpiScaling + (i * width));
            int top = (int)(thumbnailY * dpiScaling);
            int right = (int)((thumbnailX + width) * dpiScaling + (i * width));
            int bottom = (int)((thumbnailY + height) * dpiScaling);

            RECT rect = new RECT(left, top, right, bottom);
            ThumbnailCreator.CreateAndFadeInThumbnail(newThumbnail, rect);
        }
    }

    private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        foreach (var item in _currentThumbnails)
        {
            if (item != default)
                ThumbnailCreator.DwmUnregisterThumbnail(item);
        }
    }

    private double CalculateAngle(Point center, Point position)
    {
        double dx = position.X - center.X;
        double dy = position.Y - center.Y;

        double radian = Math.Atan2(dy, dx);
        double angle = radian * (180 / Math.PI);

        return angle;
    }

}
