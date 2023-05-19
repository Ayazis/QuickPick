using System.Windows.Media;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using System;

namespace QuickPick.PinnedApps;

public class AppShortCut
{
    public AppShortCut()
    {
        ClickCommand = new RelayCommand(parameter => AppClicked(this));
    }
    public string Name { get; set; }
    public string TargetPath { get; set; }
    public ImageSource AppIcon { get; set; }
    public ICommand ClickCommand { get; set; }
    public string Arguments { get; set; }
    public bool HasWindowActiveOnCurrentDesktop { get; set; }

    public static void AppClicked(AppShortCut appInfo)
    {
        IntPtr windowHandle = WindowActivator.GetActiveWindow(appInfo.TargetPath);
        if(windowHandle != default)                    
            WindowActivator.ActivateWindow(windowHandle);   
        else
            Task.Run(() => { Process.Start(appInfo.TargetPath, appInfo.Arguments); });        
        ClickWindow.HideWindow();
    }    

    public AppShortCut FromRunningProcess(Process p)
    {
        return new AppShortCut()
        {
            
            

        };
    }
}
