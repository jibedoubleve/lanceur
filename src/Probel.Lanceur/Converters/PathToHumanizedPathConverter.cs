using Humanizer;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.Lanceur.Converters
{
    public class PathToHumanizedPathConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return str.Truncate(120, "(...)", Truncator.FixedLength);
            }
            else { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}