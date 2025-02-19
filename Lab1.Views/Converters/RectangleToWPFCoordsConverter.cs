﻿using System.Globalization;
using System.Windows.Data;
using Lab1.Models.Shapes;

namespace Lab1.Views.Converters;

[ValueConversion(typeof(Rectangle), typeof(double), ParameterType = typeof(WPFCoord))]
public class RectangleToWPFCoordsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var rectangle = (Rectangle) value;
        var coord = (WPFCoord) parameter;
        var topLeft = CoordinatesTransform.ToWPFRectangle(rectangle);
        return coord switch
        {
            WPFCoord.Left => topLeft.X,
            WPFCoord.Top => topLeft.Y,
            _ => System.Windows.DependencyProperty.UnsetValue
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Can't convert from Left Value to Rectangle");
    }
}