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

    private static string _version => Assembly.GetEntryAssembly().GetName().Version.ToString();
    private static string _title => $"QuickPick {_version} - Settings";
    public static string Title { get { return _title; } }

    [ObservableProperty]
    private string _currentKeyCombo = "placeholder";
    
    [ObservableProperty]
    private string _newKeyCombo = " - ";
    
    public HashSet<System.Windows.Forms.Keys> NewKeyCombination = new();

    public void AddKeyToNewCombo(System.Windows.Forms.Keys key)
    {
        NewKeyCombination.Add(key);
        NewKeyCombo = string.Join(" + ", NewKeyCombination);
    }



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
