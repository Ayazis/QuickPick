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

    private string _currentKeyCombo;
    public string CurrentKeyCombo
    {
        get { return _currentKeyCombo; }
        set
        {
            if (_currentKeyCombo != value)
            {
                _currentKeyCombo = value;
                OnPropertyChanged(nameof(CurrentKeyCombo));
            }
        }

    }



    public HashSet<System.Windows.Forms.Keys> NewKeyCombination = new();

    public void AddKeyToNewCombo(System.Windows.Forms.Keys key)
    {
        NewKeyCombination.Add(key);
        NewKeyCombo = string.Join(" + ", NewKeyCombination);
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
        NewKeyCombination.Clear();
        NewKeyCombo = string.Empty;
    }
}
