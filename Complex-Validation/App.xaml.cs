using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TestListBoxCachonda
{
    using Configuration;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var p = new ConfigWindowViewModel(null, null);
            var configWindow = new Configuration.ConfigWindow();
            configWindow.DataContext = p;
            configWindow.Show();            
        }
    }
}
