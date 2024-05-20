using QuickPick.UI.BrightnessControls;
using QuickPick.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace QuickPick.UI.BrightnessControls
{
    public class BrightnessControl : IValueHandler
    {

        Dictionary<string, BrightnessDimmerWindow> _windowDimmers;

        Dictionary<string, BrightnessDimmerWindow> WindowDimmers => _windowDimmers ??= CreateWindowDimmers();


        static BrightnessControl _instance;
        public static BrightnessControl Instance => _instance ??= new BrightnessControl();
        public BrightnessControl()
        {

        }

        public void HandleNewValue(double brightnessLevel)
        {
            foreach (var item in WindowDimmers)
            {
                SetBrightness((int)brightnessLevel, item.Key);
            }
        }

        public void SetBrightness(int brightnessLevel, string monitorId)
        {
            bool foundWindow = WindowDimmers.TryGetValue(monitorId, out var dimmerWindow);
            if (foundWindow)
            {
                dimmerWindow.SetBrightness(brightnessLevel);
            }
        }
        Dictionary<string, BrightnessDimmerWindow> CreateWindowDimmers()
        {
            Dictionary<string, BrightnessDimmerWindow> windowDimmers = new();
            foreach (Screen screen in Screen.AllScreens)
            {
                BrightnessDimmerWindow window = new()
                {
                    Left = screen.Bounds.Left,
                    Top = screen.Bounds.Top,
                    Width = screen.Bounds.Width,
                    Height = screen.Bounds.Height,
                    WindowStartupLocation = WindowStartupLocation.Manual
                };
                window.Topmost = true;
                window.IsHitTestVisible = false;
                window.Show();
                windowDimmers.Add(screen.DeviceName, window);
            }
            return windowDimmers;
        }

    }

}
