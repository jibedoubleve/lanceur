using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Probel.Lanceur.Converters
{
    public class StringToColourBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str) { return ColorConverter.ConvertFromString(str); }
            else { return ColorConverter.ConvertFromString("#FF1E1E1E"); }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}