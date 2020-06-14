﻿using GlobalHOoks.Logic;
using GlobalHOoks.Models;
using Gma.System.MouseKeyHook;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GlobalHOoks
{
    class Program
    {
        private static WindowManager _manager;
        private static QuickPick _QP;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Hook.GlobalEvents().KeyDown += Program_KeyDown;
                Hook.GlobalEvents().KeyUp += Program_KeyUp;

                _QP = new QuickPick();
                
                Application.Run(new ApplicationContext());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private static void Program_KeyUp(object sender, KeyEventArgs e)
        { 
            Debug.WriteLine("Up:" + e.KeyCode.ToString());
            HotKeys.KeyUpped(e.KeyCode);
        }

        private static void Program_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("Down:" + e.KeyCode.ToString());
            HotKeys.KeyDowned(e.KeyCode);
        }
    }
}
