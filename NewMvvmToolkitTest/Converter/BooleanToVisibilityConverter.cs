using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NewMvvmToolkitTest
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (boolValue)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            // If the value is not a bool, return Visibility.Collapsed by default
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            // If the value is not a Visibility enum, return false by default
            return false;
        }
    }
}
