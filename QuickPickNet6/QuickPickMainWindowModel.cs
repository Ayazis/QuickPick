using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuickPick
{
    public class QuickPickMainWindowModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> ButtonLabels { get; set; } = new ObservableCollection<string>() { "1", "2", "3", "4", "5", "7", "8", "9", "10" };


        private int _CircleRadius = 50;
        public int CircleRadius
        {
            get { return _CircleRadius; }
            set
            {
                _CircleRadius = value;
                NotifyPropertyChanged(nameof(CircleRadius));
            }
        }
        public Point Center
        {
            get { return new Point(WidthHeight / 2, WidthHeight / 2); }
        }
        public int WidthHeight
        {
            get { return 200; }
        }

        #region Notify Property Changed And other Events
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
