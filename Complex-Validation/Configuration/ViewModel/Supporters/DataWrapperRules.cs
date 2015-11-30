namespace ComplexValidation.Configuration.ViewModel.Supporters
{
    using Cinch;

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

        public static Rule MinMax(DataWrapper<int?> min, DataWrapper<int?> max, string message)
        {
            return new SimpleRule("DataValue", message, dw =>
            {
                var minValue = ExtractValue(min);
                var maxValue = ExtractValue(max);
                return !DataWrapperValidationExpressions.MinimumMaximum(minValue, maxValue);
            });
        }

        private static T ExtractValue<T>(DataWrapper<T> dw)
        {
            return dw.DataValue;
        }

        public static Rule CannotBeNull<T>(string message)
        {
            return new SimpleRule("DataValue", message, o => ExtractValue((DataWrapper<T>) o) == null);
        }
    }
}