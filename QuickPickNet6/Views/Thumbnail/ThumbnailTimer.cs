using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace QuickPick.UI.Views.Thumbnail
{
    /// <summary>
    /// In charge of showing and hiding thumbnails based on a timer.
    /// </summary>
    public class ThumbnailTimer
    {
        private DispatcherTimer MouseLeaveTimer = new DispatcherTimer();
        Action _callback;

        public ThumbnailTimer(Action callback)
        {
            MouseLeaveTimer.Tick += MouseLeaveTimer_Tick;
                _callback = callback;
        }

      
        public void StartTimer()
        {
            MouseLeaveTimer.Stop(); // Reset if there was one already runnning.
            MouseLeaveTimer.Interval = TimeSpan.FromMilliseconds(500);
            MouseLeaveTimer.IsEnabled = true;
            MouseLeaveTimer.Start();
        }

        private void MouseLeaveTimer_Tick(object sender, EventArgs e)
        {
            StopTimer();
            _callback?.Invoke();           
        }

        public void StopTimer()
        {
            MouseLeaveTimer.Interval = default;
            MouseLeaveTimer.Stop();
            MouseLeaveTimer.IsEnabled = false;
        }

    }
}
