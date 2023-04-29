using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPick
{
    public class QuickPickMainWindowModel
    {
        public ObservableCollection<string> ButtonLabels { get; set; } = new ObservableCollection<string>() { "1", "2", "3", "4", "5", "7", "2", "7", "2" };
    }
}
