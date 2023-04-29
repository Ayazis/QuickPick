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

        double angleStep = 360.0 / Children.Count;
        double angle = 0;
        double radius = Math.Min(finalSize.Width, finalSize.Height) / 2;

        foreach (UIElement child in Children)
        {
            double x = (finalSize.Width / 2) + (radius * Math.Cos(angle * Math.PI / 180)) - (child.DesiredSize.Width / 2);
            double y = (finalSize.Height / 2) + (radius * Math.Sin(angle * Math.PI / 180)) - (child.DesiredSize.Height / 2);

            child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
            angle += angleStep;
        }

        return finalSize;
    }
}
