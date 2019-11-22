using MahApps.Metro.Controls;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Models.Settings;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace Probel.Lanceur.Converters
{
    public class MahAppsHotKeyToSettingsHotKey : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HotKeySettingsModel hk)
            {
                var r = new HotKey((Key)hk.Key, (ModifierKeys)hk.ModifierKeys);
                return r;
            }
            else { return value; }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HotKey hk)
            {
                var r = new HotKeySettingsModel((int)hk.ModifierKeys, (int)hk.Key);
                return r;
            }
            else { return value; }
        }

        #endregion Methods
    }
}