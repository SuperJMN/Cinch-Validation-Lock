namespace ComplexValidation
{
    using System.Windows;
    using CinchExtended.Services.Implementation;
    using Configuration.Model;
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

            var viewModel = new ConfigWindowViewModel(new InMemoryLomoConfigService(SampleData.Configs), new WpfOpenFileService(), new WpfMessageBoxService());
            var configWindow = new ConfigWindow { DataContext = viewModel };
            configWindow.Show();
        }        
    }
}
