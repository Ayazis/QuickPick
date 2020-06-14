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
        private const string SETTINGS_PATH = @"c:\temp\quickPick\qp_settings.json";

        public QuickPick QP{ get; set; }
        public SaveLoadManager(QuickPick qp)
        {
            this.QP = qp;
        }


        public void SaveSettingsToDisk()
        {
            if (!Directory.Exists(SETTINGS_PATH))
                Directory.CreateDirectory(SETTINGS_PATH);
            //string mainButtonsAsJson = JsonConvert.SerializeObject(QP.QuickPickModel.MainButtons, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});        
            
            //string shortcutButtonsJson = JsonConvert.SerializeObject(QP.QuickPickModel.ShortCutButtons, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            //File.WriteAllText($@"{_saveDirectory}{_shorts}", shortcutButtonsJson); 
            var settings = new QuickPickSettings(QP.QuickPickModel);
            string settingsAsJson = JsonConvert.SerializeObject(settings, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});        

            File.WriteAllText(SETTINGS_PATH, settingsAsJson);
        }

        public void LoadSettingsFromDisk()
        {
            try
            {              
                if (File.Exists(SETTINGS_PATH))
                {
                    var SettingsAsJson = File.ReadAllText(SETTINGS_PATH);
                    QuickPickSettings settings = JsonConvert.DeserializeObject<QuickPickSettings>(SettingsAsJson);
                    
                    QP.QuickPickModel.NrOfButtons = settings.NrOfMainButtons;
                    QP.QuickPickModel.ShortCutsFolder = settings.ShortCutsFolder;

                    QP.QuickPickModel.MainButtons.Clear();
                    foreach (var button in QP.QuickPickModel.MainButtons)
                    {
                        QP.QuickPickModel.MainButtons.Add(button);
                    }

                    ShortCutHandler.GetShortCuts(QP.QuickPickModel);

                }                

                //string shortCutButtonsFilePath = $@"{SETTINGS_PATH}{_shorts}";

                //if (File.Exists(shortCutButtonsFilePath))
                //{
                //    var shortsAsJson = File.ReadAllText(shortCutButtonsFilePath);
                //    List<QpButton> ShortcutButtons = JsonConvert.DeserializeObject<List<QpButton>>(shortsAsJson);

                //    QP.QuickPickModel.ShortCutButtons.Clear();
                //    foreach (var shortcut in ShortcutButtons)
                //    {
                //        QP.QuickPickModel.ShortCutButtons.Add(shortcut);
                //    }                   
                //}
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }      
    }
}
