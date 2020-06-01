using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlobalHOoks.Logic
{
    internal class HotKeys
    {
        static List<Keys> PressedKeys = new List<Keys>();

        private QuickPickModel _qpm;
        private WindowManager _windowManager;

        public HotKeys(QuickPickModel model, WindowManager manager)
        {
            this._qpm = model;
            this._windowManager = manager;
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            Task.Run(() => KeyDowned(e.KeyCode));         
        }
        public void KeyUp(object sender, KeyEventArgs e)
        {
            Task.Run(() => KeyUpped(e.KeyCode));         
        }


        private void KeyDowned(Keys key)
        {
            
            if (_qpm.HotKeys.Contains(key))
            {
                PressedKeys.Add(key);
                Debug.WriteLine("Down:  " + key.ToString());

                if (PressedKeys.Count == _qpm.HotKeys.Count)
                    CheckHotKeyCombo();
            }
        }

        private void KeyUpped(Keys key)
        {
            if (_qpm.HotKeys.Contains(key))
            {
                PressedKeys.Remove(key);
                Debug.WriteLine("Up: " + key.ToString());
            }

        }

        private void CheckHotKeyCombo()
        {
            Debug.WriteLine("CHECKING");
            var allPressed = true;
            foreach (var key in _qpm.HotKeys)
            {
                if (!PressedKeys.Contains(key))
                    allPressed = false;
            }
            if (allPressed)
            {
                // Debug.WriteLine("***** SHOWING THE WINDOW *****");
                 _windowManager.ShowWindow();
            }
        }


    }
}
