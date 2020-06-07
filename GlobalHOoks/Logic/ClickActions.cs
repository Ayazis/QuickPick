using GlobalHOoks.Classes;
using GlobalHOoks.Models;
using Shell32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace GlobalHOoks.Logic
{
    public class ClickActions
    {
        public const string FILES_FOLDER = @"C:\Users\FG\source\repos\GlobalHooks\GlobalHOoks\Content\Files\";
      
        private WindowManager _windowManager;

        public ClickActions(WindowManager windowManager)
        {            
            _windowManager = windowManager;
        }

        internal void ButtonClick(QpButton button)
        {
            if (button.Act != null)
                button.Act(button);
        }

        /// <summary>
        /// Used to read a .sql file, copy and paste it's commands and run the query by simulating the F5-key.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="run"></param>
        internal void ReadAndRunQuery(QpButton qpb)
        {
            try
            {
                
                var text = File.ReadAllText(qpb.AssociatedFilePath);
                Clipboard.SetText(text);

               // _window.Visibility = Visibility.Hidden;
                WindowManager.ReActivateFormerWindow();
                InputSim.CtrlN();
                InputSim.Paste();

                if(!text.ToUpper().Contains("INSERT"))
                    InputSim.F5();

                _windowManager.Hide.Begin(WindowManager.ClickWindow);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
        internal void LaunchApplication(QpButton qpbutton)
        {
            try
            {
                if (!File.Exists(qpbutton.AssociatedFilePath))
                    return;

                var startinfo = new ProcessStartInfo(qpbutton.AssociatedFilePath);
                startinfo.UseShellExecute = true;

                var proc = new Process();
                proc.StartInfo = startinfo;
                proc.Start();

                _windowManager.Hide.Begin(WindowManager.ClickWindow);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
        internal void CloseQuickPick(QpButton button)
        {
            WindowManager.ClickWindow.Close();
            System.Windows.Forms.Application.Exit();
        }
        internal void TakeScreenSnip(QpButton button)
        {
            try
            {
                // WinKey + Shift + S
                var winShift = new List<VirtualKeyCode>
                    {
                        VirtualKeyCode.LWIN,
                        VirtualKeyCode.LSHIFT
                    };

                InputSim.Simulator.Keyboard.ModifiedKeyStroke(winShift, VirtualKeyCode.VK_S);
                WindowManager.ClickWindow.Visibility = Visibility.Hidden;

                _windowManager.Hide.Begin(WindowManager.ClickWindow);

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }

        



        }
       


    }
}
