using System;
using System.Linq;
using System.Globalization;
using System.Windows.Data;

namespace Probel.Lanceur.Converters
{
    public class StringToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => Core.Constants.RunAs.Admin;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && parameter is Enum)
            {
                var r = from v in Enum.GetValues(targetType).Cast<Enum>()
                        where v.ToString() == str
                        select v;
                return r.FirstOrDefault();
            }
            else { return null; }
        }
    }
}