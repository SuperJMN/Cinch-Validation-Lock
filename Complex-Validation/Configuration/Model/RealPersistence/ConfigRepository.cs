namespace ComplexValidation.Configuration.Model.RealPersistence
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public class ConfigRepository : IConfigRepository
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
                return ExtractConfigFromReader(cmd.ExecuteReader()).ToList();
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

        private static bool ExtractBool(long record)
        {
            var v = record;
            return v == 1;
        }
        private static bool ExtractBool(decimal record)
        {
            var v = record;
            return v == 1;
        }

        private static FieldType GetFieldType(decimal fieldTypeId)
        {
            return (FieldType) (int) fieldTypeId;
        }

        public int Create(LomoConfig lomoConfig)
        {
            using (var tr = connection.BeginTransaction())
            {
                var id = GetNextId();
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO TIPOSOCR (TOCRDOCNOMBRE, TOCRDESC, TOCREXFILE, TOCRCLIENTE, TOCRCAJAS) 
                                        VALUES(:TOCRDOCNOMBRE, :TOCRDESC, :TOCREXFILE, :TOCRCLIENTE, :TOCRCAJAS)";

                    command.AddParameter("TOCRDOCNOMBRE", lomoConfig.Name);
                    command.AddParameter("TOCRDESC", lomoConfig.Description);
                    command.AddParameter("TOCREXFILE", lomoConfig.ImagePath);
                    command.AddParameter("TOCRCLIENTE", lomoConfig.Customer.Name);
                    command.AddParameter("TOCRCAJAS", lomoConfig.BoxCount);
                    command.ExecuteNonQuery();
                }

                foreach (var field in lomoConfig.Fields)
                {
                    CreateFieldsForConfig(connection, field, id+1);
                }                

                tr.Commit();
                return id+1;
            }
        }

        private void CreateFieldsForConfig(IDbConnection dbConnection, Field field, int id)
        {
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO ZONASOCR (ZOCRTIPODOC, ZOCRNOMBRE, ZOCRDESCRIPCION, ZOCRACTIVO, ZOCRREQUERIDO,ZOCRTIPOZONA,ZOCRMASCARA, ZOCRMINVALUE, ZOCRMAXVALUE, ZOCRVALORFIJO, ZOCRFORBCHAR,
                                                                ZOCRROTACION, ZOCRORIGENX, ZOCRORIGENY, ZDELTAX, ZDELTAY) 
                                        VALUES(:ZOCRTIPODOC, :ZOCRNOMBRE, :ZOCRDESCRIPCION, :ZOCRACTIVO, :ZOCRREQUERIDO, :ZOCRTIPOZONA, :ZOCRMASCARA, :ZOCRMINVALUE, :ZOCRMAXVALUE, :ZOCRVALORFIJO, :ZOCRFORBCHAR,
                                                                :ZOCRROTACION, :ZOCRORIGENX, :ZOCRORIGENY, :ZDELTAX, :ZDELTAY)";
                command.AddParameter("ZOCRTIPODOC", id);
                command.AddParameter("ZOCRNOMBRE", field.Name);
                command.AddParameter("ZOCRDESCRIPCION", field.Description);
                command.AddParameter("ZOCRACTIVO", field.IsActive.ToDecimal());
                command.AddParameter("ZOCRREQUERIDO", field.IsRequired.ToDecimal());
                command.AddParameter("ZOCRTIPOZONA", (int)field.FieldType);
                command.AddParameter("ZOCRMASCARA", field.Mask);
                command.AddParameter("ZOCRMINVALUE", field.Min);
                command.AddParameter("ZOCRMAXVALUE", field.Max);
                command.AddParameter("ZOCRVALORFIJO", field.FixedValue);
                command.AddParameter("ZOCRFORBCHAR", field.ValidChars);
                command.AddParameter("ZOCRROTACION", field.Angle);
                command.AddParameter("ZOCRORIGENX", field.Left);
                command.AddParameter("ZOCRORIGENY", field.Top);
                command.AddParameter("ZDELTAX", field.Width);
                command.AddParameter("ZDELTAY", field.Height);
                command.ExecuteNonQuery();
            }
        }


        private int GetNextId()
        {
            int id;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT GENTIPOSOCRID.NEXTVAL FROM DUAL";
                command.CommandType = CommandType.Text;
                decimal seqVal = short.Parse(command.ExecuteScalar().ToString());

                id = (int)seqVal;
            }
            return id;
        }

        public LomoConfig Get(int id)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT * FROM TIPOSOCR WHERE TOCRID={0}", id);
                var executeReader = cmd.ExecuteReader();
                var configs = ExtractConfigFromReader(executeReader);
                return configs.Single();
            }
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