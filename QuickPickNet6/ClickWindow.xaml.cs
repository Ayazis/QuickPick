using System.Windows;
namespace QuickPick;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window
{
    private static QuickPickMainWindowModel _qpm;

    public ClickWindow()
    {
        InitializeComponent();
        DataContext = _qpm;
    }
    public ClickWindow(QuickPickMainWindowModel qpm)
	{
		 _qpm = qpm;
        InitializeComponent();
    }

    private void btnCenterClick(object sender, RoutedEventArgs e)
    {
        
        
    }
}
