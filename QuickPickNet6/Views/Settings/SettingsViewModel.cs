using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Reflection;

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

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private AutoUpdateSetting _autoUpdateSetting = AutoUpdateSetting.PreRelease;
    [ObservableProperty]
    private ActiveAppSetting _activeAppSetting = ActiveAppSetting.IncludePinnedTaskBarApps;

    private string _version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
    private string _title => $"QuickPick {_version} - Settings";
    public string Title { get { return _title; } }

    [ObservableProperty]
    private string _currentKeyCombo = "placeholder";

    public HashSet<System.Windows.Forms.Keys> NewKeyCombination = new();

    public void AddKeyToNewCombo(System.Windows.Forms.Keys key)
    {
        NewKeyCombination.Add(key);
        NewKeyCombo = string.Join(" + ", NewKeyCombination);
    }

    [ObservableProperty]
    private string _newKeyCombo = " - ";

    public void ApplySettings(QuickPick.Settings settings)
    {
        AutoUpdateSetting = settings.AutoUpdateSetting;
        ActiveAppSetting = settings.ActiveAppSetting;
        CurrentKeyCombo = string.Join(" + ", settings.KeyCombination);

    }

    internal void ClearNewKeyCombo()
    {
        NewKeyCombination.Clear();
        NewKeyCombo = string.Empty;
    }
}
