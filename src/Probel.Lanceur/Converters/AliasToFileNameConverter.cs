using Probel.Lanceur.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.Lanceur.Converters
{
    public class AliasToFileNameConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AliasTextModel model)
            {
                return (model.IsPackaged)
                    ? "Packaged application"
                    : model.FileName;
            }
            else { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}