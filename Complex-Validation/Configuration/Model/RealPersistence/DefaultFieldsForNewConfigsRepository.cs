namespace ComplexValidation.Configuration.Model.RealPersistence
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public class DefaultFieldsForNewConfigsRepository : IDefaultFieldsForNewConfigsRepository
    {
        private readonly IDbConnection connection;

        public DefaultFieldsForNewConfigsRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<Field> GetAll()
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM MAESTRO_ZONAS";
                return ExtractFieldFromReader(cmd.ExecuteReader()).ToList();
            }
        }

        private static IEnumerable<Field> ExtractFieldFromReader(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return MapFromReader(reader);
            }
        }

        private static Field MapFromReader(IDataRecord reader)
        {
            var field = new Field();
            field.Id = (int)reader.GetValue<long>("MZ_COD");
            field.Name = reader.GetValue<string>("MZ_NOMBRE");
            field.Description = reader.GetValue<string>("MZ_DESC");
            field.FieldType = reader.GetFieldType("MZ_TIPO");

            return field;
        }
    }
}