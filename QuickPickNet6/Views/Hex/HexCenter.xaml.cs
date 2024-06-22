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


    public void SetCustomHexButtons()
    {
        _hexes[0].HexButton.AsSettingsButton();
        _hexes[1].HexButton.AsShowDesktopButton(); 
        _hexes[2].HexButton.AsNextSong();
        _hexes[3].HexButton.AsPlayPauseToggle();
        _hexes[4].HexButton.AsPreviousSong();
        _hexes[5].HexButton.AsVolumeControl();
        _hexes[6].HexButton.AsBrightnessControl();        
    }  
}
