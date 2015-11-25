namespace ComplexValidation
{
    using System.Windows;
    using Configuration.ViewModel;
    using ConfigWindow = Configuration.View.ConfigWindow;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var p = new ConfigWindowViewModel(null);
            var configWindow = new ConfigWindow();
            configWindow.DataContext = p;
            configWindow.Show();            
        }
    }
}
