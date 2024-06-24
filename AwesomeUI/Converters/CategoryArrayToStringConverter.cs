using System;
using System.Linq;
using System.Globalization;
using Microsoft.Maui.Controls;


namespace AwesomeUI.Converters
{
    public class CategoryArrayToStringConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Category[] categories)
            {
                return string.Join(", ", categories.Select(c => c.Name));
            }

            return string.Empty;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}