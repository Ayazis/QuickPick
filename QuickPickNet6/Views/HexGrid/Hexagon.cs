using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace QuickPick.UI.Views.HexGrid;
internal class Hexagon : Shape
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    protected override Geometry DefiningGeometry
    {
        get
        {
            PathFigure figure = CreateHexagonShape();
            return new PathGeometry { Figures = { figure } };
        }
    }

    private PathFigure CreateHexagonShape()
    {
        return HexagonOrientation == Orientation.Horizontal ? CreateHorizontalHexagon() : CreateVerticalHexagon();
    }

    private PathFigure CreateHorizontalHexagon()
    {
        double radius = Width / 2;
        double centerY = Height / 2;
        return new PathFigure
        {
            StartPoint = new Point(radius * Math.Cos(Math.PI / 6) + radius, centerY + radius * Math.Sin(Math.PI / 6)),
            Segments =
        {
            new LineSegment {Point = new Point(radius * Math.Cos(Math.PI / 2) + radius, centerY + radius * Math.Sin(Math.PI / 2))},
            new LineSegment {Point = new Point(radius * Math.Cos(5 * Math.PI / 6) + radius, centerY + radius * Math.Sin(5 * Math.PI / 6))},
            new LineSegment {Point = new Point(radius * Math.Cos(7 * Math.PI / 6) + radius, centerY + radius * Math.Sin(7 * Math.PI / 6))},
            new LineSegment {Point = new Point(radius * Math.Cos(3 * Math.PI / 2) + radius, centerY + radius * Math.Sin(3 * Math.PI / 2))},
            new LineSegment {Point = new Point(radius * Math.Cos(11 * Math.PI / 6) + radius, centerY + radius * Math.Sin(11 * Math.PI / 6))},
        }
        };
    }

    private PathFigure CreateVerticalHexagon()
    {
        double radius = Height / 2;
        double centerX = Width / 2;
        return new PathFigure
        {
            StartPoint = new Point(centerX + radius * Math.Cos(0), radius * Math.Sin(0) + radius),
            Segments =
        {
            new LineSegment {Point = new Point(centerX + radius * Math.Cos(2 * Math.PI / 3), radius * Math.Sin(2 * Math.PI / 3) + radius)},
            new LineSegment {Point = new Point(centerX + radius * Math.Cos(4 * Math.PI / 3), radius * Math.Sin(4 * Math.PI / 3) + radius)},
            new LineSegment {Point = new Point(centerX + radius * Math.Cos(2 * Math.PI), radius * Math.Sin(2 * Math.PI) + radius)},
            new LineSegment {Point = new Point(centerX + radius * Math.Cos(4 * Math.PI / 3), radius * Math.Sin(4 * Math.PI / 3) + radius)},
            new LineSegment {Point = new Point(centerX + radius * Math.Cos(2 * Math.PI / 3), radius * Math.Sin(2 * Math.PI / 3) + radius)},
        }
        };
    }





    // Dependency Property to allow the orientation of the hexagon to be set in xaml.
    public Orientation HexagonOrientation
    {
        get { return (Orientation)GetValue(OrientationProperty); }
        set { SetValue(OrientationProperty, value); }
    }
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        "Orientation", typeof(Orientation), typeof(Hexagon), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender));
    
    /// <summary>
    /// Makes sure that the hexagon is always square, resulting in a perfect hexagon
    /// </summary>
    /// <param name="availableSize"></param>
    /// <returns></returns>
    protected override Size MeasureOverride(Size availableSize)
    {
        double min = Math.Min(availableSize.Width, availableSize.Height);
        return new Size(min, min);
    }


}
