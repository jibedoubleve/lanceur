using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Probel.Lanceur.Plugin.Spotify.Converters
{
    public class InvertBooleanToVisibilityConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible) { return isVisible ? Visibility.Collapsed: Visibility.Visible; }
            else { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility && visibility== Visibility.Visible) { return false; }
            else { return true; }
        }

        #endregion Methods
    }
}