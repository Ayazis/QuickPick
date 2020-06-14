using GlobalHOoks.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput;

namespace GlobalHOoks
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ClickWindow : Window
    {
        public QuickPick QP { get; }

        public ClickWindow(QuickPick QP)
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
