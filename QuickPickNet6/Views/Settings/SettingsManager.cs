using QuickPick.UI.Views.Settings;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities.Mouse_and_Keyboard;

namespace QuickPick
{
    public interface ISettingsManager
    {
        Settings Settings { get; }
        string SettingsPath { get; }

        void ApplySettings(SettingsViewModel vm);
        void LoadSettings();
    }

    public class SettingsManager : ISettingsManager
    {             
        public string SettingsPath { get; private set; }
        public IKeyInputHandler _keyInputHandler;

        public SettingsManager(IKeyInputHandler keyInputHandler)
        {
            string saveDirectory = Path.Combine(Path.GetTempPath(), "QuickPick");
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }
            SettingsPath = Path.Combine(saveDirectory, "QpSettings.Json");            
            _keyInputHandler = keyInputHandler;
        }
        public Settings Settings { get; private set; } = new();

        public void ApplySettings(SettingsViewModel vm)
        {
            Settings.ActiveAppSetting = vm.ActiveAppSetting;
            Settings.AutoUpdateSetting = vm.AutoUpdateSetting;
            if (vm.NewKeyCombination?.Any() == true)
            {
                Settings.KeyCombination = vm.NewKeyCombination;
                _keyInputHandler.SetKeyCombination(Settings.KeyCombination);
            }
            WriteSettingsToDisk();
        }

        private void WriteSettingsToDisk()
        {
            try
            {
                var json = Settings.Serialize();

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
                    Settings = Settings.DeSerialize(json);
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