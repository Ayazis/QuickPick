using System.Diagnostics;
using System;
using System.Windows;
using QuickPick.PinnedApps;
using System.Collections.ObjectModel;
using Ayazis.KeyHooks;
using System.Windows.Media.Animation;
using MouseAndKeyBoardHooks;
using Ayazis.Utilities;

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
        ShowWindowInvisible();
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
        ShowAnimation.Begin(this);
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

                this.WindowStyle = WindowStyle.None;
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
