using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickPick.UI.Views
{
    /// <summary>
    /// Interaction logic for MusicControl.xaml
    /// </summary>
    public partial class MusicControl : UserControl
    {
        public MusicControl()
        {
            InitializeComponent();
            CreateScrollingTextAnimation();

        }

        void CreateScrollingTextAnimation()
        {
            // todo: set LineScroll width
            // fix animation when going home.

            // set timer
            // on timer tick call ScrollText();
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ScrollText();
        }

        void ScrollText()
        {
            
            TitleScroller.LineRight();
            if (TitleScroller.HorizontalOffset == TitleScroller.ScrollableWidth)
            {
                TitleScroller.ScrollToHome();
            }
        }
    }

    public partial class MusicControlViewModel : ObservableObject
    {
        public MusicControlViewModel()
        {

        }
        // [ObservableProperty]
        public string TitlePlaying { get; set; } = "Nora en Pure - Radio 388";

        public void UpdateTitle(string title)
        {
            TitlePlaying = title;
        }
    }
}
