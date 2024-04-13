using System.Windows;
using System.Windows.Controls;

namespace QuickPick.UI.Views.HexGrid
{
    public class HexGrid : Panel
    {
        // MeasureOverride is called during the measure pass of the layout process.
        // This is where each child element is measured.
        protected override Size MeasureOverride(Size availableSize)
        {
            // Measure each child element.
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            // Return the size that this panel requires.
            return base.MeasureOverride(availableSize);
        }

        // ArrangeOverride is called during the arrange pass of the layout process.
        // This is where each child element is arranged according to a specific layout.
        protected override Size ArrangeOverride(Size finalSize)
        {
            // The width and height of a hexagon.
            double hexagonWidth = 0;
            double hexagonHeight = 0;

            // If there are any child elements, get the width and height of the first one.
            if (Children.Count > 0)
            {
                hexagonWidth = Children[0].DesiredSize.Width;
                hexagonHeight = Children[0].DesiredSize.Height;
            }

            // The width of a column is 75% of the hexagon's width.
            // The height of a row is 50% of the hexagon's height.
            double columnWidth = hexagonWidth * 0.75;
            double rowHeight = hexagonHeight * 0.5;

            const int maxColumns = 3;   
            // Arrange each child element.
            for (int i = 0; i < Children.Count; i++)
            {
                // Calculate the row and column indices of the current child element.
                int row = i / maxColumns;
                int column = i % maxColumns;

                // Calculate the x and y coordinates of the current child element.
                double x = column * columnWidth;
                double y = row * rowHeight;

                // If the column index is odd, shift the y coordinate down by half the height of a row.
                if (column % 2 == 1)
                {
                    y += rowHeight;
                }

                // Arrange the current child element.
                Children[i].Arrange(new Rect(new Point(x, y), Children[i].DesiredSize));
            }

            // Return the final size of this panel.
            return finalSize;
        }
    }

}
