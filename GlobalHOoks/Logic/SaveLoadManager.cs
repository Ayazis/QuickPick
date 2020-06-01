using GlobalHOoks.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalHOoks.Logic
{
    internal class SaveLoadManager
    {
        private QuickPickModel _qpm;
        private ButtonManager _buttonManager;
        private string _saveDirectory = @"c:\temp\quickPick\";
        private string _mains = "qp_saveMains.json";
        private string _shorts = "qp_saveShortCuts.json";

        public SaveLoadManager(QuickPickModel qpm, ButtonManager buttonManager)
        {
            _qpm = qpm;
            _buttonManager = buttonManager;
        }


        public void SaveSettingsToDisk()
        {
            if (!Directory.Exists(_saveDirectory))
                Directory.CreateDirectory(_saveDirectory);


            string mainButtonsAsJson = JsonConvert.SerializeObject(_qpm.MainButtons, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});        
            File.WriteAllText($@"{_saveDirectory}{_mains}", mainButtonsAsJson);

            string shortcutButtonsJson = JsonConvert.SerializeObject(_qpm.ShortCutButtons, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText($@"{_saveDirectory}{_shorts}", shortcutButtonsJson);

            LoadSettingsFromDisk();
        }

        public void LoadSettingsFromDisk()
        {
            var mainsAsJson = File.ReadAllText($@"{_saveDirectory}{_mains}");
            List<QpButton> mainButtons = JsonConvert.DeserializeObject<List<QpButton>>(mainsAsJson);

            var shortsAsJson = File.ReadAllText($@"{_saveDirectory}{_shorts}");
            List<QpButton> ShortcutButtons = JsonConvert.DeserializeObject<List<QpButton>>(shortsAsJson);
        }

    }
}
