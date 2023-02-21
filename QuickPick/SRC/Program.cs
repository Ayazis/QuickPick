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
            MainWindow mainWindow= new MainWindow();            
            mainWindow.Show(); // Mandatory for getting thumbnail registrations.

            var processes = Process.GetProcesses().Where(w => !string.IsNullOrEmpty(w.MainWindowTitle));
            var windowHandle = processes.First(f => f.MainWindowHandle != IntPtr.Zero).MainWindowHandle;
            var currentProcess = Process.GetCurrentProcess();
            windowHandle = currentProcess.MainWindowHandle;

            var allOpenWindows = ActiveApps.GetAllOpenWindows();

            int size = 300;
            int x = 0;
            int y = 0;
            int xmax = size;
            int ymax = size;
            foreach (var process in allOpenWindows)
            {
                var thumbHandle = Thumbnails.GetThumbnailRelations(process.MainWindowHandle, windowHandle);
                if (thumbHandle == default) continue;
                RECT rect = new RECT(x, y, xmax, ymax);
                Thumbnails.CreateThumbnail(thumbHandle, rect);                     
                x += size;                
                xmax += size;               
            }
            System.Threading.Thread.Sleep(2000);
            
            return;
            try
            {
				//ActiveApps.GetAllOpenWindows();

				// Set Keyboard and Mouse Hooks for click Events.
				CaptureKeyBoardAndMouse.SetInputHooks();
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