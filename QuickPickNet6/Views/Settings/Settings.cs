using Newtonsoft.Json;
using QuickPick.UI.Views.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuickPick
{
    public class Settings
    {
        public ActiveAppSetting ActiveAppSetting { get; set; } = ActiveAppSetting.IncludePinnedTaskBarApps;
        public AutoUpdateSetting AutoUpdateSetting { get; set; } = AutoUpdateSetting.PreRelease;

        [JsonIgnore]
        public HashSet<Keys> KeyCombination { get; set; } = new() { Keys.LControlKey, Keys.RButton };

        [JsonProperty]
        private HashSet<string> KeyCombinationAsStrings;
        public string Serialise()
        {
            // Set the key combination as a string so that it can be serialised
            // This is because the keys enum will not Deserialise properly.
            KeyCombinationAsStrings = new HashSet<string>(KeyCombination.Select(k => k.ToString()));
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Settings Deserialize(string json)
        {
            // Deserialise the key combination as string, and then convert it back to a hashset of keys.
            var settings = JsonConvert.DeserializeObject<Settings>(json);
            if(settings.KeyCombinationAsStrings != null)
                settings.KeyCombination = new HashSet<Keys>(settings.KeyCombinationAsStrings.Select(k => Enum.Parse<Keys>(k)));
            return settings;
        }
    }
}
