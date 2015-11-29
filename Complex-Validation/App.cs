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
                        new InMemoryDefaultFieldsRepository(),
                        new InMemoryCustomerRepository(),
                        new WpfOpenFileService(),
                        new WpfMessageBoxService());

                case DataSourceType.Database:

                    var sicConnection = AppConfigConnectionFactory.CreateSicConnection();
                    var sgCadaConnection = AppConfigConnectionFactory.CreateSgCadaConnection();
                    var realLomoConfigService = new RealLomoConfigService(new ConfigRepository(sicConnection), new CustomerRepository(sgCadaConnection));

                    return new ConfigWindowViewModel(
                        realLomoConfigService,
                        new DefaultFieldsRepository(sicConnection), 
                        new CustomerRepository(sgCadaConnection),
                        new WpfOpenFileService(),
                        new WpfMessageBoxService());

                default:
                    throw new ArgumentOutOfRangeException("inMemory", inMemory, null);
            }
        }
    }
}