using QuickPick.UI.Views.Settings;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickPick
{
    public class Settings
    {
        public ActiveAppSetting ActiveAppSetting { get; set; } = ActiveAppSetting.IncludePinnedTaskBarApps;
        public AutoUpdateSetting AutoUpdateSetting { get; set; } = AutoUpdateSetting.PreRelease;
        public HashSet<Keys> KeyCombination { get; set; } = new() { Keys.LControlKey, Keys.RButton };
    }
}
