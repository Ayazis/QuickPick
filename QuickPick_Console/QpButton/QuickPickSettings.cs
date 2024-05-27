using System.Collections.ObjectModel;

namespace QuickPick.Models
{    public class QuickPickSettings
    {      
        public int NrOfMainButtons { get; set; }
        public string ShortCutsFolder { get; set; }
        public ObservableCollection<QpButton> MainButtons{ get; set; }
    }
}
