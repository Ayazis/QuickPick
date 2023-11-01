using MouseAndKeyBoardHooks;
using System;
using System.Windows;
using System.Windows.Input;

namespace QuickPick.UI.Views.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindow4.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
		static SettingsWindow _instance;
		public static SettingsWindow Instance => _instance ??= new SettingsWindow();
		public SettingsViewModel ViewModel { get; private set; }

		private SettingsWindow()
        {
            ViewModel = new SettingsViewModel();
            this.DataContext = ViewModel;
            InitializeComponent();
            this.MouseLeftButtonDown += SettingsWindow_MouseDown;
        }

        private void SettingsWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public void ShowWindow()
        {
			var mousePosition = MousePosition.GetCursorPosition();
			Left = mousePosition.X - ActualWidth / 2;
			Top = mousePosition.Y - ActualHeight / 2;
			// get mouseposition.

			Show();
        }

        public event EventHandler ApplySettings;
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            ApplySettings?.Invoke(this, EventArgs.Empty);
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
