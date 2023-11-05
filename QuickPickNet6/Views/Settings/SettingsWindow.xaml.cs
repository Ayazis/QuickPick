using MouseAndKeyBoardHooks;
using System;
using System.Windows;
using System.Windows.Input;
using Utilities.Mouse_and_Keyboard;

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
			Left = mousePosition.X - Width / 2;
			Top = mousePosition.Y - Height / 2;
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

        private void btnKeyCombo_Click(object sender, RoutedEventArgs e)
        {
            if(KeyInputHandler._isRecordingNewCombo)
            {
               var newKeys =  KeyInputHandler.Instance.ApplyNewRecording();
            }
            else
            {
                KeyInputHandler.Instance.StartRecordingNewCombo();
            }
        }
    }
}
