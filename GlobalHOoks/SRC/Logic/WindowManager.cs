using QuickPick.Classes;
using QuickPick.Logic;
using QuickPick.Models;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace QuickPick
{
    public class WindowManager
    {
        public Models.QuickPick QP { get; set; }
        private NotifyIcon _notificationIcon;
        static IntPtr _ActiveWindowHandle;

        public ClickWindow ClickWindow { get; set; }
        private SettingsWindow _settingsWindow;

        public Storyboard Hide { get; private set; }
        public Storyboard Show { get; private set; }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);


        public WindowManager(Models.QuickPick quickPick)
        {
            QP = quickPick;
        }

        public void Start()
        {
            CreateTrayIcon();
            CreateWindow();
            FindResources();
            Hook.GlobalEvents().MouseDown += MouseDown;
        }

        private void FindResources()
        {
            this.Hide = ClickWindow.TryFindResource("hideMe") as Storyboard;
            this.Show = ClickWindow.TryFindResource("showMe") as Storyboard;
        }

        private void CreateTrayIcon()
        {
            // CREATE TRAYICON
            var menu = new ContextMenu();
            var mnuExit = new MenuItem("Exit");
            var mnuSettings = new MenuItem("Settings");
            menu.MenuItems.Add(0, mnuExit);
            menu.MenuItems.Add(0, mnuSettings);

            _notificationIcon = new NotifyIcon()
            {
                Icon = CreateIcon(),
                //Icon = new Icon(SystemIcons.Warning, 40, 40),
                ContextMenu = menu,
                Text = "Main"
            };

            mnuExit.Click += new EventHandler(mnuExit_Click);
            mnuSettings.Click += MnuSettings_Click;
            _notificationIcon.Visible = true;
        }

        private Icon CreateIcon()
        {
            var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            var IconPath = $@"{currentPath}\SRC\Files\QP_Icon_32px.png";

            var bitmap = new Bitmap(IconPath);
            var iconHandle = bitmap.GetHicon();

            return Icon.FromHandle(iconHandle);
        }


        private void MnuSettings_Click(object sender, EventArgs e)
        {
            try
            {
                _settingsWindow = new SettingsWindow(QP);

                //SettingsWindow.WindowStyle = WindowStyle.None;
                _settingsWindow.DataContext = QP.QuickPickModel;


                _settingsWindow.Show();

                var left = _settingsWindow.Left;
                var top = _settingsWindow.Top;
                var height = _settingsWindow.Height;

                QP.WindowManager.ClickWindow.Left = left - 50 - QP.WindowManager.ClickWindow.Width;
                QP.WindowManager.ClickWindow.Top = top;
                QP.WindowManager.Show.Begin(QP.WindowManager.ClickWindow);
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }


        private void MouseDown(object sender, MouseEventArgs e)
        {
            // Hide the window it user clicked outside of window.
            if (QP.QuickPickModel.Hotkey == Enums.HotKey.XMouse1 && e.Button != MouseButtons.XButton1 ||
                QP.QuickPickModel.Hotkey == Enums.HotKey.XMouse2 && e.Button != MouseButtons.XButton2 ||
                    QP.QuickPickModel.Hotkey == Enums.HotKey.KeyCombination)
            {
                if (MouseIsOutsideWindow())
                {
                    if (ClickWindow != null)
                    {
                        Hide.Begin(ClickWindow);
                        //   Window.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
                return;
            }

            else if (QP.QuickPickModel.Hotkey == Enums.HotKey.XMouse1 && e.Button == MouseButtons.XButton1 ||
                     QP.QuickPickModel.Hotkey == Enums.HotKey.XMouse2 && e.Button == MouseButtons.XButton2)
            {

                try
                {

                    if (ClickWindow != null)
                    {
                        ShowWindow();
                    }
                }
                catch (Exception ex)
                {
                    Logs.Logger.Log(ex);
                }
            }
        }
        private void CreateWindow()
        {
            try
            {
                ClickWindow = new ClickWindow(QP);
                QP.SaveLoadManager.LoadSettingsFile();

                ClickWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                ClickWindow.WindowStyle = WindowStyle.None;
                ClickWindow.Topmost = true;
                ClickWindow.Show();
                ClickWindow.Visibility = Visibility.Hidden;
                ClickWindow.Closed += Window_Closed;
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _notificationIcon.Dispose();
        }

        private bool MouseIsOutsideWindow()
        {
            var mouse = GetMousePosition();

            bool isOutside = (mouse.X < ClickWindow.Left || mouse.X > ClickWindow.Left + ClickWindow.ActualWidth)
                            || (mouse.Y < ClickWindow.Top || mouse.Y > ClickWindow.Top + ClickWindow.ActualHeight);

            return isOutside;
        }

        public void ShowWindow()
        {
            try
            {
                SetActiveWindow();

                ClickWindow.Dispatcher.Invoke(() =>
                {
                    HideShortCuts();
                    var mousePosition = GetMousePosition();
                    ClickWindow.Left = mousePosition.X - (ClickWindow.ActualWidth / 2);
                    ClickWindow.Top = mousePosition.Y - (ClickWindow.ActualHeight / 2);
                    Show.Begin(ClickWindow);
                });

                if(QP.QuickPickModel.InstantShortCuts)
                {
                   ShowShortCuts();
                }

            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        private System.Windows.Point GetMousePosition()
        {
            return MousePosition.GetCursorPosition();      
        }

        public void ReActivateFormerWindow()
        {
            try
            {
                SetForegroundWindow(_ActiveWindowHandle);
            }
            catch (Exception)
            {
                // Do nothing, except log..?
            }
        }

        public void SetActiveWindow()
        {
            try
            {
                const int nChars = 256;
                StringBuilder Buff = new StringBuilder(nChars);
                _ActiveWindowHandle = GetForegroundWindow();
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }

        public void HideWindow(QpButton button)
        {
            if (Hide != null)
                Hide.Begin();
        }
        private void mnuExit_Click(object sender, EventArgs e)
        {
            _notificationIcon.Visible = false;
            _notificationIcon.Dispose();
            System.Windows.Forms.Application.Exit();
        }

        public void ShowShortCuts()
        {
            foreach (var b in QP.QuickPickModel.ShortCutButtons)
            {
                b.Icon.Visibility = Visibility.Visible;
            }
        }
        public void HideShortCuts()
        {
            foreach (var b in QP.QuickPickModel.ShortCutButtons)
            {
                b.Icon.Visibility = Visibility.Hidden;
            }
        }

    }
}
