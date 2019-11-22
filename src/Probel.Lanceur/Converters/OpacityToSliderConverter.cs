using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.Lanceur.Converters
{
    public class OpacityToSliderConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is double d ? d * 100 : value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is int i ? (double)i / 100 : value;

        #endregion Methods
    }
}