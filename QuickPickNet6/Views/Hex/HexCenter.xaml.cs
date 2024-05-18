using System.Windows.Controls;
using HexTest;
using System.Collections.Generic;

namespace QuickPick.UI.Views.Hex;
/// <summary>
/// Interaction logic for HexCenter.xaml
/// </summary>
public partial class HexCenter : UserControl
{
    private int _size = 35;
    private int _hexCount = 1 + 6;
    HexGridCreator _hexGridCreator = new(new HexPositionsCalculator());
    public HexCenter()
    {
        InitializeComponent();
        HexCanvas.Children.Clear();

        IEnumerable<HexPosition> hexes = _hexGridCreator.CreateHexButtonsInHoneyCombStructure(HexCanvas.Width, _size, _hexCount);
        foreach (var hexPosition in hexes)
        {
            HexCanvas.Children.Add(hexPosition.HexButton);
            Canvas.SetLeft(hexPosition.HexButton, hexPosition.Position.X);
            Canvas.SetTop(hexPosition.HexButton, hexPosition.Position.Y);
        }
    }
}
