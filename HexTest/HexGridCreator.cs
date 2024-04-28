using QuickPick.UI.Views.Hex;
using System.Windows.Controls;

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
            double xOffSet = canvas.Width / 2;
            double yOffset = canvas.Height / 2;

            // todo: Add offset the size of the button

            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            var hexPositions = _hexPositionsCalculator.GenerateHexagonalGridFixed(numberOfHexes);

            canvas?.Children.Clear();
            foreach (var point in hexPositions)
            {
                double x = hexagonSize / 1.75 * (3.0 / 2 * point.Q);
                double y = hexagonSize / 1.75 * (Math.Sqrt(3) * (point.R + 0.5 * point.Q));

                // add offset so that the grid is centered.
                x += xOffSet;
                y += yOffset;

                var hexagon = new HexagonButton() { Width = hexagonSize, Height = hexagonSize };
                canvas.Children.Add(hexagon);
                Canvas.SetLeft(hexagon, x);
                Canvas.SetTop(hexagon, y);
            }
        }
    }
}