using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace QuickPick.Converters;

public class CenterToMarginConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            var diameter = double.Parse(value.ToString()) /2;
            var buttonSize = double.Parse(parameter.ToString());

            var margin = new Thickness(diameter-buttonSize/2);
            return margin;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return new Thickness();
    }
}
