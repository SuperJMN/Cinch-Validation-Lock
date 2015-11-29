namespace ComplexValidation
{
    using System.Configuration;
    using System.Data;
    using System.Data.Common;

    public static class AppConfigConnectionFactory
    {
        public static IDbConnection CreateConnection(string connectionStringToken)
        {
            var connStrSettings = ConfigurationManager.ConnectionStrings[connectionStringToken];

            var factory = DbProviderFactories.GetFactory("Oracle.ManagedDataAccess.Client");

            var dbConnection = factory.CreateConnection();
            dbConnection.ConnectionString = connStrSettings.ConnectionString;
            dbConnection.Open();
            return dbConnection;
        }

        public static IDbConnection CreateSicConnection()
        {
            return CreateConnection("SIC");
        }

        public static IDbConnection CreateSgCadaConnection()
        {
            return CreateConnection("SGCADA");
        }
    }
}