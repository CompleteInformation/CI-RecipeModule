using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Markup;
using System;
using System.Globalization;

namespace CompleteInformation.RecipeModule.AvaloniaApp.Converter
{
    public class IsNotNullConverter : IValueConverter
    {
        public static readonly IsNotNullConverter Instance = new IsNotNullConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IsNotNullConverter can only be used OneWay.");
        }
    }
}
