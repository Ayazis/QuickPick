using QuickPick.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuickPick.Models
{
    public class QuickPick
    {
        public QuickPickModel QuickPickModel { get; set; }
        public ButtonManager ButtonManager { get; set; }
        public ButtonActions ClickActions { get; set; }
        public HotKeys HotKeys { get; set; }        
        public ISaveLoader SaveLoader { get; set; }        
        public WindowManager WindowManager{ get; set; }

        public QuickPick()
        {          
            QuickPickModel = new QuickPickModel();
            ButtonManager = new ButtonManager(this);         
            WindowManager = new WindowManager(this);
            HotKeys = new HotKeys(this);
            SaveLoader = new JsonSaveLoader(this);
            ClickActions = new ButtonActions(this);

            WindowManager.Start();
        
        }

    }
}
