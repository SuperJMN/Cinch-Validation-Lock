namespace TestListBoxCachonda.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using CinchExtended.BusinessObjects;
    using CinchExtended.ViewModels;
    using GalaSoft.MvvmLight.Command;

    public class LomoConfigViewModel : EditableValidatingViewModelBase, ICloneable
    {
        private readonly string name;
        private static readonly PropertyChangedEventArgs NameChangedArgs = new PropertyChangedEventArgs("Name");
        private RelayCommand addFieldCommand;
        private FieldViewModel selectedField;
        private RelayCommand saveEditCommand;

        private LomoConfigViewModel(LomoConfigViewModel lomoConfigViewModel)
        {
            Name = new DataWrapper<string>(this, NameChangedArgs);
            Name.DataValue = string.Copy(lomoConfigViewModel.Name.DataValue);
            Fields = CloneFields(lomoConfigViewModel.Fields);
        }

        public LomoConfigViewModel(string name)
        {
            this.name = name;

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
            Customers = FixedData.Customers.Select(customer => new CustomerViewModel { Id = customer.Id, Name = customer.Name});
        }

        private void SetupDataWrappers()
        {
            Name = new DataWrapper<string>(this, new PropertyChangedEventArgs("Name")) { DataValue = name };
            Name.AddRule(DataWrapperRules.NotNullOrEmtpyRule("Pon algo"));

            Description = new DataWrapper<string>(this, new PropertyChangedEventArgs("Description")) { DataValue = "Descripción" };
            Description.AddRule(DataWrapperRules.NotNullOrEmtpyRule("Pon algo"));

            ImagePath = new DataWrapper<string>(this, new PropertyChangedEventArgs("ImagePath"));
            BoxCount = new DataWrapper<int>(this, new PropertyChangedEventArgs("BoxCount")) { DataValue = 1 };
            BoxCount.AddRule(DataWrapperRules.NumberBetween(1, 100, "Rango no permitido"));
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
                    }));
            }
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
    }
}