namespace ComplexValidation.Configuration.ViewModel.Supporters
{
    using CinchExtended.BusinessObjects;
    using CinchExtended.Validation;

    static internal class DataWrapperRules
    {
        public static SimpleRule NotNullOrEmtpyRule(string message)
        {
            return new SimpleRule("DataValue", message, DataWrapperValidationExpressions.IsNullOrEmpty);
        }

        public static Rule NumberBetween(int min, int max, string message)
        {
            return new SimpleRule("DataValue", message, dw => !DataWrapperValidationExpressions.NumberBetween(dw, min, max));
        }

        public static Rule MinMax(object min, object max, string message)
        {
            return new SimpleRule("DataValue", message, dw =>
            {
                var minValue = ExtractValue<int?>(min);
                var maxValue = ExtractValue<int?>(max);
                return !DataWrapperValidationExpressions.MinimumMaximum(minValue, maxValue);
            });
        }

        private static T ExtractValue<T>(object dw)
        {
            return ((DataWrapper<T>) dw).DataValue;
        }
    }
}