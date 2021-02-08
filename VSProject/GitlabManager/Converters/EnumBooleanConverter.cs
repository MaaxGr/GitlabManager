using System;
using System.Globalization;
using System.Windows.Data;

namespace GitlabManager.Converters
{
    /// <summary>
    /// Converter that can convert enum values to booleans.
    ///
    /// Code from StackOverflow:
    /// https://stackoverflow.com/questions/9212873/binding-radiobuttons-group-to-a-property-in-wpf
    /// #LOC
    /// </summary>
    public class EnumBooleanConverter: IValueConverter
    {
        
        public static readonly EnumBooleanConverter Instance = new EnumBooleanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : Binding.DoNothing;
        }
    }
}