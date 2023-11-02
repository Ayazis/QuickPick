using Microsoft.VisualBasic;
using Newtonsoft.Json;
using QuickPick.UI.Views.Settings;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;

namespace QuickPick
{
	public class SettingsManager
	{
		const string SETTINGS_FILENAME = "QuickPickSettings.json";
		static SettingsManager _instance;
		public static SettingsManager Instance => _instance ??= new SettingsManager();
		static SettingsManager()
		{
			// prevent public constructin
		}

		public Settings Settings { get; private set; } = new();

		public void ApplySettings(SettingsViewModel vm)
		{
			this.Settings.ActiveAppSetting = vm.ActiveAppSetting;
			this.Settings.AutoUpdateSetting = vm.AutoUpdateSetting;

			try
			{
				string tempPath = Path.GetTempPath();
				string settingsPath = Path.Combine(tempPath, SETTINGS_FILENAME);

				var json = JsonConvert.SerializeObject(Settings, Formatting.Indented);

				// Use a FileStream to ensure proper handling of the file
				using (FileStream fileStream = new FileStream(settingsPath, FileMode.Create, FileAccess.Write, FileShare.None))
				using (StreamWriter streamWriter = new StreamWriter(fileStream))
				{
					streamWriter.Write(json);
				}

				Trace.WriteLine("Settings saved successfully.");
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"An error occurred while saving the settings: {ex.Message}");
			}
		}

		public void LoadSettings()
		{
			try
			{
				string tempPath = Path.GetTempPath();
				string settingsPath = Path.Combine(tempPath, SETTINGS_FILENAME);

				if (File.Exists(settingsPath))
				{
					using (FileStream fileStream = new FileStream(settingsPath, FileMode.Open, FileAccess.Read, FileShare.Read))
					using (StreamReader streamReader = new StreamReader(fileStream))
					{
						var json = streamReader.ReadToEnd();
						Settings = JsonConvert.DeserializeObject<Settings>(json);
					}

					Trace.WriteLine("Settings loaded successfully.");
				}
				else
				{
					// Handle the case where the settings file doesn't exist yet
					Trace.WriteLine("Settings file does not exist.");
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"An error occurred while loading the settings: {ex.Message}");
			}
		}

	}
	public class Settings
	{
		public ActiveAppSetting ActiveAppSetting { get; set; } = ActiveAppSetting.IncludePinnedTaskBarApps;
		public AutoUpdateSetting AutoUpdateSetting { get; set; } = AutoUpdateSetting.PreRelease;
	}
}
