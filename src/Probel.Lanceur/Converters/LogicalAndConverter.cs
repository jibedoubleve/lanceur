using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.Lanceur.Converters
{
    public class LogicalAndConverter : IMultiValueConverter
    {
        #region Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value is bool boolean)
                {
                    if (!boolean) { return false; }
                }
                else { return false; }
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}