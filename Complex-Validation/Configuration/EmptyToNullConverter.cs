namespace TestListBoxCachonda
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class EmptyToNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string) value))
            {
                return null;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}