namespace QuickPick.UI.Views.Settings;

public class SettingsViewModel : ObservableObject
{
    public SettingsViewModel()
    {
			
    }
    private AutoUpdateSetting _autoUpdateSetting = AutoUpdateSetting.Master;
	private ActiveAppSetting _activeAppSetting = ActiveAppSetting.IncludePinnedTaskBarApps;

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
