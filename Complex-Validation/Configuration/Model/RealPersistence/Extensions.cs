namespace ComplexValidation.Configuration.Model.RealPersistence
{
    using System.Data;

    public static class Extensions
    {
        public static T GetValue<T>(this IDataRecord record, string columnName)
        {
            return record.IsDBNull(record.GetOrdinal(columnName)) ? default(T) : (T)record[columnName];
        }
    }
}