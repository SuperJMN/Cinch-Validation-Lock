using CinchExtended.Validation;

static internal class DataWrapperRules
{
    public static SimpleRule NotNullOrEmtpyRule(string message)
    {
        return new SimpleRule("DataValue", message, DataWrapperValidationExpressions.NotNullOrEmpty);
    }
}