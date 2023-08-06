
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickPick.UI.Views.QuickPickMainWindow;

namespace QuickPick.Models
{
    public class QuickPickSettings
    {
        public QuickPickSettings(QuickPickModel qpm)
        {
            NrOfMainButtons = qpm.NrOfButtons;
            ShortCutsFolder = qpm.ShortCutsFolder;
            MainButtons = qpm.MainButtons;
            InstantShortcuts = qpm.InstantShortCuts;
        }

        public QuickPickSettings()
        {

        }

        public bool InstantShortcuts { get; set; }
        public int NrOfMainButtons { get; set; }
        public string ShortCutsFolder { get; set; }
        public ObservableCollection<QpButton> MainButtons{ get; set; }

    }
}
