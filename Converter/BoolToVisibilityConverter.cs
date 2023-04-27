using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MVVMToolkitTest
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //value를 boolValue로 형변환
            if (value is bool boolValue)
            {
                //true면 trueValue반환, false이면 falseValue반환
                if (boolValue)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
