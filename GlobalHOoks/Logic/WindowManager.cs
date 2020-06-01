using GlobalHOoks.Classes;
using GlobalHOoks.Logic;
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

namespace GlobalHOoks
{
    public class WindowManager
    {
        private SaveLoadManager _saveLoadManager;
        private QuickPickModel _qpm;
        static ClickWindow ClickWindow = null;
        private ButtonManager _buttonManager;
        static SettingsWindow SettingsWindow = null;
       
        private NotifyIcon _notificationIcon;
        static IntPtr _ActiveWindowHandle;
        private HotKeys _hotkeys;

        public Storyboard Hide { get; private set; }
        public Storyboard Show { get; private set; }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

      

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);


        public WindowManager()
        {            
            CreateTrayIcon();            
            CreateWindow();
            FindResources();
            _hotkeys = new HotKeys(_qpm,this);
           Hook.GlobalEvents().KeyDown += _hotkeys.KeyDown;
           Hook.GlobalEvents().KeyUp += _hotkeys.KeyUp;
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
            var IconPath = $@"{currentPath}Files\QP_Icon_32px.png";

            var bitmap = new Bitmap(IconPath); 
            var iconHandle = bitmap.GetHicon();
      
            return Icon.FromHandle(iconHandle);
        }

      

        private void MnuSettings_Click(object sender, EventArgs e)
        {
            if (SettingsWindow == null)
            {
                SettingsWindow = new SettingsWindow(_qpm, this, _buttonManager); ;
                //SettingsWindow.WindowStyle = WindowStyle.None;
                SettingsWindow.DataContext = _qpm;
            }
            SettingsWindow.Show();
        }
     

        private void MouseDown(object sender, MouseEventArgs e)
        {
       
            // Hide the window it user clicked outside of window.
            if (e.Button != MouseButtons.XButton2)
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

            try
            {
                try
                {
                    SetActiveWindow();
                }
                catch (Exception)
                {
                }

                if (ClickWindow != null)
                {
                    ShowWindow();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void CreateWindow()
        {
            _qpm = new QuickPickModel();
            ClickWindow = new ClickWindow(_qpm, this);
            
            _buttonManager = new ButtonManager(_qpm, ClickWindow, this);
            _saveLoadManager = new SaveLoadManager(_qpm,_buttonManager);
            ClickWindow.Initialize(_buttonManager);                                
            
            ClickWindow.DataContext = _qpm;
            ClickWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            ClickWindow.WindowStyle = System.Windows.WindowStyle.None;
            ClickWindow.Topmost = true;
            ClickWindow.Visibility = System.Windows.Visibility.Hidden;
            ClickWindow.Show();
            ClickWindow.Closed += Window_Closed;
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
                ClickWindow.Dispatcher.Invoke(() => {
                    HideShortCuts();
                    var mousePosition = GetMousePosition();
                    ClickWindow.Left = mousePosition.X - (ClickWindow.ActualWidth / 2);
                    ClickWindow.Top = mousePosition.Y - (ClickWindow.ActualHeight / 2);
                    Show.Begin(ClickWindow);
                });
           
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }    
        }

        private System.Windows.Point GetMousePosition()
        {
            System.Drawing.Point point = Control.MousePosition;
            return new System.Windows.Point(point.X, point.Y);
        }

        public static void ReActivateFormerWindow()
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
            catch (Exception)
            {

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
            foreach (var b in _qpm.ShortCutButtons)
            {
                b.Icon.Visibility = Visibility.Visible;
            }
        }
        public void HideShortCuts()
        {
            foreach (var b in _qpm.ShortCutButtons)
            {
                b.Icon.Visibility = Visibility.Hidden;
            }
        }

    }
}
