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
using QuickPick.UI.BrightnessControls;
using QuickPick.Logic;

namespace QuickPick.UI.Views.Hex;
/// <summary>
/// Interaction logic for HexCenter.xaml
/// </summary>
public partial class HexCenter : UserControl
{
    BrightnessControl _brightnessControl = new();
    bool _brightnessButtonDown;
    Point _previousPosition;
    double _percentage;
    public HexCenter()
    {
        InitializeComponent();
    }

    private void CenterHex_Click(object sender, RoutedEventArgs e)
    {
        SettingsWindow.Instance.ShowWindow();
        SettingsWindow.Instance.Activate();
        SettingsWindow.Instance.Focus();
        ClickWindow.Instance.HideUI();
    }

    private void Hex1_Click(object sender, RoutedEventArgs e)
    {

    }
    private void BrightnessButton_MouseDown(object sender, MouseButtonEventArgs e)
    {
        _brightnessButtonDown = true;
        BrightnessBar.Visibility = Visibility.Visible;
        _previousPosition = e.GetPosition(this);
        // Capture the mouse
        ((dynamic)sender).CaptureMouse();
    }

    private void BrightnessButton_MouseUp(object sender, MouseButtonEventArgs e)
    {
        BrightnessBar.Visibility = Visibility.Collapsed;
        _brightnessButtonDown = false;
        // Release the mouse
        ((dynamic)sender).ReleaseMouseCapture();
    }

    private void BrightnessButton_MouseMove(object sender, MouseEventArgs e)
    {

        if (_brightnessButtonDown)
        {
            // get current mousePosition
            Point position = e.GetPosition(this);
            // compare with previousposition, calculate vertical distance:
            var pointDifference = -(position.Y - _previousPosition.Y);
            _percentage += pointDifference;

            if (_percentage > 100)
                _percentage = 100;
            if (_percentage < 50)
                _percentage = 50;

            BrightnessBar.Value = _percentage;
            ExposeNewBrightnessLevel();
            _previousPosition = position;
        }
    }

    public delegate void IntValueChangedEventHandler(double value);
    public event IntValueChangedEventHandler BrightnessLevelChanged;

    public void ExposeNewBrightnessLevel()
    {
        _brightnessControl.SetBrightnessOnAllScreens((int)_percentage);
    }
    private void Hex2_Click(object sender, RoutedEventArgs e)
    {
        ToggleVolumeAndVolumeOffButtons(sender as HexagonButton);
    }

    private void Hex3_Click(object sender, RoutedEventArgs e)
    {
        ToggleMute(sender as HexagonButton);
    }
    void ToggleMute(HexagonButton muteButton)
    {
        InputSim.ToggleMute();

        if (muteButton.FontIcon == FontAwesome5.EFontAwesomeIcon.Solid_VolumeUp)
            muteButton.FontIcon = FontAwesome5.EFontAwesomeIcon.Solid_VolumeMute;
        else
            muteButton.FontIcon = FontAwesome5.EFontAwesomeIcon.Solid_VolumeUp;
    }

    private void ToggleVolumeAndVolumeOffButtons(HexagonButton musicButton)
    {
        InputSim.PlayPause();

        if (musicButton.FontIcon == FontAwesome5.EFontAwesomeIcon.Solid_Play)
            musicButton.FontIcon = FontAwesome5.EFontAwesomeIcon.Solid_Pause;
        else
            musicButton.FontIcon = FontAwesome5.EFontAwesomeIcon.Solid_Play;
    }

    private void Hex4_Click(object sender, RoutedEventArgs e)
    {
        InputSim.WinD();
        ClickWindow.Instance.HideUI();
    }
}
