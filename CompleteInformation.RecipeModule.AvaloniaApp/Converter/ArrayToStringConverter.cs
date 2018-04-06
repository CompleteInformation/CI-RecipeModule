using Avalonia;
using Avalonia.Markup;
using System;
using System.Globalization;

namespace CompleteInformation.RecipeModule.AvaloniaApp.Converter
{
    public class ArrayToStringConverter : IValueConverter
    {
        public static readonly ArrayToStringConverter Instance = new ArrayToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string)) {
                throw new InvalidOperationException("The target must be a String");
            }

            return String.Join("\r\n", (string[])value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("ArrayToStringConverter can only be used OneWay.");
        }
    }
}
