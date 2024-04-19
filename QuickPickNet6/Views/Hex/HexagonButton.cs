using FontAwesome5;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuickPick.UI.Views.Hex;

public class HexagonButton : Button
{
    Hexagon _hexagon;
    ImageAwesome _icon;

    double hexScale = .9;
    double iconScale = .3;
    public HexagonButton()
    {
        this.BorderThickness = new System.Windows.Thickness(0);
        Padding = new System.Windows.Thickness(0);
        Background = Brushes.Transparent;

        // Create a Grid to hold the Hexagon and the icon
        Grid grid = new Grid();

        // Create the Hexagon shape
        _hexagon = new Hexagon()
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
        };

        // Add the Hexagon to the Grid
        grid.Children.Add(_hexagon);

        // Create the FontAwesome icon
        _icon = new ImageAwesome
        {
            Icon = EFontAwesomeIcon.Solid_InfoCircle,
            Foreground = Brushes.White,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            Width = this.Width * iconScale,
        };

        // Add the icon to the Grid
        grid.Children.Add(_icon);

        // Set the Content of the Button to the Grid
        this.Content = grid;

        SizeChanged += HexagonButton_SizeChanged; ;
    }

    private void HexagonButton_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
    {
        // Set the Width and Height of the Hexagon to be the same as the ActualWidth and ActualHeight of the HexagonButton
        _hexagon.Width = this.ActualWidth * hexScale;
        _hexagon.Height = this.ActualHeight * hexScale;

        _icon.Width = this.ActualWidth * iconScale;
    }
}