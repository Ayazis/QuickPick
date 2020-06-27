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
                KeyHook._hookID = KeyHook.SetHook(KeyHook._proc);

                Logger.Log("Started at " + DateTime.Now.ToString());            

                _QP = new Models.QuickPick();

                Application.Run(new ApplicationContext());

                KeyHook.UnhookWindowsHookEx(KeyHook._hookID);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }    
    }
}