using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;

namespace QuickPick.Custom;

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

        double angleStep = 10; // Set the desired angle step between buttons (in degrees)
        double totalAngle = 360; // Set the total angle (in degrees) for the circle
        double radius = Math.Min(finalSize.Width, finalSize.Height) / 2;

        // Calculate the minimum size required for each child
        double minWidth = 0;
        double minHeight = 0;

        foreach (UIElement child in Children)
        {
            minWidth = Math.Max(minWidth, child.DesiredSize.Width);
            minHeight = Math.Max(minHeight, child.DesiredSize.Height);
        }

        // Calculate the actual spacing based on the minimum size of the children
        double actualSpacing = 2 * radius * Math.Sin((angleStep / 2) * (Math.PI / 180)) + Math.Max(minWidth, minHeight);

        int numButtons = (int)Math.Floor(totalAngle / angleStep); // Calculate the number of buttons that can fit in the circle
        angleStep = totalAngle / numButtons; // Recalculate the angle step based on the number of buttons

        double angle = 0;
        foreach (UIElement child in Children)
        {
            double x = (finalSize.Width / 2) + (radius * Math.Cos(angle * Math.PI / 180)) - (child.DesiredSize.Width / 2);
            double y = (finalSize.Height / 2) + (radius * Math.Sin(angle * Math.PI / 180)) - (child.DesiredSize.Height / 2);

            child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
            angle += angleStep;
            angle += (actualSpacing / radius) * (180 / Math.PI); // Increase the angle by the desired spacing
        }

        return finalSize;
    }

    //protected override Size ArrangeOverride(Size finalSize)
    //{
    //    if (Children.Count == 0) return finalSize;

    //    double angleStep = 360.0 / Children.Count;
    //    double angle = 0;
    //    double radius = Math.Min(finalSize.Width, finalSize.Height) / 2;

    //    foreach (UIElement child in Children)
    //    {
    //        double x = (finalSize.Width / 2) + (radius * Math.Cos(angle * Math.PI / 180)) - (child.DesiredSize.Width / 2);
    //        double y = (finalSize.Height / 2) + (radius * Math.Sin(angle * Math.PI / 180)) - (child.DesiredSize.Height / 2);

    //        child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
    //        angle += angleStep;
    //    }

    //    return finalSize;
    //}
}
