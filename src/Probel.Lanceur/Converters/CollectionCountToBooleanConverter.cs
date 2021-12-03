using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.Lanceur.Converters
{
    public class CollectionCountToBooleanConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int integer) { return integer > 0; }
            else { return null; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}