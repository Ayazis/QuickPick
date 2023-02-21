using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using QuickPick.SRC.Logic;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using ThumbnailLogic;

namespace MyApp
{
    public partial class MainWindow : Window
    {
        private List<Image> _allApps = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
            applications.DataContext = _allApps;
        }

        //public void AddActiveAppThumbnails(Image image)
        //{
        //    var allActiveApps = ActiveApps.GetAllOpenWindows();
        //    double angle = 360 / allActiveApps.Count();

        //    int i = 0;


        //    applications.Children.Add(image);


        //}
    }

}
