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

        public  void KeyPress(object sender, KeyPressEventArgs e)
        {
            Task.Run(() => { Debug.WriteLine("Pressed:" + e.KeyChar.ToString()); });
        }


        public void KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("Down:" + e.KeyCode.ToString());
            //Task.Run(() => { Debug.WriteLine("Down:" + e.KeyCode.ToString()); });
           
           // Task.Run(() => KeyDowned(e.KeyCode));         
        }
        public void KeyUp(object sender, KeyEventArgs e)
        {
           // Task.Run(() => { Debug.WriteLine("Up:" + e.KeyCode.ToString()); });

            //Task.Run(() => KeyUpped(e.KeyCode));         
        }


        private void KeyDowned(Keys key)
        {

            try
            {
                if (_qpm.HotKeys.Contains(key))
                {
                    PressedKeys.Add(key);
                    Debug.WriteLine("Down:  " + key.ToString());

                    if (PressedKeys.Count == _qpm.HotKeys.Count)
                        CheckHotKeyCombo();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private void KeyUpped(Keys key)
        {
            try
            {
                if (_qpm.HotKeys.Contains(key))
                {
                    PressedKeys.Remove(key);
                    Debug.WriteLine("Up: " + key.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
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
                 Debug.WriteLine("***** SHOWING THE WINDOW *****");
                 _windowManager.ShowWindow();
            }
        }


    }
}
