namespace ComplexValidation.Configuration.Model
{
    using System.Collections.Generic;
    using System.Data;

    class ConfigRepository : IConfigRepository
    {
        private readonly IDbConnection connection;

        public ConfigRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<LomoConfig> GetAll()
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM TIPOSOCR";
                return ExtractConfigFromReader(cmd.ExecuteReader());
            }
        }

        private IEnumerable<LomoConfig> ExtractConfigFromReader(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return MapFromReader(reader);
            }
        }

        private LomoConfig MapFromReader(IDataReader record)
        {
            var entity = new LomoConfig
            {
                Id = (int) (decimal) record["TOCRID"],
                Name = GetValue<string>(record, "TOCRDOCNOMBRE"),
                Description = record.IsDBNull(record.GetOrdinal("TOCRDESC")) ? null : (string) record["TOCRDESC"],
                ImagePath = record.IsDBNull(record.GetOrdinal("TOCREXFILE")) ? null : (string) record["TOCREXFILE"],
                CropLeft = record.IsDBNull(record.GetOrdinal("TOCRORX")) ? 0 : (decimal) record["TOCRORX"],
                CropTop = record.IsDBNull(record.GetOrdinal("TOCRORY")) ? 0 : (decimal) record["TOCRORY"],
                CropWidth = record.IsDBNull(record.GetOrdinal("TOCRDX")) ? 0 : (decimal) (record["TOCRDX"] ?? 0),
                CropHeight = record.IsDBNull(record.GetOrdinal("TOCRDY")) ? 0 : (decimal) (record["TOCRDY"] ?? 0),
                Rotation = record.IsDBNull(record.GetOrdinal("TOCRROTACIONINICIAL")) ? 0 : (decimal) record["TOCRROTACIONINICIAL"],
                BoxCount = (int) (record.IsDBNull(record.GetOrdinal("TOCRCAJAS")) ? 0 : (decimal) record["TOCRCAJAS"])
            };

            var customerName = record.IsDBNull(record.GetOrdinal("TOCRCLIENTE")) ? null : (string) record["TOCRCLIENTE"];
            entity.Customer = customerName == null ? null : new Customer(customerName, null);
            //entity.Customer = record.IsDBNull(record.GetOrdinal("TOCRCLIENTE")) ? null : (string)record["TOCRCLIENTE"];

            return entity;
        }

        private static T GetValue<T>(IDataRecord record, string columnName)
        {
            return record.IsDBNull(record.GetOrdinal(columnName)) ? default(T) : (T)record[columnName];
        }

        public int Create(LomoConfig lomoConfig)
        {
            throw new System.NotImplementedException();
        }

        public LomoConfig Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(LomoConfig newLomoConfig)
        {
            throw new System.NotImplementedException();
        }
    }
}