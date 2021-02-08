using QuickPick.Logic;
using QuickPick.Models;
using Gma.System.MouseKeyHook;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using QuickPick.SRC.Logic;

namespace QuickPick
{
    public class Program
    {
        // Must have a variable to Keep the program running. ?
        public static Models.QuickPick _QP;

        [STAThread]
        static void Main(string[] args)
        {          
            try
            {
                //ActiveApps.GetAllOpenWindows();


                _QP = new Models.QuickPick();

                using (var context = new ApplicationContext())
                {
                    Application.Run(context);
                }
            

                KeyHook.UnhookWindowsHookEx(KeyHook._hookID);
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);              
            }
        }    
    }
}