using System.Windows.Media;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using Newtonsoft.Json;
using QuickPick.UI.Views;

namespace QuickPick.PinnedApps;
[DebuggerDisplay("{Name} - {WindowHandle} - {TargetPath}")]
/// <summary>
/// Wrapper for ShortCut to PinnedApps & open Windows, includes Icon.
/// </summary>
public class TaskbarShortCut
{ 
    public TaskbarShortCut()
    {
        ClickCommand = new RelayCommand(parameter => AppClicked(this));
    }
    public string Name { get; set; }
    public string TargetPath { get; set; }
    public ImageSource AppIcon { get; set; }
    public ICommand ClickCommand { get; set; }
    public string Arguments { get; set; }
    public bool HasWindowActiveOnCurrentDesktop { get; set; }
    public IntPtr WindowHandle { get; set; }

    public string Info => $"{Name} - {WindowHandle} - {TargetPath}";

    public static void AppClicked(TaskbarShortCut appInfo)
    {
        IntPtr windowHandle = ActiveWindows.GetActiveWindowOnCurentDesktop(appInfo.TargetPath);
        if(windowHandle != default)                    
            ActiveWindows.ToggleWindow(windowHandle);   
        else
            Task.Run(() => { Process.Start(appInfo.TargetPath, appInfo.Arguments); });
        ClickWindow.HideWindow();
    }    


}
