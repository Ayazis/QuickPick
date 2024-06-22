using MouseAndKeyBoardHooks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace QuickPick.UI.Views.Settings
{
    public interface ISettingsWindow
    {
        void InitializeComponent();
        void ShowWindow(bool showNextToUI);
    }

    /// <summary>
    /// Interaction logic for SettingsWindow4.xaml
    /// </summary>
    public partial class SettingsWindow : Window, ISettingsWindow
    {
        public static SettingsWindow Instance;
        readonly ISettingsSaver _settingsSaver;
        private SettingsViewModel _viewModel;

        public SettingsWindow(ISettingsSaver settingsSaver, SettingsViewModel viewModel)
        {
            _settingsSaver = settingsSaver;
            InitializeComponent();
            _viewModel = viewModel;
            this.DataContext = viewModel;
            this.MouseLeftButtonDown += SettingsWindow_MouseLeftButtonDown;
            Instance = this;
        }



        public void ShowWindow(bool showNextToUI = true)
        {
            const int uiWidth = 250; // matches width of QuickPickUI, must be refactored to fetch width.
            double halfWidth = Width / 2;
            double halfHeight = Height / 2;

            if (showNextToUI)
            {
                WindowStartupLocation = WindowStartupLocation.Manual;
                var mousePosition = MousePosition.GetCursorPosition();
                Left = mousePosition.X - halfWidth + uiWidth;
                Top = mousePosition.Y - halfHeight;
            }
            else
            {
                // Calculate center of the current screen
                var screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);
                Left = screen.Bounds.Left + (screen.Bounds.Width - Width) / 2;
                Top = screen.Bounds.Top + (screen.Bounds.Height - Height) / 2;
            }

            this.WindowState = WindowState.Normal; // In case the settingswindow is already shown but minimized. This will force the window to appear again.
            Show();          
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            _settingsSaver.ApplySettings();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btnApplyNewCombo_Click(object sender, RoutedEventArgs e)
        {
            Instance._viewModel.CurrentKeyCombo = _viewModel.NewKeyCombo.ToLower();
            tbNewCombo.Visibility = Visibility.Collapsed;
            btnApplyNewCombo.Visibility = Visibility.Collapsed;
            btnCancelNewCombo.Visibility = Visibility.Collapsed;
            btnRecordNewCombo.Visibility = Visibility.Visible;
            this.KeyDown -= SettingsWindow_KeyDown;
            this.MouseDown -= SettingsWindow_MouseDown;
        }

        private void btnCancelNewCombo_Click(object sender, RoutedEventArgs e)
        {
            Instance._viewModel.NewKeyCombo = string.Empty;
            tbNewCombo.Visibility = Visibility.Collapsed;
            btnApplyNewCombo.Visibility = Visibility.Collapsed;
            btnCancelNewCombo.Visibility = Visibility.Collapsed;
            btnRecordNewCombo.Visibility = Visibility.Visible;
            this.KeyDown -= SettingsWindow_KeyDown;
            this.MouseDown -= SettingsWindow_MouseDown;
        }

        private void btnRecordNewCombo_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ClearNewKeyCombo();
            this.KeyDown += SettingsWindow_KeyDown;
            this.MouseDown += SettingsWindow_MouseDown;
            tbNewCombo.Visibility = Visibility.Visible;
            btnApplyNewCombo.Visibility = Visibility.Visible;
            btnCancelNewCombo.Visibility = Visibility.Visible;
            btnRecordNewCombo.Visibility = Visibility.Collapsed;
        }

        private void SettingsWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SettingsWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            System.Windows.Forms.Keys formsKey = (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(key);

            if (key == Key.System)
                formsKey = System.Windows.Forms.Keys.LMenu;
            // todo:
            // Key system translates to Keys none
            _viewModel.AddKeyToNewCombo(formsKey);
            tbNewCombo.Text = _viewModel.NewKeyCombo;
        }
        private void SettingsWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseButton mouseButton = e.ChangedButton;
            System.Windows.Forms.Keys formsKey = System.Windows.Forms.Keys.None;

            switch (mouseButton)
            {
                // left mouse is not allowed.

                //case MouseButton.Left:
                //    formsKey = System.Windows.Forms.Keys.LButton; 
                //    break;
                case MouseButton.Right:
                    formsKey = System.Windows.Forms.Keys.RButton;
                    break;
                case MouseButton.Middle:
                    formsKey = System.Windows.Forms.Keys.MButton;
                    break;
                case MouseButton.XButton1:
                    formsKey = System.Windows.Forms.Keys.XButton1;
                    break;
                case MouseButton.XButton2:
                    formsKey = System.Windows.Forms.Keys.XButton2;
                    break;
            }

            _viewModel.AddKeyToNewCombo(formsKey);
            //tbNewCombo.Text = ViewModel.NewKeyCombo;
            // Use formsKey as needed
        }

    }
}
