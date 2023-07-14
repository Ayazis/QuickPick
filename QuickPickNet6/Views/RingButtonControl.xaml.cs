using QuickPick.Logic;
using System.Diagnostics;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;

using System.Windows.Shapes;

namespace QuickPick.UI.Views
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
        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            QuadrantEnter(sender as Path);
        }

        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            QuadrantLeave(sender as Path);
        }

        private void Path_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Path source = sender as Path;
            if (source.Name == nameof(this.TopRight))
            {
                InputSim.CtrlAltBreak();
                ClickWindow.HideWindow();
            }

        }

        private void QuadrantEnter(Path path)
        {
            path.Fill = Brushes.Black;
        }
        private void QuadrantLeave(Path path)
        {
            path.Fill = Brushes.Transparent;
        }

        private void SmallMiddleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ClickWindow.HideWindow();
        }
    }
}
