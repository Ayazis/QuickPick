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
        public Models.QuickPick QP { get; set; }
        public SaveLoadSettings(Models.QuickPick qp)
        {
            QP = qp;
        }

        public void SaveSettingsToDisk()
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(QP.QuickPickModel.SettingsPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(QP.QuickPickModel.SettingsPath));

                var settings = new QuickPickSettings(QP.QuickPickModel);
                string settingsAsJson = JsonConvert.SerializeObject(settings, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                string fileName = "";
                var saveDialog = new System.Windows.Forms.SaveFileDialog
                {
                    InitialDirectory = QP.QuickPickModel.SettingsPath,
                    Filter = "JSON files(*.json)|*.json"
                };

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = saveDialog.FileName;
                    QP.QuickPickModel.SettingsPath = fileName;
                }



                string finalPath = fileName != "" ? fileName : QP.QuickPickModel.SettingsPath;
                File.WriteAllText(finalPath, settingsAsJson);
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
            var settings = DeserialiseSettingsFile(QP.QuickPickModel.SettingsPath);
            ApplySettings(settings);

        }
        private void ApplySettings(QuickPickSettings settings)
        {
            if (settings == null)
            {
                RemoveExistingButtons();
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
                RemoveExistingButtons();

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

        private void RemoveExistingButtons()
        {         
            QP.QuickPickModel.MainButtons.Clear();
            QP.ButtonManager.ClearCanvas();
            QP.ButtonManager.AddCentralButton();
        }

        private QuickPickSettings DeserialiseSettingsFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                var SettingsAsJson = File.ReadAllText(QP.QuickPickModel.SettingsPath);
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