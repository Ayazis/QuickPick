using QuickPick.UI.Views.Settings;

namespace QuickPick
{
	public class Settings
	{
		static Settings()
		{
			_instance = new();
		}
		public static Settings Instance => _instance;
		private Settings() { }

		private static Settings _instance;
		public ActiveAppSetting ActiveAppSetting { get; set; }
		public AutoUpdateSetting AutoUpdateSetting { get; set; }
		public void ApplySettings(SettingsViewModel vm)
		{
			this.ActiveAppSetting = vm.ActiveAppSetting;
			this.AutoUpdateSetting = vm.AutoUpdateSetting;
		}

	}
}
