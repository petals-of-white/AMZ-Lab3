using System.Globalization;
using System.Windows.Data;
using Lab1.Models.Shapes;

namespace Lab1.Views.Converters;

[ValueConversion(typeof(Rectangle), typeof(double), ParameterType = typeof(WPFCoord))]
public class RoiToInfoLayoutCoordsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var rectangle = (Rectangle) value;
        var coord = (WPFCoord) parameter;
        var wpfRectangle = CoordinatesTransform.OverlayInfoCoordinates(rectangle);

        return coord switch
        {
            WPFCoord.Top => wpfRectangle.Top,
            WPFCoord.Left => wpfRectangle.Left,
            _ => System.Windows.DependencyProperty.UnsetValue
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Converting back is not supported");
    }
}