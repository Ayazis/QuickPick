using QuickPick.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput;

namespace QuickPick
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ClickWindow : Window
    {
        public Models.QuickPick QP { get; }

        public ClickWindow(Models.QuickPick QP)
        {
            this.QP = QP;
            this.DataContext = QP.QuickPickModel;
            InitializeComponent();
        }
        public void btnShowShortCuts_Click(object sender, RoutedEventArgs e)
        {
            QP.WindowManager.ShowShortCuts();
        }
    }
}
