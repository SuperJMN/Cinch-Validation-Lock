namespace ComplexValidation.Configuration.Model.RealPersistence
{
    using System;
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

        private LomoConfig MapFromReader(IDataRecord record)
        {
            var id = (int) (decimal) record["TOCRID"];

            var entity = new LomoConfig
            {
                Id = id,
                Name = record.GetValue<string>("TOCRDOCNOMBRE"),
                Description = record.GetValue<string>("TOCRDESC"),
                ImagePath = record.GetValue<string>("TOCREXFILE"),
                CropLeft = record.GetValue<decimal>("TOCRORX"),
                CropTop = record.GetValue<decimal>("TOCRORY"),
                CropWidth = record.GetValue<decimal>("TOCRDX"),
                CropHeight = record.GetValue<decimal>("TOCRDY"),
                Rotation = record.GetValue<decimal>("TOCRROTACIONINICIAL"),
                BoxCount = (int) record.GetValue<decimal>("TOCRCAJAS"),
                Fields = GetFields(id),
            };

            var customerName = record.GetValue<string>("TOCRCLIENTE");
            entity.Customer = customerName == null ? null : new Customer(customerName, null);

            return entity;
        }

        private IEnumerable<Field> GetFields(int id)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format(@"SELECT * FROM ZONASOCR WHERE ZOCRTIPODOC = '{0}'", id);
                return ExtractFieldsFromReader(cmd.ExecuteReader());
            }
        }

        private IEnumerable<Field> ExtractFieldsFromReader(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return MapFieldFromReader(reader);
            }
        }

        private Field MapFieldFromReader(IDataRecord record)
        {
            var entity = new Field();
            var id = (int)record.GetValue<decimal>("ZOCRID");
            entity.Id = id;
            entity.SpineCode = record.GetValue<decimal>("ZOCRTIPODOC");
            entity.Name = record.GetValue<string>("ZOCRNOMBRE");
            entity.Description = record.GetValue<string>("ZOCRDESCRIPCION");
            entity.IsActive = ExtractBool((long)record["ZOCRACTIVO"]);
            entity.IsRequired = ExtractBool((decimal)record["ZOCRREQUERIDO"]);
            entity.FieldType = GetFieldType(record.GetValue<decimal>("ZOCRTIPOZONA"));
            entity.Mask = record.GetValue<string>("ZOCRMASCARA");
            entity.Min = (int)record.GetValue<decimal>("ZOCRMINVALUE");
            entity.Min = (int)record.GetValue<decimal>("ZOCRMAXVALUE");
            entity.FixedValue = record.GetValue<string>("ZOCRVALORFIJO");
            entity.ValidChars = record.GetValue<string>("ZOCRFORBCHAR");
            entity.Angle = (int)record.GetValue<decimal>("ZOCRROTACION");
            entity.Left = record.GetValue<decimal>("ZOCRORIGENX");
            entity.Top = record.GetValue<decimal>("ZOCRORIGENY");
            entity.Width = record.GetValue<decimal>("ZDELTAX");
            entity.Height = record.GetValue<decimal>("ZDELTAY");

            return entity;
        }

        private bool ExtractBool(long record)
        {
            var v = record;
            return v == 1;
        }
        private bool ExtractBool(decimal record)
        {
            var v = record;
            return v == 1;
        }

        private FieldType GetFieldType(decimal fieldTypeId)
        {
            return (FieldType) (int) fieldTypeId;
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