using System;
using System.Windows;
using System.Windows.Data;

namespace InventoryManagement.Common
{
    /// <summary>
    ///     One way IValueConverter that lets you bind a property on a bindable object
    ///     that can be either null, empty string, or white spaces value to a dependency
    ///     property that should be set to Visibility.Collapsed in that case
    /// </summary>
    public sealed class NullVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            return (string.IsNullOrWhiteSpace((string)value)) ? Visibility.Collapsed : Visibility.Visible;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
