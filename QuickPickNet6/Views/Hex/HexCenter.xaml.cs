using System.Windows.Controls;
using HexTest;

namespace QuickPick.UI.Views.Hex;
/// <summary>
/// Interaction logic for HexCenter.xaml
/// </summary>
public partial class HexCenter : UserControl
{
    HexGridCreator _hexGridCreator = new(new HexPositionsCalculator());
    public HexCenter()
    {
        InitializeComponent();

        _hexGridCreator.DrawHexagonalGrid(HexCanvas, 30, 7);

    }







}
