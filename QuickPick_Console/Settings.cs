using QuickPick.UI.Views.Settings;

namespace QuickPick
{
	public class Settings
	{
		public static Settings Instance => new();
		private Settings() { }


		public ActiveAppSetting ActiveAppSetting { get; set; }
		public AutoUpdateSetting AutoUpdateSetting { get; set; }
		public void ApplySettings(SettingsViewModel vm)
		{
			this.ActiveAppSetting = vm.ActiveAppSetting;
			this.AutoUpdateSetting = vm.AutoUpdateSetting;
		}

	}
}
