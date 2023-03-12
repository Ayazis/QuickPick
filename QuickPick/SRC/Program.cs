using QuickPick.Logic;
using QuickPick.Models;
using Gma.System.MouseKeyHook;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using QuickPick.SRC.Logic;
using MyApp;
using ThumbnailLogic;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;
using QuickPick.SRC.Logic.ScreenCaptures;
using System.Drawing;

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

				// Set Keyboard and Mouse Hooks for click Events.
				CaptureKeyBoardAndMouse.SetInputHooks();

				_QP = new Models.QuickPick();
                

                using (var context = new ApplicationContext())
                {
                    System.Windows.Forms.Application.Run(context);
                }
            

                CaptureKeyBoardAndMouse.UnhookWindowsHookEx(CaptureKeyBoardAndMouse._keyboardHookID);
				CaptureKeyBoardAndMouse.UnhookWindowsHookEx(CaptureKeyBoardAndMouse._mouseHookId);
			}
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);              
            }
        }    
    }
}