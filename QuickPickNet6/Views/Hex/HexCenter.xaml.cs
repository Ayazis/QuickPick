using System.Windows.Controls;
using HexTest;
using System.Collections.Generic;
using QuickPick.Logic;
using QuickPick.UI.BrightnessControls;
using QuickPick.UI.Views.Settings;
using System;
using System.Windows.Input;
using System.Windows;
using FontAwesome5;
using System.Linq;

namespace QuickPick.UI.Views.Hex;
/// <summary>
/// Interaction logic for HexCenter.xaml
/// </summary>
public partial class HexCenter : UserControl
{
    private int _size = 40;
    private int _hexCount = 1 + 6;
    HexGridCreator _hexGridCreator = new(new HexPositionsCalculator());
    List<HexPosition> _hexes;
    public HexCenter()
    {
        InitializeComponent();
        HexCanvas.Children.Clear();

        _hexes = _hexGridCreator.CreateHexButtonsInHoneyCombStructure(HexCanvas.Width, _size, _hexCount);


        foreach (var hexPosition in _hexes)
        {
            HexCanvas.Children.Add(hexPosition.HexButton);
            Canvas.SetLeft(hexPosition.HexButton, hexPosition.Position.X);
            Canvas.SetTop(hexPosition.HexButton, hexPosition.Position.Y);
        }
        SetCustomHexButtons();

    }

    BrightnessControl _brightnessControl;
    bool _brightnessButtonDown;
    Point _previousPosition;
    double _percentage;
    public void SetCustomHexButtons()
    {
        _hexes[0].HexButton.AsSettingsButton();
        _hexes[1].HexButton.AsConnectToBluetoothHeadset();
        _hexes[2].HexButton.AsNextSong();
        _hexes[3].HexButton.AsPlayPauseToggle();
        _hexes[4].HexButton.AsPreviousSong();
        _hexes[5].HexButton.AsVolumeControl();        
        _hexes[6].HexButton.AsBrightnessControl();
    }


    private void Hex1_Click(object sender, RoutedEventArgs e)
    {

    }
    //private void BrightnessButton_MouseDown(object sender, MouseButtonEventArgs e)
    //{
    //    _brightnessButtonDown = true;
    //    BrightnessBar.Visibility = Visibility.Visible;
    //    _previousPosition = e.GetPosition(this);
    //    // Capture the mouse
    //    ((dynamic)sender).CaptureMouse();
    //}

    //private void BrightnessButton_MouseUp(object sender, MouseButtonEventArgs e)
    //{
    //    BrightnessBar.Visibility = Visibility.Collapsed;
    //    _brightnessButtonDown = false;
    //    // Release the mouse
    //    ((dynamic)sender).ReleaseMouseCapture();
    //}

    //private void BrightnessButton_MouseMove(object sender, MouseEventArgs e)
    //{

    //    if (_brightnessButtonDown)
    //    {
    //        // get current mousePosition
    //        Point position = e.GetPosition(this);
    //        // compare with previousposition, calculate vertical distance:
    //        var pointDifference = -(position.Y - _previousPosition.Y);
    //        _percentage += pointDifference;

    //        if (_percentage > 100)
    //            _percentage = 100;
    //        if (_percentage < 50)
    //            _percentage = 50;

    //        BrightnessBar.Value = _percentage;
    //        ExposeNewBrightnessLevel();
    //        _previousPosition = position;
    //    }
    //}

    public delegate void IntValueChangedEventHandler(double value);
    public event IntValueChangedEventHandler BrightnessLevelChanged;

    public void ExposeNewBrightnessLevel()
    {
        _brightnessControl.HandleNewValue((int)_percentage);
    }
    private void Hex2_Click(object sender, RoutedEventArgs e)
    {
        ToggleVolumeAndVolumeOffButtons(sender as HexagonButton);
    }

    private void Hex3_Click(object sender, RoutedEventArgs e)
    {
        ToggleMute(sender as HexagonButton);
    }
    void ToggleMute(HexagonButton button)
    {
        InputSim.ToggleMute();

        if (button.FontIcon == EFontAwesomeIcon.Solid_VolumeUp)
            button.FontIcon = FontAwesome5.EFontAwesomeIcon.Solid_VolumeMute;
        else
            button.FontIcon = FontAwesome5.EFontAwesomeIcon.Solid_VolumeUp;
    }

    private void ToggleVolumeAndVolumeOffButtons(HexagonButton musicButton)
    {
        InputSim.PlayPause();

        if (musicButton.FontIcon == FontAwesome5.EFontAwesomeIcon.Solid_Play)
            musicButton.FontIcon = FontAwesome5.EFontAwesomeIcon.Solid_Pause;
        else
            musicButton.FontIcon = FontAwesome5.EFontAwesomeIcon.Solid_Play;
    }



}
