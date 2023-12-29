using QuickPick.Models;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Point = System.Windows.Point;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuickPick.UI.Views.QuickPickMainWindow
{
    public partial class QuickPickModel : ObservableObject
    {
        [ObservableProperty]
        private string _Title = "QuickPick Settings                v.1.2.1alpha";
  
        [ObservableProperty]
        private string _ShortCutsFolder = @"c:\shortcuts";
    

        [ObservableProperty]
        private bool _SettingsAreSaved = false;       
        

        public bool InstantShortCuts { get; set; }

        
        [ObservableProperty]
        private ObservableCollection<Keys> _HotKeys = new ObservableCollection<Keys> {
      //   Keys.LControlKey,
            Keys.LMenu,
            Keys.RButton
        };
  
        [ObservableProperty]
        private ObservableCollection<ShortCut> _ShortCuts = new();


        [ObservableProperty]
        private ObservableCollection<QpButton> _ShortCutButtons = new();


        [ObservableProperty]
        private ObservableCollection<QpButton> _MainButtons = new();

           

    }
}
