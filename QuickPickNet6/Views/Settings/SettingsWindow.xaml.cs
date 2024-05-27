using MouseAndKeyBoardHooks;
using System.Windows;
using System.Windows.Input;

namespace QuickPick.UI.Views.Settings
{
    public interface ISettingsWindow
    {
        SettingsViewModel ViewModel { get; }

        void InitializeComponent();
        void ShowWindow();
    }

    /// <summary>
    /// Interaction logic for SettingsWindow4.xaml
    /// </summary>
    public partial class SettingsWindow : Window, ISettingsWindow
    {       
        public static SettingsWindow Instance;
        readonly ISettingsManager _settingsManager;
        public SettingsViewModel ViewModel { get; private set; } = new();

        public SettingsWindow(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            InitializeComponent();           
            this.DataContext = ViewModel;
            this.MouseLeftButtonDown += SettingsWindow_MouseLeftButtonDown;
            Instance = this;
        }



        public void ShowWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            var mousePosition = MousePosition.GetCursorPosition();
            Left = mousePosition.X - Width / 2;
            Top = mousePosition.Y - Height / 2;
            // get mouseposition.

            Show();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {

            _settingsManager.ApplySettings(ViewModel);

            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btnApplyNewCombo_Click(object sender, RoutedEventArgs e)
        {
            Instance.ViewModel.CurrentKeyCombo = ViewModel.NewKeyCombo.ToLower();
            tbNewCombo.Visibility = Visibility.Collapsed;
            btnApplyNewCombo.Visibility = Visibility.Collapsed;
            btnCancelNewCombo.Visibility = Visibility.Collapsed;
            btnRecordNewCombo.Visibility = Visibility.Visible;
            this.KeyDown -= SettingsWindow_KeyDown;
            this.MouseDown -= SettingsWindow_MouseDown;
        }

        private void btnCancelNewCombo_Click(object sender, RoutedEventArgs e)
        {
            Instance.ViewModel.NewKeyCombo = string.Empty;
            tbNewCombo.Visibility = Visibility.Collapsed;
            btnApplyNewCombo.Visibility = Visibility.Collapsed;
            btnCancelNewCombo.Visibility = Visibility.Collapsed;
            btnRecordNewCombo.Visibility = Visibility.Visible;
            this.KeyDown -= SettingsWindow_KeyDown;
            this.MouseDown -= SettingsWindow_MouseDown;
        }

        private void btnRecordNewCombo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearNewKeyCombo();
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
            ViewModel.AddKeyToNewCombo(formsKey);
            tbNewCombo.Text = ViewModel.NewKeyCombo;
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

            ViewModel.AddKeyToNewCombo(formsKey);
            //tbNewCombo.Text = ViewModel.NewKeyCombo;
            // Use formsKey as needed
        }

    }
}
