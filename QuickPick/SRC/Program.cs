﻿using QuickPick.Logic;
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

				// Set Keyboard and Mouse Hooks for click Events.
				CaptureKeyBoardAndMouse.SetInputHooks();
                CaptureKeyBoardAndMouse.SetInputHooks();

				_QP = new Models.QuickPick();

                using (var context = new ApplicationContext())
                {
                    Application.Run(context);
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