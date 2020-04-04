using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Probel.Lanceur.Converters
{
    public class HexaToColourConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (value is string str) ? (Color)ColorConverter.ConvertFromString(str) : default;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value is Color colour ? colour.ToString() : null;
            return result;
        }

        #endregion Methods
    }
}