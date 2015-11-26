namespace ComplexValidation.Configuration.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using CinchExtended.Services.Interfaces;
    using CinchExtended.ViewModels;
    using GalaSoft.MvvmLight.Command;
    using Model;

    public class ConfigWindowViewModel : EditableValidatingViewModelBase
    {
        private readonly ILomoConfigService lomoConfigService;
        private readonly IMessageBoxService messageBoxService;
        private readonly IOpenFileService openFileService;
        private RelayCommand addCommand;
        private RelayCommand deleteCommand;
        private RelayCommand duplicateCommand;
        private RelayCommand saveCommand;
        private LomoConfigViewModel selectedConfig;

        public ConfigWindowViewModel(ILomoConfigService lomoConfigService, IOpenFileService openFileService, IMessageBoxService messageBoxService)
        {
            this.lomoConfigService = lomoConfigService;
            this.openFileService = openFileService;
            this.messageBoxService = messageBoxService;
            Configs =
                new ObservableCollection<LomoConfigViewModel>(
                    lomoConfigService.LomoConfigs.Select(config => ViewModelModelConverter.ConvertToViewModel(config, openFileService)));
        }

        public ObservableCollection<LomoConfigViewModel> Configs { get; set; }

        public LomoConfigViewModel SelectedConfig
        {
            get { return selectedConfig; }
            set
            {
                if (selectedConfig != null)
                {
                    selectedConfig.PropertyChanged -= SelectedConfigOnPropertyChanged;
                }

                selectedConfig = value;

                if (selectedConfig != null)
                {
                    if (!selectedConfig.IsDirty)
                    {
                        selectedConfig.BeginEdit();
                    }

                    selectedConfig.PropertyChanged += SelectedConfigOnPropertyChanged;
                }

                NotifyPropertyChanged("SelectedConfig");
                UpdateCommandsCanExecuteState();
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(Add);
                }

                return addCommand;
            }
        }

        public RelayCommand DuplicateCommand
        {
            get
            {
                return duplicateCommand ??
                       (duplicateCommand = new RelayCommand(Duplicate, () => SelectedConfig != null && SelectedConfig.IsValid && !SelectedConfig.IsDirty));
            }
        }

        public RelayCommand DeleteCommand
        {
            get { return deleteCommand ?? (deleteCommand = new RelayCommand(Delete, () => SelectedConfig != null)); }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand(Save, () => SelectedConfig != null && IsValid));
            }
        }

        private void SelectedConfigOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveCommand.RaiseCanExecuteChanged();
            DuplicateCommand.RaiseCanExecuteChanged();
            AddCommand.RaiseCanExecuteChanged();
        }

        private void UpdateCommandsCanExecuteState()
        {
            DuplicateCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        private void Save()
        {
            if (SelectedConfig.Id == null)
            {
                SelectedConfig.Id = AddNew(SelectedConfig);
            }
            else
            {
                Update(SelectedConfig);
            }
        }

        private void Update(LomoConfigViewModel lomoConfigViewModel)
        {
            var lomoConfig = ModelConverter.ConvertToModel(lomoConfigViewModel);
            lomoConfigService.Update(lomoConfig);
        }

        private int AddNew(LomoConfigViewModel lomoConfigViewModel)
        {
            var lomoConfig = ModelConverter.ConvertToModel(lomoConfigViewModel);
            return lomoConfigService.Add(lomoConfig);
        }

        private void Add()
        {
            var lomoConfigViewModel = new LomoConfigViewModel("Config", openFileService);
            Configs.Add(lomoConfigViewModel);
            SelectedConfig = lomoConfigViewModel;
            UpdateCommandsCanExecuteState();
        }

        private void Delete()
        {
            try
            {
                if (SelectedConfig.Id.HasValue)
                {
                    lomoConfigService.Delete(SelectedConfig.Id.Value);
                }

                Configs.Remove(SelectedConfig);
            }
            catch (Exception)
            {
                messageBoxService.ShowError("Problema al eliminar");
            }
        }

        private void Duplicate()
        {
            Configs.Add((LomoConfigViewModel)SelectedConfig.Clone());
        }

        public override bool IsValid
        {
            get { return Configs.All(c => c.IsValid); }
        }
    }
}