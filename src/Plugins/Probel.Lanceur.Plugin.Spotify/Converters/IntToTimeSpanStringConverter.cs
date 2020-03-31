using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.Lanceur.Plugin.Spotify.Converters
{
    public class IntToTimeSpanStringConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value is int ms) ? TimeSpan.FromMilliseconds(ms).ToString(@"mm\:ss") : "N.A.";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}