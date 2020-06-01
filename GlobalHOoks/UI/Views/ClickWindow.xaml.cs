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
        private WindowManager _windowManager;
        private QuickPickModel _qpm;
        internal ButtonManager _buttonManager;
        private static InputSimulator _sim = new InputSimulator();

        public ClickWindow(QuickPickModel model, WindowManager manager)
        {
            _windowManager = manager;
            _qpm = model;
          
        }    

        internal void Initialize(ButtonManager buttonManager)
        {
            _buttonManager = buttonManager;
            InitializeComponent();
            _buttonManager.AddButtons();
            _buttonManager.AddShortCuts();
        }
        internal void btnShowShortCuts_Click(object sender, RoutedEventArgs e)
        {
            _windowManager.ShowShortCuts();
        }
    }
}
