namespace ComplexValidation.Configuration.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
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
        private RelayCommand discardCommand;
        private RelayCommand duplicateCommand;
        private RelayCommand exitCommand;
        private RelayCommand saveCommand;
        private LomoConfigViewModel selectedConfig;
        private bool ignoreSelectionChanges;

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
                    Unhook(selectedConfig);
                }

                selectedConfig = value;

                if (selectedConfig != null)
                {
                    Hook(selectedConfig);
                }

                NotifyPropertyChanged("SelectedConfig");
            }
        }

        public RelayCommand AddCommand
        {
            get { return addCommand ?? (addCommand = new RelayCommand(() => Add(), () => !IsDirty)); }
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
            get { return saveCommand ?? (saveCommand = new RelayCommand(Save, () => IsDirty && IsValid)); }
        }

        public override bool IsValid
        {
            get { return Configs.All(c => c.IsValid); }
        }

        public RelayCommand DiscardCommand
        {
            get { return discardCommand ?? (discardCommand = new RelayCommand(DiscardChanges, () => IsDirty)); }
        }

        public bool IsDirty
        {
            get { return Configs.Any(c => c.IsDirty || !c.Id.HasValue); }
        }

        public RelayCommand ExitCommand
        {
            get { return exitCommand ?? (exitCommand = new RelayCommand(() => Application.Current.Shutdown())); }
        }

        private void Hook(EditableValidatingViewModelBase config)
        {
            config.PropertyChanged += ConfigOnPropertyChanged;
            config.BeginEdit();
        }

        private void Unhook(INotifyPropertyChanged config)
        {
            config.PropertyChanged -= ConfigOnPropertyChanged;
        }

        private void ConfigOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdateState();
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
            EndEdit();

            foreach (var config in Configs)
            {
                if (!config.Id.HasValue)
                {
                    config.Id = SaveNew(SelectedConfig);
                }
                else
                {
                    Update(config);
                }
            }

            BeginEdit();

            NotifyPropertyChanged("IsDirty");
            UpdateCommandsCanExecuteState();
        }

        private void Update(LomoConfigViewModel lomoConfigViewModel)
        {
            var lomoConfig = ModelConverter.ConvertToModel(lomoConfigViewModel);
            lomoConfigService.Update(lomoConfig);
        }

        private int SaveNew(LomoConfigViewModel lomoConfigViewModel)
        {
            var lomoConfig = ModelConverter.ConvertToModel(lomoConfigViewModel);
            return lomoConfigService.Add(lomoConfig);
        }

        private void Add()
        {
            var lomoConfigViewModel = new LomoConfigViewModel(GenerateNewName(), openFileService);
            Hook(lomoConfigViewModel);
            Configs.Add(lomoConfigViewModel);
            SelectedConfig = lomoConfigViewModel;
            UpdateState();
        }

        private void UpdateState()
        {
            NotifyPropertyChanged("IsDirty");
            NotifyPropertyChanged("IsValid");
            NotifyPropertyChanged("IsCurrentConfigSesionEditingActive");
            UpdateCommandsCanExecuteState();
        }

        private string GenerateNewName()
        {
            var offset = 1;
            bool isTaken;
            string name;
            do
            {
                var id = Configs
                    .Where(lc => lc.Id.HasValue)
                    .Select(config => config.Id.Value)
                    .DefaultIfEmpty(0)
                    .Max(lc => lc) + offset;

                name = string.Format("Configuratión {0}", id);
                isTaken = Configs.Any(model => string.Equals(model.Name.DataValue, name));

                if (isTaken)
                {
                    offset++;
                }
            } while (isTaken);

            return name;
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

        private void DiscardChanges()
        {
            ignoreSelectionChanges = true;
            ResetChangesInSavedElements();
            DeleteUnsavedElements();
            ignoreSelectionChanges = false;
            UpdateState();
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

        protected override void OnEndEdit()
        {
            foreach (var config in Configs)
            {
                config.EndEdit();
            }
        }

        protected override void OnBeginEdit()
        {
            foreach (var config in Configs)
            {
                config.BeginEdit();
            }
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