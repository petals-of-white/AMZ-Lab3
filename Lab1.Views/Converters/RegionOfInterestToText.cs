using System.Globalization;
using System.Windows.Data;
using Lab1.Models.Tools.ROI;

namespace Lab1.Views.Converters;

[ValueConversion(typeof(IRegionOfInterestInfo), typeof(string))]
public class RegionOfInterestToText : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        IRegionOfInterestInfo roi = (IRegionOfInterestInfo) value;
        if (roi is null) return "";
        else return $"N pixels: {roi.NumberOfPixels}\nArea: {roi.Area}";
        
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}