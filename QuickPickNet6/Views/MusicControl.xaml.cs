using CommunityToolkit.Mvvm.ComponentModel;
using QuickPick.Logic;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QuickPick.UI.Views
{
    /// <summary>
    /// Interaction logic for MusicControl.xaml
    /// </summary>
    public partial class MusicControl : UserControl
    {
        private double _maxScrollLength;
        private bool _infititeScrollCreated;

        public MusicControl()
        {
            InitializeComponent();
            CreateScrollingTimer();
        }

        private void CreateScrollingTimer()
        {
            // todo: set LineScroll width
            // fix animation when going home.

            // set timer
            // on timer tick call ScrollText();
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

      
        private void Timer_Tick(object sender, EventArgs e)
        {
            if(!_infititeScrollCreated)
            { 
                
                // save the original width, and double the title text for infinite scroll effect.
                var title = MusicControlViewModel.Instance.TitlePlaying;
                MusicControlViewModel.Instance.TitlePlaying = $"{title}{title.Trim()}";
                _maxScrollLength = TitleScroller.ExtentWidth;
                _infititeScrollCreated = true;
            }

            ScrollText();
        }

        void ScrollText()
        {
            Task.Run(() => 
            {

                Dispatcher.Invoke(() => 
                {
                    TitleScroller.ScrollToHorizontalOffset(TitleScroller.HorizontalOffset + 1);
                    // If we've reached the end of the first copy of the text, jump back to the start
                    if (TitleScroller.HorizontalOffset >= _maxScrollLength)
                    {
                        TitleScroller.ScrollToLeftEnd();
                    }
                });
            
            });
        }

        private void PlayButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            InputSim.PlayPause();
        }
    }

    public partial class MusicControlViewModel : ObservableObject
    {
        public static MusicControlViewModel Instance { get; set; } 

        public MusicControlViewModel()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("MusicControlViewModel already exists");
            }
        }
        [ObservableProperty]
        string _titlePlaying = "Nora en Pure - Radio 388    ";

        public void UpdateTitle(string title)
        {
            TitlePlaying = title;
        }
    }

}
