namespace ComplexValidation.Configuration.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using CinchExtended.BusinessObjects;
    using CinchExtended.Services.Interfaces;
    using CinchExtended.ViewModels;
    using GalaSoft.MvvmLight.Command;
    using Microsoft.Win32;
    using Model;
    using Supporters;

    public class LomoConfigViewModel : EditableValidatingViewModelBase, ICloneable
    {
        private readonly string name;
        private readonly IOpenFileService openFileService;
        private static readonly PropertyChangedEventArgs NameChangedArgs = new PropertyChangedEventArgs("Name");
        private RelayCommand addFieldCommand;
        private FieldViewModel selectedField;
        private RelayCommand saveEditCommand;
        private ICommand scapeAttemptCommand;
        private ICommand chooseImageCommand;

        private const string Required = "Requerido";
        private const string InvalidBoxCount = "Número de cajas no válido";

        private LomoConfigViewModel(LomoConfigViewModel lomoConfigViewModel)
        {
            Name = new DataWrapper<string>(this, NameChangedArgs);
            Name.DataValue = string.Copy(lomoConfigViewModel.Name.DataValue);
            Fields = CloneFields(lomoConfigViewModel.Fields);
        }

        public LomoConfigViewModel(string name, IOpenFileService openFileService)
        {
            this.name = name;
            this.openFileService = openFileService;

            Fields = new ObservableCollection<FieldViewModel>();
            CancelEditCommand = new RelayCommand(
               () =>
               {
                   CancelEdit();
                   BeginEdit();
               });

            SetupDataWrappers();
            SetupFixedData();
        }

        private void SetupFixedData()
        {
            Customers = FixedData.Customers.Select(customer => new CustomerViewModel { Id = customer.Id, Name = customer.Name });
        }

        private void SetupDataWrappers()
        {
            Name = new DataWrapper<string>(this, new PropertyChangedEventArgs("Name")) { DataValue = name };
            Name.AddRule(DataWrapperRules.NotNullOrEmtpyRule(Required));

            Description = new DataWrapper<string>(this, new PropertyChangedEventArgs("Description")) { DataValue = "Descripción" };

            ImagePath = new DataWrapper<string>(this, new PropertyChangedEventArgs("ImagePath"));
            ImagePath.AddRule(DataWrapperRules.NotNullOrEmtpyRule(Required));

            BoxCount = new DataWrapper<int>(this, new PropertyChangedEventArgs("BoxCount")) { DataValue = 1 };
            BoxCount.AddRule(DataWrapperRules.NumberBetween(1, 100, InvalidBoxCount));
            SelectedCustomer = new DataWrapper<CustomerViewModel>(this, new PropertyChangedEventArgs("SelectedCustomer"));

            SubscribeToChangesInAllDataWrappers();
        }

        private void SubscribeToChangesInAllDataWrappers()
        {
            foreach (var dataWrapperBase in AllDataWrappers)
            {
                dataWrapperBase.PropertyChanged += OnPropertyChanged;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            NotifyPropertyChanged("IsDirty");
            CancelEditCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }

        public RelayCommand CancelEditCommand { get; set; }

        public IEnumerable<DataWrapperBase> AllDataWrappers
        {
            get { return DataWrapperHelper.GetWrapperProperties(this); }
        }

        public IEnumerable<IChangeIndicator> EditableDataWrappers
        {
            get { return AllDataWrappers.Where(IsChangeIndicator).Cast<IChangeIndicator>(); }
        }

        private static bool IsChangeIndicator(DataWrapperBase dw)
        {
            var type = typeof(IChangeIndicator);
            return type.IsInstanceOfType(dw);
        }

        public DataWrapper<string> Name { get; set; }
        public int? Id { get; set; }
        public ObservableCollection<FieldViewModel> Fields { get; set; }

        public FieldViewModel SelectedField
        {
            get { return selectedField; }
            set
            {
                if (selectedField != null)
                {
                    selectedField.EndEdit();
                }

                selectedField = value;

                if (selectedField != null)
                {
                    selectedField.BeginEdit();
                }

                NotifyPropertyChanged("SelectedField");
            }
        }

        public RelayCommand AddFieldCommand
        {
            get { return addFieldCommand ?? (addFieldCommand = new RelayCommand(AddField)); }
        }

        public DataWrapper<string> Description { get; set; }

        public DataWrapper<string> ImagePath { get; set; }

        public DataWrapper<int> BoxCount { get; set; }

        public DataWrapper<CustomerViewModel> SelectedCustomer { get; set; }

        public object Clone()
        {
            return new LomoConfigViewModel(this);
        }

        private ObservableCollection<FieldViewModel> CloneFields(ObservableCollection<FieldViewModel> fields)
        {
            return new ObservableCollection<FieldViewModel>(fields);
        }

        private void AddField()
        {
            Fields.Add(new FieldViewModel("Field"));
        }

        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            DataWrapperHelper.SetBeginEdit(AllDataWrappers);
        }

        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            DataWrapperHelper.SetEndEdit(AllDataWrappers);
        }

        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            DataWrapperHelper.SetCancelEdit(AllDataWrappers);
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return saveEditCommand ?? (saveEditCommand = new RelayCommand(
                    () =>
                    {
                        EndEdit();
                        BeginEdit();
                        NotifyPropertyChanged("IsDirty");
                    }, () => IsValid));
            }
        }

        public override bool IsValid
        {
            get { return AllDataWrappers.All(dw => dw.IsValid); }
        }

        public bool IsDirty
        {
            get
            {
                var isDirty = EditableDataWrappers.Any(dw => dw.IsDirty);
                return isDirty;
            }
        }

        public IEnumerable<CustomerViewModel> Customers { get; set; }

        public ICommand ScapeAttemptCommand
        {
            get { return scapeAttemptCommand ?? (scapeAttemptCommand = new RelayCommand(OnScapeAttempt)); }
        }

        public ICommand ChooseImageCommand
        {
            get { return chooseImageCommand ?? (chooseImageCommand = new RelayCommand(ChooseImage)); }
        }

        private void ChooseImage()
        {
            var dialogResult = openFileService.ShowDialog(null);
            if (dialogResult.HasValue && dialogResult.Value)
            {
                ImagePath.DataValue = openFileService.FileName;
            }
        }

        private static void OnScapeAttempt()
        {
            MessageBox.Show("No te me escapes, cabrón, que hay cambios sin guardar");
        }
    }
}