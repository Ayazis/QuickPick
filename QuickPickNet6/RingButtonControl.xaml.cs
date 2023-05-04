using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace QuickPick
{
    /// <summary>
    /// Interaction logic for RingButtonControl.xaml
    /// </summary>
    public partial class RingButtonControl : UserControl
    {
        public RingButtonControl()
        {
            InitializeComponent();
        }
        private void BottomRight_MouseEnter(object sender, MouseEventArgs e)
        {
            QuadrantEnter(sender as Path);            
        }

      
        private void BottomRight_MouseLeave(object sender, MouseEventArgs e)
        {
            QuadrantLeave(sender as Path);
            // Code to execute when the mouse leaves the BottomRight Path element
        }

        private void BottomLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            QuadrantEnter(sender as Path);
        }

        private void BottomLeft_MouseLeave(object sender, MouseEventArgs e)
        {
            QuadrantLeave(sender as Path);
        }

        private void TopLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            QuadrantEnter(sender as Path);
        }

        private void TopLeft_MouseLeave(object sender, MouseEventArgs e)
        {
            QuadrantLeave(sender as Path);
      }

        private void TopRight_MouseEnter(object sender, MouseEventArgs e)
        {
            QuadrantEnter(sender as Path);
        }

        private void TopRight_MouseLeave(object sender, MouseEventArgs e)
        {
            QuadrantLeave(sender as Path);
        }
        private void QuadrantEnter(Path path)
        {
            path.Fill = Brushes.Black;
        }
        private void QuadrantLeave(Path path)
        {
            path.Fill = Brushes.Transparent;            
        }
    }
}
