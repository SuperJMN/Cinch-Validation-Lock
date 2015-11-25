namespace ComplexValidation.Configuration.View.Supporters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class NegateBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Negate(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Negate(value);
        }

        private static object Negate(object value)
        {
            var original = (bool) value;
            return !original;
        }
    }
}