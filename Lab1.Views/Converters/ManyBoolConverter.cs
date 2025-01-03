using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Lab1.Views.Converters;

public class ManyBoolConverter : IMultiValueConverter
{
    public object Convert(object [] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values.Cast<bool>().All(x => x);
    }

    public object [] ConvertBack(object value, Type [] targetTypes, object parameter, CultureInfo culture)
    {
        return targetTypes.Select(_ => value).ToArray();
    }
}