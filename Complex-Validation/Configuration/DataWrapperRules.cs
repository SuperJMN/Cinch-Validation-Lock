using CinchExtended.Validation;
using TestListBoxCachonda.Configuration;

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
}