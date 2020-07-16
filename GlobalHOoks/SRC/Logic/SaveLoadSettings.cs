using QuickPick.Classes;
using QuickPick.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace QuickPick.Logic
{
    public class SaveLoadSettings
    {
        public string SettingsPath { get; set; } = @"C:\Temp\QuickPickSettings.json";

        public Models.QuickPick QP { get; set; }
        public SaveLoadSettings(Models.QuickPick qp)
        {
            QP = qp;
        }

        public void SaveSettingsToDisk()
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(SettingsPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));

                var settings = new QuickPickSettings(QP.QuickPickModel);
                string settingsAsJson = JsonConvert.SerializeObject(settings, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });




                File.WriteAllText(SettingsPath, settingsAsJson);
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        public void LoadSettingsFile(string filePath)
        {
            var settings = DeserialiseSettingsFile(filePath);

            if (settings != null)
                ApplySettings(settings);

        }
        public void LoadSettingsFile()
        {
            var settings = DeserialiseSettingsFile(SettingsPath);
            ApplySettings(settings);

        }
        private void ApplySettings(QuickPickSettings settings)
        {
            if (settings == null)
            {
                for (int i = 0; i < QP.QuickPickModel.NrOfButtons; i++)
                {
                    var button = new QpButton();
                    button.Id = i + 1;
                    QP.ButtonManager.ConfigureButton(button);
                    QP.QuickPickModel.MainButtons.Add(button);
                }
            }
            else
            {
                // Clear the canvas
                QP.QuickPickModel.MainButtons.Clear();
                QP.ButtonManager.ClearCanvas();
                QP.ButtonManager.AddCentralButton();

                QP.QuickPickModel.NrOfButtons = settings.NrOfMainButtons;
                QP.QuickPickModel.ShortCutsFolder = settings.ShortCutsFolder;
                QP.QuickPickModel.InstantShortCuts = settings.InstantShortcuts;

                // Create mainButtons.                
                foreach (var button in settings.MainButtons)
                {
                    QP.ButtonManager.ConfigureButton(button);
                    QP.QuickPickModel.MainButtons.Add(button);
                }

            }
            QP.ButtonManager.PlaceButtonsOnCanvas();

            // Get Shortcuts from saved folderLocation.
            QP.QuickPickModel.ShortCuts.Clear();
            ShortCutHandler.GetShortCuts(QP.QuickPickModel);
            QP.ButtonManager.AddShortCuts();
        }

        private QuickPickSettings DeserialiseSettingsFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                var SettingsAsJson = File.ReadAllText(SettingsPath);
                QuickPickSettings settings = JsonConvert.DeserializeObject<QuickPickSettings>(SettingsAsJson);
                return settings;
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to load QuickPickSettings");
                return null;
            }
        }
    }
}