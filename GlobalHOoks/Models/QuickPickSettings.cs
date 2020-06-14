using GlobalHOoks.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalHOoks.Models
{
    public class QuickPickSettings
    {
        public QuickPickSettings(QuickPickModel qpm)
        {
            NrOfMainButtons = qpm.NrOfButtons;
            ShortCutsFolder = qpm.ShortCutsFolder;
            MainButtons = qpm.MainButtons;
        }

        public QuickPickSettings()
        {

        }


        public int NrOfMainButtons { get; set; }
        public string ShortCutsFolder { get; set; }
        public ObservableCollection<QpButton> MainButtons{ get; set; }

    }
}
