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
        private RelayCommand discardCommand;
        private ObservableCollection<LomoConfigViewModel> configs;

        public ConfigWindowViewModel(ILomoConfigService lomoConfigService, IOpenFileService openFileService, IMessageBoxService messageBoxService)
        {
            this.lomoConfigService = lomoConfigService;
            this.openFileService = openFileService;
            this.messageBoxService = messageBoxService;
            Configs =
                new ObservableCollection<LomoConfigViewModel>(
                    lomoConfigService.LomoConfigs.Select(config => ViewModelModelConverter.ConvertToViewModel(config, openFileService)));
        }

        public ObservableCollection<LomoConfigViewModel> Configs
        {
            get { return configs; }
            set
            {
                if (configs != null)
                {
                    foreach (var config in configs)
                    {
                        config.PropertyChanged -= ConfigOnPropertyChanged;
                    }
                }

                configs = value;

                if (configs != null)
                {
                    foreach (var config in configs)
                    {
                        config.PropertyChanged += ConfigOnPropertyChanged;
                    }
                }

            }
        }

        private void ConfigOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            NotifyPropertyChanged("IsDirty");
            UpdateCommandsCanExecuteState();
        }

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
            get { return addCommand ?? (addCommand = new RelayCommand(Add)); }
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
                return saveCommand ?? (saveCommand = new RelayCommand(Save, () => IsDirty && IsValid));
            }
        }

        private void SelectedConfigOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdateCommandsCanExecuteState();
        }

        private void UpdateCommandsCanExecuteState()
        {
            DuplicateCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            DiscardCommand.RaiseCanExecuteChanged();
        }

        private void Save()
        {
            foreach (var config in Configs)
            {
                if (!config.Id.HasValue)
                {
                    config.Id = AddNew(SelectedConfig);
                }
                else
                {
                    Update(config);
                }
            }           

            NotifyPropertyChanged("IsDirty");
            UpdateCommandsCanExecuteState();
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
            NotifyPropertyChanged("IsDirty");
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

        public RelayCommand DiscardCommand
        {
            get { return discardCommand ?? (discardCommand = new RelayCommand(DiscardChanges, () => IsDirty)); }
        }

        private void DiscardChanges()
        {
            ResetChangesInSavedElements();
            DeleteUnsavedElements();
        }

        private void DeleteUnsavedElements()
        {
            var configsToDelete = Configs.Where(model => !model.Id.HasValue).ToList();
            foreach (var config in configsToDelete)
            {
                Configs.Remove(config);
            }

            foreach (var config in Configs)
            {
                var fieldsToDelete = config.Fields.Where(model => !model.Id.HasValue).ToList();
                foreach (var field in fieldsToDelete)
                {
                    config.Fields.Remove(field);
                }
            }          
        }

        public bool IsDirty
        {
            get { return Configs.Any(c => c.IsDirty || !c.Id.HasValue); }
        }

        private void ResetChangesInSavedElements()
        {
            foreach (var lomoConfigViewModel in Configs.Where(config => config.Id.HasValue && config.IsDirty))
            {
                lomoConfigViewModel.CancelEdit();
                lomoConfigViewModel.BeginEdit();
                foreach (var field in lomoConfigViewModel.Fields.Where(field => field.Id.HasValue && field.IsDirty))
                {
                    field.CancelEdit();
                    field.BeginEdit();
                }
            }
        }
    }
}