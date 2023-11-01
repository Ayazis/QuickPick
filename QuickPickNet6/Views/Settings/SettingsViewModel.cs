using System.Reflection;
using System;

namespace QuickPick.UI.Views.Settings;
public enum AutoUpdateSetting
{
    Never,
    PreRelease,
    Master
}
public enum ActiveAppSetting
{
    IncludePinnedTaskBarApps,
    ActiveAppsOnly
}

public class SettingsViewModel : ObservableObject
{
    private AutoUpdateSetting _autoUpdateSetting = AutoUpdateSetting.Master;
    private ActiveAppSetting _activeAppSetting = ActiveAppSetting.IncludePinnedTaskBarApps;

    private string _version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
    private string _title => $"QuickPick {_version} - Settings";
    public string Title { get { return _title; } }

    public AutoUpdateSetting AutoUpdateSetting
    {
        get { return _autoUpdateSetting; }
        set
        {
            if (_autoUpdateSetting != value)
            {
                _autoUpdateSetting = value;
                OnPropertyChanged(nameof(AutoUpdateSetting));
            }
        }
    }

    public ActiveAppSetting ActiveAppSetting
    {
        get { return _activeAppSetting; }
        set
        {
            if (_activeAppSetting != value)
            {
                _activeAppSetting = value;
                OnPropertyChanged(nameof(ActiveAppSetting));
            }
        }
    }
}
