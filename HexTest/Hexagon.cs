using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace QuickPick.UI.Views.Hex
{
    public class Hexagon : Shape
    {
        private static LinearGradientBrush _defaultGradient;
        private static LinearGradientBrush _reversedGradient;

        static Hexagon()
        {
            CreateBrushes();
        }


        const string BorderBrush = "#6F6F6F";
        Color BorderColor = (Color)ColorConverter.ConvertFromString(BorderBrush);

        public Hexagon()
        {
            // Create a new trigger
            Trigger mouseOverTrigger = new Trigger { Property = UIElement.IsMouseOverProperty, Value = true };
            // Create setter for the trigger
            Setter backgroundSetter = new Setter { Property = Shape.FillProperty, Value = _reversedGradient };
            // Add the setter to the trigger
            mouseOverTrigger.Setters.Add(backgroundSetter);


            // Set the style directly in the constructor
            Style = new Style(typeof(Hexagon))
            {
                Setters =
                  {
                      new Setter(Shape.FillProperty, _defaultGradient ),
                      new Setter(Shape.StrokeProperty, new SolidColorBrush(BorderColor)),
                      new Setter(Shape.StrokeThicknessProperty, 1.0)
                  },
                Triggers =
                {
                    mouseOverTrigger
                }
            };




        }
        public enum Orientation
        {
            Horizontal,
            Vertical
        }
        static void CreateBrushes()
        {
            _defaultGradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops = new GradientStopCollection
                          {
                              new GradientStop(Color.FromRgb(48, 48, 48), 1),
                              new GradientStop(Color.FromRgb(67, 67, 67), 0.3)
                          }
            };

            _reversedGradient = new LinearGradientBrush
            {
                StartPoint = new Point(1, 1),
                EndPoint = new Point(0, 0),
                GradientStops = new GradientStopCollection
                          {
                              new GradientStop(Color.FromRgb(48, 48, 48), 1),
                              new GradientStop(Color.FromRgb(67, 67, 67), 0.3)
                          }
            };



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
            var startPoint = new Point(radius * Math.Cos(Math.PI / 6) + radius, centerY + radius * Math.Sin(Math.PI / 6));
            return new PathFigure
            {
                StartPoint = startPoint,
                Segments =
            {
                new LineSegment {Point = new Point(radius * Math.Cos(Math.PI / 2) + radius, centerY + radius * Math.Sin(Math.PI / 2))},
                new LineSegment {Point = new Point(radius * Math.Cos(5 * Math.PI / 6) + radius, centerY + radius * Math.Sin(5 * Math.PI / 6))},
                new LineSegment {Point = new Point(radius * Math.Cos(7 * Math.PI / 6) + radius, centerY + radius * Math.Sin(7 * Math.PI / 6))},
                new LineSegment {Point = new Point(radius * Math.Cos(3 * Math.PI / 2) + radius, centerY + radius * Math.Sin(3 * Math.PI / 2))},
                new LineSegment {Point = new Point(radius * Math.Cos(11 * Math.PI / 6) + radius, centerY + radius * Math.Sin(11 * Math.PI / 6))},
                new LineSegment {Point = startPoint} // Add this line to close the hexagon.
			}
            };
        }
        private PathFigure CreateHorizontalHexagon()
        {
            double radius = Width / 2;
            double centerX = Width / 2;
            double centerY = Height / 2;

            var startPoint = new Point(centerX + radius * Math.Cos(0), centerY + radius * Math.Sin(0));
            return new PathFigure
            {
                StartPoint = startPoint,
                Segments =
                {
                    new LineSegment {Point = new Point(centerX + radius * Math.Cos(Math.PI / 3), centerY + radius * Math.Sin(Math.PI / 3))},
                    new LineSegment {Point = new Point(centerX + radius * Math.Cos(2 * Math.PI / 3), centerY + radius * Math.Sin(2 * Math.PI / 3))},
                    new LineSegment {Point = new Point(centerX + radius * Math.Cos(Math.PI), centerY + radius * Math.Sin(Math.PI))},
                    new LineSegment {Point = new Point(centerX + radius * Math.Cos(4 * Math.PI / 3), centerY + radius * Math.Sin(4 * Math.PI / 3))},
                    new LineSegment {Point = new Point(centerX + radius * Math.Cos(5 * Math.PI / 3), centerY + radius * Math.Sin(5 * Math.PI / 3))},
                    new LineSegment {Point = startPoint} // Add this line to close the hexagon.
				}
            };
        }

    }
}