namespace ComplexValidation
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Windows;
    using CinchExtended.Services.Implementation;
    using Configuration.Model;
    using Configuration.Model.RealPersistence;
    using Configuration.ViewModel;
    using ConfigWindow = Configuration.View.ConfigWindow;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var lomoConfigService = CreateLomoConfigService(LomoConfigServiceType.Database);

            var viewModel = new ConfigWindowViewModel(lomoConfigService, new WpfOpenFileService(), new WpfMessageBoxService());
            var configWindow = new ConfigWindow { DataContext = viewModel };
            configWindow.Show();
        }

        private ILomoConfigService CreateLomoConfigService(LomoConfigServiceType inMemory)
        {
            switch (inMemory)
            {
                case LomoConfigServiceType.InMemory:
                    return new InMemoryLomoConfigService(SampleData.Configs);
                case LomoConfigServiceType.Database:
                    return new RealLomoConfigService(new ConfigRepository(CreateConnection("SIC")), null);
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
