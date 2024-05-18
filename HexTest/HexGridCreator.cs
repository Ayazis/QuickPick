using QuickPick.UI.Views.Hex;
using System.Windows;
using System.Windows.Controls;


namespace HexTest
{
    public class HexPosition
    {
        public HexagonButton HexButton { get; set; }
        public Point Position { get; set; }
    }

    public class HexGridCreator
    {
        IHexPositionsCalculator _hexPositionsCalculator;

        public HexGridCreator(IHexPositionsCalculator hexPositionsCalculator)
        {
            _hexPositionsCalculator = hexPositionsCalculator;
        }

        public IEnumerable<HexPosition> CreateHexButtonsInHoneyCombStructure(double canvasWidth, int hexagonSize, int numberOfHexes)
        {

            double xOffSet = (canvasWidth / 2) - (hexagonSize / 2);
            double yOffset = (canvasWidth / 2) - (hexagonSize / 2);

            // todo: Add offset the size of the button

            var hexPositions = _hexPositionsCalculator.GenerateHexagonalGridFixed(numberOfHexes);

            foreach (var point in hexPositions)
            {
                double x = hexagonSize / 1.75 * (3.0 / 2 * point.Column);
                double y = hexagonSize / 1.75 * (Math.Sqrt(3) * (point.Row + 0.5 * point.Column));

                // add offset so that the grid is centered.
                x += xOffSet;
                y += yOffset;

                var hexbutton = new HexagonButton() { Width = hexagonSize, Height = hexagonSize };
#if DEBUG
                //string nr = hexPositions.IndexOf(point).ToString();
                //hexagon.Grid.Children.Add(new TextBlock()
                //{
                //    Text = nr,
                //    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                //    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                //    Foreground = Brushes.White,
                //    Margin = new System.Windows.Thickness(15, 15, 0, 0)
                //});
#endif

                yield return new HexPosition() { HexButton = hexbutton, Position = new(x, y) };
            }
        }
    }
}