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
using Utilities.Mouse_and_Keyboard;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using Utilities.Utilities.VirtualDesktop;
using Utilities.VirtualDesktop;
using System.Threading.Tasks;

namespace QuickPick;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window
{
    private static ClickWindow _instance;
    private QuickPickMainWindowModel _qpm = new QuickPickMainWindowModel();
    public Storyboard HideAnimation { get; private set; }
    public Storyboard ShowAnimation { get; private set; }
    public ClickWindow()
    {
        InitializeComponent();
        DataContext = _qpm;

        HideAnimation = TryFindResource("hideMe") as Storyboard;
        ShowAnimation = TryFindResource("showMe") as Storyboard;



        UpdateLayout();
        _instance = this;
    }

    private void HotKeys_LeftMouseClicked()
    {
        if (MouseIsOutsideWindow())
            HideAnimation.Begin(this);
    }
    public bool MouseIsOutsideWindow()
    {
        var mouse = MousePosition.GetCursorPosition();

        bool isOutside = (mouse.X < this.Left || mouse.X > this.Left + this.ActualWidth)
                        || (mouse.Y < this.Top || mouse.Y > this.Top + this.ActualHeight);

        return isOutside;
    }


    public void OnKeyCombinationHit()
    {
        ShowWindow();
    }


    public void UpdateTaskbarShortCuts()
    {
        List<TaskbarShortCut> apps = TaskbarApps.GetPinnedAppsAndActiveWindows();

        foreach (var app in apps)
        {
            var handle = ActiveWindows.GetActiveWindowOnCurentDesktop(app.TargetPath);
            if (handle != default)
                app.HasWindowActiveOnCurrentDesktop = true;
        }

        _qpm.PinnedApps = new ObservableCollection<TaskbarShortCut>(apps);
        _qpm.NotifyPropertyChanged(nameof(_qpm.PinnedApps));


        // Since we are not on the UI thread, use a dispatcher.
        this.Dispatcher.Invoke(() =>
        {
            UpdateLayoutOfApplinks();
        });
    }

    private void UpdateLayoutOfApplinks()
    {
        // Save original state
        var originalState = WindowState;
        var originalOpacity = Opacity;

        Opacity = 0.01;

        //  Visibility = Visibility.Hidden;        
        //Visibility = Visibility.Visible;
        //Opacity = 1;

        Applinks.InvalidateMeasure();
        Applinks.InvalidateVisual();
        Applinks.UpdateLayout();
        InvalidateMeasure();
        InvalidateVisual();
        UpdateLayout();
        //Hide();        
        //Show();

        // Restore original state
        Opacity = originalOpacity;    

        //Hide();
        //   Visibility = Visibility.Visible;
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
            this.Left = mousePosition.X - (this.ActualWidth / 2);
            this.Top = mousePosition.Y - (this.ActualHeight / 2);
            ShowAnimation.Begin(this);

        }
        catch (Exception ex)
        {
            Logs.Logger.Log(ex);
        }
    }
}
