using CinchExtended.BusinessObjects;

internal static class DataWrapperValidationExpressions
{
    public static bool NotNullOrEmpty(object o)
    {
        return string.IsNullOrEmpty(((DataWrapper<string>) o).DataValue);
    }
}