namespace ComplexValidation.Configuration.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using Cinch;
    using GalaSoft.MvvmLight.Command;
    using Model.RealPersistence;
    using Supporters;

    public class LomoConfigViewModel : FullyFledgedViewModel, ICloneable
    {
        private readonly string name;
        private readonly IOpenFileService openFileService;
        private readonly ICustomerRepository customerRepository;
        private RelayCommand addFieldCommand;
        private FieldViewModel selectedField;
        private ICommand chooseImageCommand;
        private ObservableCollection<FieldViewModel> fields;

        private const string Required = "Requerido";
        private const string InvalidBoxCount = "N�mero de cajas no v�lido";
        private const string CannotBeNull = "Requerido";

        //private LomoConfigViewModel(LomoConfigViewModel lomoConfigViewModel)
        //{
        //    Name = new DataWrapper<string>(this, new PropertyChangedEventArgs("Name"));
        //    Name.DataValue = string.Copy(lomoConfigViewModel.Name.DataValue);
        //    Fields = CloneFields(lomoConfigViewModel.Fields);
        //}

        public LomoConfigViewModel(string name, IOpenFileService openFileService, ICustomerRepository customerRepository)
        {
            this.name = name;
            this.openFileService = openFileService;
            this.customerRepository = customerRepository;

            DisplayName = "la Configuraci�n de Lomo";
            Customers = this.customerRepository.GetAll().Select(customer => new CustomerViewModel { Id = customer.Id, Name = customer.Name });

            SetupDataWrappers();
        }

        private void SetupDataWrappers()
        {
            Name = new DataWrapper<string>(this, new PropertyChangedEventArgs("Name")) { DataValue = name };
            Name.AddRule(DataWrapperRules.NotNullOrEmtpyRule(Required));

            Description = new DataWrapper<string>(this, new PropertyChangedEventArgs("Description")) { DataValue = string.Empty };

            ImagePath = new DataWrapper<string>(this, new PropertyChangedEventArgs("ImagePath"));
            ImagePath.AddRule(DataWrapperRules.NotNullOrEmtpyRule(Required));

            BoxCount = new DataWrapper<int?>(this, new PropertyChangedEventArgs("BoxCount")) { DataValue = 1 };
            BoxCount.AddRule(DataWrapperRules.NumberBetween(1, 100, InvalidBoxCount));
            SelectedCustomer = new DataWrapper<CustomerViewModel>(this, new PropertyChangedEventArgs("SelectedCustomer"));
            SelectedCustomer.AddRule(DataWrapperRules.CannotBeNull<CustomerViewModel>(CannotBeNull));

            SubscribeToChangesInAllDataWrappers();
        }

        public DataWrapper<string> Name { get; set; }

        public ObservableCollection<FieldViewModel> Fields
        {
            get { return fields; }
            set
            {
                fields = value;
                foreach (var fieldViewModel in fields)
                {
                    SubscribeToChanges(fieldViewModel);
                }
            }
        }

        public FieldViewModel SelectedField
        {
            get { return selectedField; }
            set
            {
                if (selectedField != null)
                {
                    UnsubscribeFromChanges(selectedField);
                }

                selectedField = value;

                if (selectedField != null)
                {
                    if (!selectedField.IsDirty)
                    {
                        selectedField.BeginEdit();
                    }
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

        public DataWrapper<int?> BoxCount { get; set; }

        public DataWrapper<CustomerViewModel> SelectedCustomer { get; set; }

        public object Clone()
        {
            //return new LomoConfigViewModel(this);
            throw new NotImplementedException();
        }

        private ObservableCollection<FieldViewModel> CloneFields(IEnumerable<FieldViewModel> fields)
        {
            return new ObservableCollection<FieldViewModel>(fields);
        }

        private void AddField()
        {
            var newField = new FieldViewModel("Field");
            Fields.Add(newField);
            SelectedField = newField;
            SubscribeToChanges(newField);
        }

        private void SubscribeToChanges(FieldViewModel field)
        {
            field.PropertyChanged += FieldOnPropertyChanged;
        }

        private void UnsubscribeFromChanges(FieldViewModel field)
        {
            field.PropertyChanged -= FieldOnPropertyChanged;
        }

        private void FieldOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            NotifyPropertyChanged("IsValid");
        }

        public IEnumerable<CustomerViewModel> Customers { get; set; }

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

        public override bool IsValid
        {
            get
            {
                var myDataIsValid = base.IsValid;
                return myDataIsValid && Fields.All(f => f.IsValid);
            }
        }

        public override bool IsDirty
        {
            get { return !Id.HasValue || base.IsDirty || Fields.Any(f => f.IsDirty || !f.Id.HasValue); }
        }
    }
}