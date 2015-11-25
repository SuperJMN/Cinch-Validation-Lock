namespace ComplexValidation.Configuration.ViewModel
{
    using System.Collections.ObjectModel;
    using CinchExtended.Services.Interfaces;
    using GalaSoft.MvvmLight.Command;
    using Model;

    public class ConfigWindowViewModel : CinchExtended.ViewModels.EditableValidatingViewModelBase
    {
        private readonly ILomoConfigRepository lomoConfigRepository;
        private readonly IOpenFileService openFileService;
        private RelayCommand duplicateCommand;
        private LomoConfigViewModel selectedConfig;
        private RelayCommand deleteCommand;

        public ConfigWindowViewModel(ILomoConfigRepository lomoConfigRepository, IOpenFileService openFileService)
        {
            this.lomoConfigRepository = lomoConfigRepository;
            this.openFileService = openFileService;
            var sampleData = new SampleData(openFileService);
            Configs = new ObservableCollection<LomoConfigViewModel>(sampleData.Configs);
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
            Configs.Add(new LomoConfigViewModel("Config", openFileService));
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