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
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            var hexPositions = _hexPositionsCalculator.GenerateHexagonalGridFixed(numberOfHexes);

            canvas?.Children.Clear();
            foreach (var point in hexPositions)
            {
                double x = hexagonSize / 1.75 * (3.0 / 2 * point.Q);
                double y = hexagonSize / 1.75 * (Math.Sqrt(3) * (point.R + 0.5 * point.Q));
                var hexagon = new HexagonButton() { Width = hexagonSize, Height = hexagonSize };
                //hexagon.Fill = Brushes.LightGray;
                //hexagon.Stroke = Brushes.Black;
                canvas.Children.Add(hexagon);
                Canvas.SetLeft(hexagon, x);
                Canvas.SetTop(hexagon, y);
            }
        }
    }
}