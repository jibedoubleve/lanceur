using Probel.Lanceur.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Probel.Lanceur.Converters
{
    public class NotificationEnumToStringConverter : IValueConverter
    {
        #region Fields

        private const string Notification_Classic = "Internal notification";
        private const string Notification_Win10 = "Windows 10 notification";

        #endregion Fields

        #region Methods

        private static string EnumToString(NotificationTypes notifType)
        {
            switch (notifType)
            {
                case NotificationTypes.Classic: return Notification_Classic;
                case NotificationTypes.Win10: return Notification_Win10;
                default: throw new NotSupportedException($"Cannot convert '{notifType}' into an enumeration '{typeof(NotificationTypes)}'");
            }
        }

        private static NotificationTypes StringToEnum(string str)
        {
            switch (str)
            {
                case Notification_Classic: return NotificationTypes.Classic;
                case Notification_Win10: return NotificationTypes.Win10;
                default: throw new NotSupportedException($"Cannot convert '{str}' into '{typeof(NotificationTypes)}'");
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string val && Enum.TryParse(val, true, out NotificationTypes resType))
            {
                return EnumToString(resType);
            }
            if (value is NotificationTypes[] array)
            {
                var strings = new List<string>();
                foreach (var item in array)
                {
                    strings.Add(EnumToString(item));
                }
                return strings;
            }
            else { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str) { return StringToEnum(str); }
            else { return value; }
        }

        #endregion Methods
    }
}