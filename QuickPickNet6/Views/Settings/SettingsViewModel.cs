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

	private static SettingsViewModel _instance = new();

	private SettingsViewModel()
	{
		// prevent construction outside this class.
	}

	public static SettingsViewModel Instance
	{
		get { return _instance; }  // needs explicit get accesor for xaml binding.
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
