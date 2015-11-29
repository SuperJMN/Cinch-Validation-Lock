namespace ComplexValidation
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Windows;
    using CinchExtended.Services.Implementation;
    using Configuration.Model;
    using Configuration.Model.InMemory;
    using Configuration.Model.RealPersistence;
    using Configuration.View;
    using Configuration.ViewModel;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var viewModel = CreateLomoConfigService(DataSourceType.Database);
            var configWindow = new ConfigWindow {DataContext = viewModel};
            configWindow.Show();
        }

        private ConfigWindowViewModel CreateLomoConfigService(DataSourceType inMemory)
        {
            switch (inMemory)
            {
                case DataSourceType.InMemory:
                    return new ConfigWindowViewModel(
                        new InMemoryLomoConfigService(SampleData.Configs),
                        new InMemoryDefaultFieldsForNewConfigsRepository(),
                        new InMemoryCustomerRepository(),
                        new WpfOpenFileService(),
                        new WpfMessageBoxService());

                case DataSourceType.Database:

                    var sicConnection = CreateConnection("SIC");
                    var sgCadaConnection = CreateConnection("SGCADA");
                    var realLomoConfigService = new RealLomoConfigService(new ConfigRepository(sicConnection), new CustomerRepository(sgCadaConnection));

                    return new ConfigWindowViewModel(
                        realLomoConfigService,
                        new InMemoryDefaultFieldsForNewConfigsRepository(), 
                        new CustomerRepository(sgCadaConnection),
                        new WpfOpenFileService(),
                        new WpfMessageBoxService());

                default:
                    throw new ArgumentOutOfRangeException("inMemory", inMemory, null);
            }
        }

        private IDbConnection CreateConnection(string connectionStringToken)
        {
            var connStrSettings = ConfigurationManager.ConnectionStrings[connectionStringToken];

            var factory = DbProviderFactories.GetFactory("Oracle.ManagedDataAccess.Client");

            var dbConnection = factory.CreateConnection();
            dbConnection.ConnectionString = connStrSettings.ConnectionString;
            dbConnection.Open();
            return dbConnection;
        }
    }
}