using GlobalHOoks.Classes;
using GlobalHOoks.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlobalHOoks.Logic
{
    public class SaveLoadManager
    {
        public string SettingsPath { get; set; }

        public QuickPick QP { get; set; }
        public SaveLoadManager(QuickPick qp)
        {
            this.QP = qp;
            this.SettingsPath = AppDomain.CurrentDomain.BaseDirectory + @"Settings.Json";
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
                if (File.Exists(SettingsPath))
                {
                    // Get SettingsFile From Disk
                    var SettingsAsJson = File.ReadAllText(SettingsPath);
                    QuickPickSettings settings = JsonConvert.DeserializeObject<QuickPickSettings>(SettingsAsJson);
                    QP.QuickPickModel.NrOfButtons = settings.NrOfMainButtons;
                    QP.QuickPickModel.ShortCutsFolder = settings.ShortCutsFolder;


                    // Get Shortcuts from saved folderLocation.
                    QP.QuickPickModel.ShortCuts.Clear();
                    ShortCutHandler.GetShortCuts(QP.QuickPickModel);
                    QP.ButtonManager.AddShortCuts();

                    // Create mainButtons.
                    QP.QuickPickModel.MainButtons.Clear();
                    foreach (var button in settings.MainButtons)
                    {
                        QP.ButtonManager.ConfigureButton(button);
                        QP.QuickPickModel.MainButtons.Add(button);                     
                    }

                    QP.ButtonManager.PlaceButtons();

                }
            
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}
