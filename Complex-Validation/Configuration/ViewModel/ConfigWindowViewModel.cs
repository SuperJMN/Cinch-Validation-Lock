namespace ComplexValidation.Configuration.ViewModel
{
    using System.Collections.ObjectModel;
    using GalaSoft.MvvmLight.Command;
    using Model;

    public class ConfigWindowViewModel : CinchExtended.ViewModels.EditableValidatingViewModelBase
    {
        private readonly ILomoConfigRepository lomoConfigRepository;
        private RelayCommand duplicateCommand;
        private LomoConfigViewModel selectedConfig;
        private RelayCommand deleteCommand;

        public ConfigWindowViewModel(ILomoConfigRepository lomoConfigRepository)
        {
            this.lomoConfigRepository = lomoConfigRepository;
            Configs = new ObservableCollection<LomoConfigViewModel>(SampleData.Configs);
        }

        public ObservableCollection<LomoConfigViewModel> Configs { get; set; }

        public LomoConfigViewModel SelectedConfig
        {
            get { return selectedConfig; }
            set
            {
                if (selectedConfig != null)
                {
                    selectedConfig.EndEdit();
                }

                selectedConfig = value;

                if (selectedConfig != null)
                {
                    selectedConfig.BeginEdit();
                }

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