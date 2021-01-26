using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GitlabManager.Converters
{
    
    /// <summary>
    /// Kann wahrscheinlich gelöscht werden? (TODO)
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    internal class BoolToInverseBoolConverter
        : IValueConverter
    {
        public static readonly BoolToInverseBoolConverter Instance = new BoolToInverseBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue))
                return DependencyProperty.UnsetValue;

            return !boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
