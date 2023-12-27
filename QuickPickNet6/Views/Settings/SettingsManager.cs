using Newtonsoft.Json;
using QuickPick.UI.Views.Settings;
using System;
using System.Diagnostics;
using System.IO;
using Utilities.Mouse_and_Keyboard;

namespace QuickPick
{
    public class SettingsManager
    {
        static SettingsManager _instance;
        public static SettingsManager Instance => _instance ??= new SettingsManager();
        public string SettingsPath { get; private set; }

        private SettingsManager()
        {
            string saveDirectory = Path.Combine(Path.GetTempPath(), "QuickPick");
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }
            SettingsPath = Path.Combine(saveDirectory, "QpSettings.Json");
        }


        public Settings Settings { get; private set; } = new();

        public void ApplySettings(SettingsViewModel vm)
        {
            Settings.ActiveAppSetting = vm.ActiveAppSetting;
            Settings.AutoUpdateSetting = vm.AutoUpdateSetting;
            Settings.KeyCombination = vm.NewKeyCombination;
            KeyInputHandler.Instance.SetKeycombination(Settings.KeyCombination);
            WriteSettingsToDisk();
        }

        private void WriteSettingsToDisk()
        {
            try
            {
                var json = JsonConvert.SerializeObject(Settings, Formatting.Indented);

                // Use a FileStream to ensure proper handling of the file
                using (FileStream fileStream = new FileStream(SettingsPath, FileMode.Create, FileAccess.Write, FileShare.None))
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
            if (!File.Exists(SettingsPath))
            {
                Trace.WriteLine("Settings file does not exist.");
                return;
            }
            try
            {
                using (FileStream fileStream = new FileStream(SettingsPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    var json = streamReader.ReadToEnd();
                    Settings = JsonConvert.DeserializeObject<Settings>(json);
                    throw new Exception("Settings has 1 extra button?!");
                }

                Trace.WriteLine("Settings loaded successfully.");

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"An error occurred while loading the settings: {ex.Message}");
            }
        }

    }
}
