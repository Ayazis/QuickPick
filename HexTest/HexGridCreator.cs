using QuickPick.UI.Views.Hex;
using System.Windows.Controls;
using System.Windows.Media;

namespace HexTest
{
    public class HexGridCreator
    {
        IHexPositionsCalculator _hexPositionsCalculator;

        public HexGridCreator(IHexPositionsCalculator hexPositionsCalculator)
        {
            _hexPositionsCalculator = hexPositionsCalculator;
        }

        public void DrawHexagonalGrid(Canvas canvas, int hexagonSize, int numberOfHexes)
        {
            double xOffSet = (canvas.Width / 2) - (hexagonSize / 2);
            double yOffset = (canvas.Height / 2) - (hexagonSize / 2);

            // todo: Add offset the size of the button

            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            var hexPositions = _hexPositionsCalculator.GenerateHexagonalGridFixed(numberOfHexes);

            canvas.Children.Clear();
            foreach (var point in hexPositions)
            {
                double x = hexagonSize / 1.75 * (3.0 / 2 * point.Column);
                double y = hexagonSize / 1.75 * (Math.Sqrt(3) * (point.Row + 0.5 * point.Column));

                // add offset so that the grid is centered.
                x += xOffSet;
                y += yOffset;

                var hexagon = new HexagonButton() { Width = hexagonSize, Height = hexagonSize };
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

                canvas.Children.Add(hexagon);
                Canvas.SetLeft(hexagon, x);
                Canvas.SetTop(hexagon, y);
            }
        }
    }
}