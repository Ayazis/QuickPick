using System.Windows.Controls;
using HexTest;

namespace QuickPick.UI.Views.Hex;
/// <summary>
/// Interaction logic for HexCenter.xaml
/// </summary>
public partial class HexCenter : UserControl
{
    private int _size = 35;
    private int _hexCount =48;
    HexGridCreator _hexGridCreator = new(new HexPositionsCalculator());
    public HexCenter()
    {
        InitializeComponent();

        _hexGridCreator.DrawHexagonalGrid(HexCanvas, _size, _hexCount);

    }
}
