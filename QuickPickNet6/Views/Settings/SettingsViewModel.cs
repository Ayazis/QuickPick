using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

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
    private AutoUpdateSetting _autoUpdateSetting = AutoUpdateSetting.PreRelease;
    private ActiveAppSetting _activeAppSetting = ActiveAppSetting.IncludePinnedTaskBarApps;

    private string _version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
    private string _title => $"QuickPick {_version} - Settings";
    public string Title { get { return _title; } }

    public string CurrentKeyCombo { get; set; } = "ctrl + rMouse";

    private HashSet<System.Windows.Forms.Keys> _newKeyComboKeys = new();

    public void AddKeyToNewCombo(System.Windows.Forms.Keys key)
    {
        _newKeyComboKeys.Add(key);
        NewKeyCombo = string.Join(" + ", _newKeyComboKeys);
    }

    private string _newKeyCombo = " - ";
    public string NewKeyCombo
    {
        get { return _newKeyCombo; }
        set
        {
            _newKeyCombo = value;
            OnPropertyChanged(nameof(NewKeyCombo));
        }
    }

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
    public void ApplySettings(QuickPick.Settings settings)
    {
        AutoUpdateSetting = settings.AutoUpdateSetting;
        ActiveAppSetting = settings.ActiveAppSetting;
    }

    internal void ClearNewKeyCombo()
    {
        _newKeyComboKeys.Clear();
        NewKeyCombo = string.Empty;
    }
}
