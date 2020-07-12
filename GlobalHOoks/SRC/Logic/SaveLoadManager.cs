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
             QP = qp;
             SettingsPath = AppDomain.CurrentDomain.BaseDirectory + @"QuickPickSettings.json";
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
                Logger.Log(ex);
            }
        }

        public void LoadAndApplySettings()
        {
            try
            {
                // Clear the canvas
                QP.QuickPickModel.MainButtons.Clear();
                QP.ButtonManager.ClearCanvas();
                QP.ButtonManager.AddCentralButton();

                if (File.Exists(SettingsPath))
                {
                    // Get SettingsFile From Disk
                    var SettingsAsJson = File.ReadAllText(SettingsPath);
                    QuickPickSettings settings = JsonConvert.DeserializeObject<QuickPickSettings>(SettingsAsJson);
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

                // Get Shortcuts from saved folderLocation.
                QP.QuickPickModel.ShortCuts.Clear();
                ShortCutHandler.GetShortCuts(QP.QuickPickModel);
                QP.ButtonManager.AddShortCuts();
                

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}