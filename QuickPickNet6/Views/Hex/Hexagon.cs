using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace QuickPick.UI.Views.Hex
{
	public class Hexagon : Shape
	{
		public enum Orientation
		{
			Horizontal,
			Vertical
		}

		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
			"Orientation", typeof(Orientation), typeof(Hexagon), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender));

		public Orientation HexagonOrientation
		{
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		protected override Geometry DefiningGeometry
		{
			get
			{
				return new PathGeometry
				{
					Figures =
					{
						HexagonOrientation == Orientation.Horizontal ? CreateHorizontalHexagon() : CreateVerticalHexagon()
					}
				};
			}
		}

		private PathFigure CreateVerticalHexagon()
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
		private PathFigure CreateHorizontalHexagon()
		{
			double radius = Width / 2;
			double centerX = Width / 2;
			double centerY = Height / 2;
			return new PathFigure
			{
				StartPoint = new Point(centerX + radius * Math.Cos(0), centerY + radius * Math.Sin(0)),
				Segments =
	{
		new LineSegment {Point = new Point(centerX + radius * Math.Cos(Math.PI / 3), centerY + radius * Math.Sin(Math.PI / 3))},
		new LineSegment {Point = new Point(centerX + radius * Math.Cos(2 * Math.PI / 3), centerY + radius * Math.Sin(2 * Math.PI / 3))},
		new LineSegment {Point = new Point(centerX + radius * Math.Cos(Math.PI), centerY + radius * Math.Sin(Math.PI))},
		new LineSegment {Point = new Point(centerX + radius * Math.Cos(4 * Math.PI / 3), centerY + radius * Math.Sin(4 * Math.PI / 3))},
		new LineSegment {Point = new Point(centerX + radius * Math.Cos(5 * Math.PI / 3), centerY + radius * Math.Sin(5 * Math.PI / 3))},
	}
			};
		}

	}


}


