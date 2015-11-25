using CinchExtended.BusinessObjects;

namespace TestListBoxCachonda.Configuration
{
    internal static class DataWrapperValidationExpressions
    {
        public static bool IsNullOrEmpty(object o)
        {
            return string.IsNullOrEmpty(((DataWrapper<string>)o).DataValue);
        }

        public static bool NumberBetween(object o, int min, int max)
        {
            var dataValue = ((DataWrapper<int>)o).DataValue;
            return dataValue >= min && dataValue <= max;
        }
    }
}