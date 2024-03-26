using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuickPick.UI.BrightnessControl
{
    /// <summary>
    /// Interaction logic for BrightnessDimmerWindow.xaml
    /// </summary>
    public partial class BrightnessDimmerWindow : Window
    {
        public const int WS_EX_TRANSPARENT = 0x00000020; // Makes the window transparent to mouse events
        public const int WS_EX_TOOLWINDOW = 0x00000080; // Does not show up in alt-tab
        const int GWL_EXSTYLE = (-20);
        public BrightnessDimmerWindow()
        {
            MakeWindowTransparentForMouseEvents();
            InitializeComponent();
            MakeWindowTransparentForMouseEvents();
        }

        public void SetBrightness(int brightnessLevel)
        {
            // Set the background color of the window to black in the inverted brightness level
            var newAlphaLevel = (byte)(255 - brightnessLevel * 255 / 100);
            this.Background = new SolidColorBrush(Color.FromArgb(newAlphaLevel, 0, 0, 0));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            MakeWindowTransparentForMouseEvents();
        }

        private void MakeWindowTransparentForMouseEvents()
        {
            // Get this window's handle
            IntPtr hwnd = new WindowInteropHelper(this).Handle;

            // Change the extended window style to include WS_EX_TRANSPARENT and WS_EX_TOOLWINDOW
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW); // WS_EX_TOOLWINDOW
        }


        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
    }
}
