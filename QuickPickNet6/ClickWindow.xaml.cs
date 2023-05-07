using System.Diagnostics;
using System;
using System.Windows;
using QuickPick.PinnedApps;
using System.Collections.ObjectModel;
using Ayazis.KeyHooks;
using System.Windows.Media.Animation;
using MouseAndKeyBoardHooks;
using Ayazis.Utilities;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace QuickPick;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window
{
    private QuickPickMainWindowModel _qpm = new QuickPickMainWindowModel();
    public Storyboard HideAnimation { get; private set; }
    public Storyboard ShowAnimation { get; private set; }
    public ClickWindow()
    {        
        InitializeComponent();
        var handle = GetMainWindowHandle();
        DataContext = _qpm;

        HideAnimation = TryFindResource("hideMe") as Storyboard;
        ShowAnimation = TryFindResource("showMe") as Storyboard;        
        
        HotKeys.KeyCombinationHit += HotKeys_KeyCombinationHit;
        HotKeys.LeftMouseClicked += HotKeys_LeftMouseClicked;
        ShowWindowInvisible();
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
    private void ShowWindowInvisible()
    {
        Opacity = 0;
        Show();
        Visibility = Visibility.Hidden;
        Opacity = 1;
    }

    private void HotKeys_KeyCombinationHit()
    {
        UpdatePinnedApps();
        ShowWindow();
    }

    private void UpdatePinnedApps()
    {
        var apps = TaskbarPinnedApps.GetPinnedTaskbarApps();
        _qpm.PinnedApps = new ObservableCollection<TaskBarApp>(apps);
        _qpm.NotifyPropertyChanged(nameof(_qpm.PinnedApps));
    }

    public void ShowWindow()
    {
        try
        {            
            //SetActiveWindow();

            this.Dispatcher.Invoke(() =>
            {
                //HideShortCuts();
                var mousePosition = MousePosition.GetCursorPosition();
                this.Left = mousePosition.X - (this.ActualWidth / 2);
                this.Top = mousePosition.Y - (this.ActualHeight / 2);                
                ShowAnimation.Begin(this);
            });

        }
        catch (Exception ex)
        {
            Logs.Logger.Log(ex);
        }
    }   

    private IntPtr GetMainWindowHandle()
    {
        // Getting the window handle only works when the app is shown in the taskbar & the mainwindow is shown.
        // The handle remains usable after setting this to false.

        ShowInTaskbar = true;
        Process currentProcess = Process.GetCurrentProcess();
        var quickPickMainWindowHandle = currentProcess.MainWindowHandle;
        ShowInTaskbar = false;
        Hide();
        return quickPickMainWindowHandle;
    }

    private void btnCenterClick(object sender, RoutedEventArgs e)
    {


    }

}
