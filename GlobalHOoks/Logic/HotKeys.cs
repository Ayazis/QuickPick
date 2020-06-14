﻿using QuickPick.Models;
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


        public static void KeyDowned(Keys key)
        {
            try
            {
                if (_qpm.PreDefinedHotKeys.Contains(key))
                {
                    PressedKeys.Add(key);
                   // Debug.WriteLine("Down:  " + key.ToString());

                    if (PressedKeys.Count == _qpm.PreDefinedHotKeys.Count)
                        CheckHotKeyCombo();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public static void KeyUpped(Keys key)
        {
            try
            {
                if (_qpm.PreDefinedHotKeys.Contains(key))
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

        private static void CheckHotKeyCombo()
        {
            try
            {
                Logger.Log("checking hotkeycombo");

                Debug.WriteLine("CHECKING");
                var allPressed = true;
                foreach (var key in _qpm.PreDefinedHotKeys)
                {
                    if (!PressedKeys.Contains(key))
                        allPressed = false;
                }
                if (allPressed)
                {
                    Debug.WriteLine("***** SHOWING THE WINDOW *****");
                    Logger.Log("showing the window...");
                    _windowManager.ShowWindow();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }


    }
}
