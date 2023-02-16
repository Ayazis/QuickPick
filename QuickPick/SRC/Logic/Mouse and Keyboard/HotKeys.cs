using QuickPick.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace QuickPick.Logic
{
    public static class HotKeys
    {
        static List<Keys> PressedKeys = new List<Keys>();
        public static Models.QuickPick QP { get; private set; }

        private static QuickPickModel _qpm;
        private static WindowManager _windowManager;

        public static void Initialise(Models.QuickPick quickPick)
        {
            QP = quickPick;
            _qpm = QP.QuickPickModel;
            _windowManager = QP.WindowManager;
            
        }



        public static void KeyDowned(Keys key)
        {
            try
            {
                if (key == Keys.LButton)
                    HideWindowIfNeeded();

                if (_qpm.HotKeys.Contains(key))
                {
                    // Don't add it again if it's already there. This may happen when a user holds down a key.
                    if(!PressedKeys.Contains(key))
                        PressedKeys.Add(key);
                   // Debug.WriteLine("Down:  " + key.ToString());

                    if (PressedKeys.Count == _qpm.HotKeys.Count)
                        CheckHotKeyCombo();
                }
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        public static void KeyUpped(Keys key)
        {
            try
            {
                if ( _qpm.HotKeys.Contains(key))
                {
                    PressedKeys.Remove(key);
                    Debug.WriteLine("Up: " + key.ToString());
                }
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        public static void HideWindowIfNeeded()
        {
			if (_windowManager.ClickWindow != null)
			{
				if (_windowManager.MouseIsOutsideWindow())
				{
					_windowManager.ClickWindow.WindowStyle = WindowStyle.None;
					_windowManager.Hide.Begin(_windowManager.ClickWindow);
				}
			}
		}

        private static void CheckHotKeyCombo()
        {
            try
            {
                Logs.Logger.Log("checking hotkeycombo");

				// Debug.WriteLine("CHECKING");
				bool allPressed = true;
                foreach (Keys key in _qpm.HotKeys)
                {
                    if (!PressedKeys.Contains(key))
                        allPressed = false;
                }
                if (allPressed)
                {
                  //  Debug.WriteLine("***** SHOWING THE WINDOW *****");
                   // Logs.Logger.Log("showing the window...");
                    PressedKeys.Clear();
                    _windowManager.ShowWindow();
                }
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }      

    }
}
