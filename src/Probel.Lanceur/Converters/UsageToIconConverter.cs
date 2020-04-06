using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.Lanceur.Converters
{
    public class UsageToIconConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                if (count > 0 && count <= 10) { return "SignalCellularOutline"; }
                else if (count > 10 && count <= 50) { return "SignalCellular1"; }
                else if (count > 50 && count <= 100) { return "SignalCellular2"; }
                else { return "SignalCellular3"; }
            }
            else { return "SignalCellular3"; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}