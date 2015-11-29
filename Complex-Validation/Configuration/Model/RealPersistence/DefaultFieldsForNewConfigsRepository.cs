namespace ComplexValidation.Configuration.Model.RealPersistence
{
    using System;
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

        public int Add(Field field)
        {
            var id = GetNextId();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO MAESTRO_ZONAS (MZ_COD, MZ_NOMBRE, MZ_DESC, MZ_TIPO) 
                                        VALUES(:MZ_COD, :MZ_NOMBRE, :MZ_DESC, :MZ_TIPO)";
                command.AddParameter("MZ_COD", id);
                command.AddParameter("MZ_NOMBRE", field.Name);
                command.AddParameter("MZ_DESC", field.Description);
                command.AddParameter("MZ_TIPO", (decimal)field.FieldType);
                command.ExecuteNonQuery();
            }

            return id;
        }

        private int GetNextId()
        {
            int id;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT GENMAESTROZONAS.NEXTVAL FROM DUAL";
                command.CommandType = CommandType.Text;
                decimal seqVal = short.Parse(command.ExecuteScalar().ToString());

                id = (int) seqVal;
            }
            return id;
        }

        public Field Get(int id)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format(@"SELECT * FROM MAESTRO_ZONAS WHERE MZ_COD='{0}'", id);
                return ExtractFieldFromReader(cmd.ExecuteReader()).ToList().Single();
            }
        }
    }
}