using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SearchTool.Resources.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            return str.Length > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility vis = (Visibility)value;
            return (vis == Visibility.Visible);
        }
    }
}
