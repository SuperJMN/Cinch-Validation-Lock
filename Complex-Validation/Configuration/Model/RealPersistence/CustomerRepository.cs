namespace ComplexValidation.Configuration.Model.RealPersistence
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection connection;
        private IEnumerable<Customer> allCustomers;

        public CustomerRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public Customer Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Customer> GetAll()
        {
            return allCustomers ?? (allCustomers = ExplicitGetAll());
        }

        private IEnumerable<Customer> ExplicitGetAll()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM CLIENTES";
                command.CommandType = CommandType.Text;
                return ExtractFromReader(command.ExecuteReader());
            }
        }

        private IEnumerable<Customer> ExtractFromReader(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return MapFromReader(reader);
            }
        }

        private Customer MapFromReader(IDataReader record)
        {
            var id = (int) (long)record["CLI_ID_CLIENTE"];
            var name = record.IsDBNull(record.GetOrdinal("CLI_NOMBRE")) ? null : (string)record["CLI_NOMBRE"];
            var customer = new Customer(name, id);
            return customer;
        }
    }
}