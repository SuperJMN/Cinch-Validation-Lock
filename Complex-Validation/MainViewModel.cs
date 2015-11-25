namespace TestListBoxCachonda
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using CinchExtended.ViewModels;
    using GalaSoft.MvvmLight.CommandWpf;

    public class MainViewModel : ViewModelBase
    {
        private bool canSelectItem;

        public MainViewModel()
        {
            Items = new[] {"Hola", "cómo", "estás"};
            PromptCommand = new RelayCommand(Execute, () => HasPendingChanges);
        }

        private void Execute()
        {
            CanSelectItem = MessageBox.Show("Pepito", "Título", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        public bool HasPendingChanges { get; set; }

        public bool CanSelectItem
        {
            get { return canSelectItem; }
            set
            {
                if (value == canSelectItem)
                {
                    return;
                }
                canSelectItem = value;
                NotifyPropertyChanged("CanSelectItem");
            }
        }

        public IEnumerable<string> Items { get; set; }

        public string SelectedItem { get; set; }

        public ICommand PromptCommand { get; set; }        
    }
}