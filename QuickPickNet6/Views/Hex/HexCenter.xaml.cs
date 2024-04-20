using QuickPick.UI.Views.Settings;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickPick.UI.Views.Hex;
/// <summary>
/// Interaction logic for HexCenter.xaml
/// </summary>
public partial class HexCenter : UserControl
{
	public HexCenter()
	{
		InitializeComponent();
	}

    private void HexCenter_Click(object sender, RoutedEventArgs e)
    {
        SettingsWindow.Instance.ShowWindow();
        SettingsWindow.Instance.Activate();
        SettingsWindow.Instance.Focus();
        ClickWindow.Instance.HideUI();
    }

    private void Hex1_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Hex2_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Hex3_Click(object sender, RoutedEventArgs e)
    {

    }
}
