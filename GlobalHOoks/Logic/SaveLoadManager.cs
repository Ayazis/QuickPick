using QuickPick.Classes;
using QuickPick.Models;
using Newtonsoft.Json;
using System;
using System.IO;


namespace QuickPick.Logic
{
    public class SaveLoadManager
    {
        public string SettingsPath { get; set; }

        public Models.QuickPick QP { get; set; }
        public SaveLoadManager(Models.QuickPick qp)
        {
            this.QP = qp;
            this.SettingsPath = AppDomain.CurrentDomain.BaseDirectory + @"Settings.json";
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

                LoadAndApplySettings();

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public void LoadAndApplySettings()
        {
            try
            {
                // Clear the canvas
                QP.ButtonManager.ClearCanvas();
                QP.ButtonManager.AddCentralButton();

                if (File.Exists(SettingsPath))
                {
                    // Get SettingsFile From Disk
                    var SettingsAsJson = File.ReadAllText(SettingsPath);
                    QuickPickSettings settings = JsonConvert.DeserializeObject<QuickPickSettings>(SettingsAsJson);
                    QP.QuickPickModel.NrOfButtons = settings.NrOfMainButtons;
                    QP.QuickPickModel.ShortCutsFolder = settings.ShortCutsFolder;               

                    // Create mainButtons.
                    QP.QuickPickModel.MainButtons.Clear();
                    foreach (var button in settings.MainButtons)
                    {
                        QP.ButtonManager.ConfigureButton(button);
                        QP.QuickPickModel.MainButtons.Add(button);
                    }

                    // Get Shortcuts from saved folderLocation.
                    QP.QuickPickModel.ShortCuts.Clear();
                    ShortCutHandler.GetShortCuts(QP.QuickPickModel);
                    QP.ButtonManager.AddShortCuts();                 
                    

                }
                else
                {
                    for (int i = 0; i < QP.QuickPickModel.NrOfButtons; i++)
                    {
                        var button = new QpButton();
                        button.Id = i + 1;
                        QP.ButtonManager.ConfigureButton(button);
                        QP.QuickPickModel.MainButtons.Add(button);
                    }
                }

                QP.ButtonManager.PlaceButtonsOnCanvas();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}
