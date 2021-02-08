using QuickPick.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickPick.Logic
{
    public class HotKeys
    {
        static List<Keys> PressedKeys = new List<Keys>();
        public Models.QuickPick QP { get; set; }

        private static QuickPickModel _qpm;
        private static WindowManager _windowManager;

        public HotKeys(Models.QuickPick quickPick)
        {
            this.QP = quickPick;
            _qpm = QP.QuickPickModel;
            _windowManager = QP.WindowManager;
            
        }



        public static void KeyDowned(Keys key)
        {
            try
            {
                if (_qpm.Hotkey == Enums.HotKey.KeyCombination &&  _qpm.HotKeys.Contains(key))
                {
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
                if (_qpm.Hotkey == Enums.HotKey.KeyCombination &&  _qpm.HotKeys.Contains(key))
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

        private static void CheckHotKeyCombo()
        {
            try
            {
                Logs.Logger.Log("checking hotkeycombo");

                Debug.WriteLine("CHECKING");
                var allPressed = true;
                foreach (var key in _qpm.HotKeys)
                {
                    if (!PressedKeys.Contains(key))
                        allPressed = false;
                }
                if (allPressed)
                {
                    Debug.WriteLine("***** SHOWING THE WINDOW *****");
                    Logs.Logger.Log("showing the window...");
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
