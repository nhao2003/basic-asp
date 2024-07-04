﻿using System;
using System.Linq;
using System.Globalization;
using Microsoft.Maui.Controls;


namespace AwesomeUI.Converters
{
    public class CategoryArrayToStringConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string[] strings)
            {
                return string.Join(", ", strings);
            }
            return string.Empty;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}