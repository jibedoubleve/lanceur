using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Probel.Lanceur.Converters
{
    public class InvertBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = new BooleanToVisibilityConverter().Convert(value, targetType, parameter, culture);
            if (result is Visibility v)
            {
                return v == (Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            }
            else { return result; }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}