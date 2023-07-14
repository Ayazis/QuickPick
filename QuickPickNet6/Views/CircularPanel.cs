using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;

namespace QuickPick.UI.Views;

public class CircularPanel : Panel
{
    public CircularPanel()
    {

    }
    protected override Size MeasureOverride(Size availableSize)
    {
        foreach (UIElement child in Children)
        {
            child.Measure(availableSize);
        }

        return base.MeasureOverride(availableSize);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Children.Count == 0) return finalSize;

        double totalAngle = 360; // Set the total angle (in degrees) for the circle
        double radius = Math.Min(finalSize.Width, finalSize.Height) / 2;

        int numItems = Children.Count;

        // Calculate the angle step based on the number of items
        double angleStep = totalAngle / numItems;

        double angle = -90;
        foreach (UIElement child in Children)
        {
            double x = finalSize.Width / 2 + radius * Math.Cos(angle * Math.PI / 180) - child.DesiredSize.Width / 2;
            double y = finalSize.Height / 2 + radius * Math.Sin(angle * Math.PI / 180) - child.DesiredSize.Height / 2;

            child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
            angle += angleStep;
        }

        return finalSize;
    }
}
