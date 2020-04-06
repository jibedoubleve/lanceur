using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Probel.Lanceur.Converters
{
    public class UsageToColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                if (count > 0 && count <= 10) { return Brushes.Red; }
                else if (count > 10 && count <= 50) { return Brushes.Orange; }
                else if (count > 50 && count <= 100) { return Brushes.GreenYellow; }
                else { return Brushes.Green; }
            }
            else { return Brushes.Transparent; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}