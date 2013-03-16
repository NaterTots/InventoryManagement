﻿using System;
using System.Windows.Data;

namespace InventoryManagement.Common
{
    /// <summary>
    /// Two way IValueConverter that lets you bind the inverse of a boolean property
    /// to a dependency property
    /// </summary>
    public sealed class NotOperatorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
