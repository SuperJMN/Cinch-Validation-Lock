namespace TestListBoxCachonda.Configuration.ViewModel.Supporters
{
    using CinchExtended.BusinessObjects;

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

        public static bool MinimumMaximum(int? min, int? max)
        {
            if (min.HasValue && max.HasValue)
            {
                return min.Value <= max.Value;
            }

            return true;
        }
    }
}