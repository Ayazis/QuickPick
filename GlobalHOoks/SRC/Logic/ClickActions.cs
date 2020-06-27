using QuickPick.Classes;
using QuickPick.Models;
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

namespace QuickPick.Logic
{
    public class ClickActions
    {       

        public Models.QuickPick QP { get; set; }    

        public ClickActions(Models.QuickPick qp)
        {
            this.QP = qp;            
        }

        public void ButtonClick(QpButton button)
        {
            if (button.Act != null)
                button.Act(button);
        }

        /// <summary>
        /// Used to read a .sql file, copy and paste it's commands and run the query by simulating the F5-key.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="run"></param>
        public void ReadAndRunQuery(QpButton qpb)
        {
            try
            {
                
                var text = File.ReadAllText(qpb.AssociatedFilePath);
                Clipboard.SetText(text);

               // _window.Visibility = Visibility.Hidden;
                QP.WindowManager.ReActivateFormerWindow();
                InputSim.CtrlN();
                InputSim.Paste();

                //if(!text.ToUpper().Contains("INSERT"))
                //    InputSim.F5();

                QP.WindowManager.Hide.Begin(QP.WindowManager.ClickWindow);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
        public void LaunchApplication(QpButton qpbutton)
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

                QP.WindowManager.Hide.Begin(QP.WindowManager.ClickWindow);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
        public void CloseQuickPick(QpButton button)
        {
            QP.WindowManager.ClickWindow.Close();
            System.Windows.Forms.Application.Exit();
        }
        public void TakeScreenSnip(QpButton button)
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
                QP.WindowManager.ClickWindow.Visibility = Visibility.Hidden;

                QP.WindowManager.Hide.Begin(QP.WindowManager.ClickWindow);

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }

        



        }
       


    }
}
