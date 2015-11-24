namespace TestListBoxCachonda.Configuration
{
    using System.Collections.ObjectModel;
    using GalaSoft.MvvmLight.Command;

    public class ConfigWindowViewModel : CinchExtended.ViewModels.EditableValidatingViewModelBase
    {
        private RelayCommand duplicateCommand;
        private LomoConfigViewModel selectedConfig;
        private RelayCommand deleteCommand;

        public ConfigWindowViewModel(ILomoConfigRepository lomoConfigRepository, ILomoFieldsRepository lomoFieldsRepository)
        {
            Configs = new ObservableCollection<LomoConfigViewModel>(SampleData.Configs);
        }

        public ObservableCollection<LomoConfigViewModel> Configs { get; set; }

        public LomoConfigViewModel SelectedConfig
        {
            get { return selectedConfig; }
            set
            {
                selectedConfig = value;
                NotifyPropertyChanged("SelectedConfig");
                UpdateCommandsCanExecuteState();
            }
        }

        private void UpdateCommandsCanExecuteState()
        {
            DuplicateCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        public RelayCommand AddCommand
        {
            get { return new RelayCommand(Add); }
        }

        public RelayCommand DuplicateCommand
        {
            get { return duplicateCommand ?? (duplicateCommand = new RelayCommand(Duplicate, () => SelectedConfig != null)); }
        }

        public RelayCommand DeleteCommand
        {
            get { return deleteCommand ?? (deleteCommand = new RelayCommand(Delete, () => SelectedConfig != null)); }
        }
      
        private void Add()
        {
            Configs.Add(new LomoConfigViewModel("Config"));
        }

        private void Delete()
        {
            Configs.Remove(SelectedConfig);
        }

        private void Duplicate()
        {
            Configs.Add((LomoConfigViewModel)SelectedConfig.Clone());
        }
    }
}