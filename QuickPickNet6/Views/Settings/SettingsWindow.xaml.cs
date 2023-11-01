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
using System.Windows.Shapes;

namespace QuickPick.UI.Views.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsViewModel ViewModel;
        public SettingsWindow(SettingsViewModel viewModel)
        {
            ViewModel = viewModel;
            this.DataContext = ViewModel;
            InitializeComponent();
            this.MouseLeftButtonDown += SettingsWindow_MouseDown;
        }

        private void SettingsWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public event EventHandler ApplySettings;
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            ApplySettings?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
