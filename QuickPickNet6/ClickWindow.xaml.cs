using System.Diagnostics;
using System;
using System.Windows;
using QuickPick.PinnedApps;
using System.Collections.ObjectModel;
using Ayazis.KeyHooks;

namespace QuickPick;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window
{
    private QuickPickMainWindowModel _qpm = new QuickPickMainWindowModel();

    public ClickWindow()
    {
        var apps = TaskbarPinnedApps.GetPinnedTaskbarApps();
        _qpm.PinnedApps = new ObservableCollection<PinnedAppInfo>(apps);

        InitializeComponent();
        var handle = GetMainWindowHandle();
        DataContext = _qpm;        
        ContentRendered += ClickWindow_SourceInitialized; ;

        HotKeys.KeyCombinationHit += HotKeys_KeyCombinationHit;    
    }

    private void HotKeys_KeyCombinationHit()
    {
        this.ShowDialog();
    }

    private void ClickWindow_SourceInitialized(object sender, EventArgs e)
    {
     //   Show();
    }

    private IntPtr GetMainWindowHandle()
    {
        // Getting the window handle only works when the app is shown in the taskbar & the mainwindow is shown.
        // The handle remains usable after setting this to false.
        this.Show();
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
