using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace QuickPick.PinnedApps;
[DebuggerDisplay("{Name} - {TargetPath}")]
/// <summary>
/// Wrapper for ShortCut to PinnedApps & open Windows, includes Icon.
/// </summary>
public partial class AppLink : ObservableObject
{
    public AppLink()
    {
        ClickCommand = new RelayCommand(parameter => ToggleWindowOrStartApplication(this));
    }
    public string Name { get; set; }
    public string TargetPath { get; set; }
    public string StartInDirectory { get; set; }
    public ImageSource AppIcon { get; set; }
    public ICommand ClickCommand { get; set; }
    public string Arguments { get; set; }
    [ObservableProperty]
    bool _hasWindowActiveOnCurrentDesktop;
  
    public List<IntPtr> WindowHandles { get; set; } = new();
    public string Info => $"{Name} - {TargetPath}";

    public void ToggleWindowOrStartApplication(AppLink appInfo)
    {
        if (WindowHandles.Count == 1)
            ActiveWindows.ToggleWindow(WindowHandles[0]);
        else
        {
            Task.Run(() =>
            {
                ProcessStartInfo info = new(appInfo.TargetPath, appInfo.Arguments);

                if (!string.IsNullOrWhiteSpace(appInfo.StartInDirectory))
                    info.WorkingDirectory = appInfo.StartInDirectory;

                Process.Start(info);
            });
            ClickWindow.Instance.HideWindow();
        }
    }

    internal void CloseThumbnail(object s, EventArgs e)
    {
        throw new NotImplementedException();
    }

    internal void RemoveThumbnail(IntPtr windowHandle)
    {
        WindowHandles.Remove(windowHandle);
        if(WindowHandles.Count == 0)
            HasWindowActiveOnCurrentDesktop = false;
    }
}
