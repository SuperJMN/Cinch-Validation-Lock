namespace ComplexValidation.Configuration.Model.RealPersistence
{
    using System;
    using System.Data;

    public static class Extensions
    {
        public static T GetValue<T>(this IDataRecord record, string columnName)
        {
            return record.IsDBNull(record.GetOrdinal(columnName)) ? default(T) : (T) record[columnName];
        }

        public static FieldType GetFieldType(this IDataRecord record, string columnName)
        {
            return (FieldType) record.GetValue<long>(columnName);
        }

        public static void AddParameter(this IDbCommand command, string name, object value)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (name == null)
                throw new ArgumentNullException("name");

            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            command.Parameters.Add(p);
        }

        public static decimal ToDecimal(this bool obj)
        {
            return obj ? 1 : 0;
        }
    }
}