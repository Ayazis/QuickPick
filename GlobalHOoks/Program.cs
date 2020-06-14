using QuickPick.Logic;
using QuickPick.Models;
using Gma.System.MouseKeyHook;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace QuickPick
{
    class Program
    {
        // Must have a variable to Keep the program running.
        private static Models.QuickPick _QP;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
               Logger.Log("Started at " + DateTime.Now.ToString()); 
               Hook.GlobalEvents().KeyDown += Program_KeyDown;
               Hook.GlobalEvents().KeyUp += Program_KeyUp;

                _QP = new Models.QuickPick();
                
                Application.Run(new ApplicationContext());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }


        }
        
        private static void Program_KeyUp(object sender, KeyEventArgs e)
        {
            //Debug.WriteLine("Up:" + e.KeyCode.ToString());       
            Logger.Log(e.KeyCode.ToString());
            HotKeys.KeyUpped(e.KeyCode);
        }

        private static void Program_KeyDown(object sender, KeyEventArgs e)
        {
            //Debug.WriteLine("Down:" + e.KeyCode.ToString());
            Logger.Log(e.KeyCode.ToString());
            HotKeys.KeyDowned(e.KeyCode);
        }
    }
}
