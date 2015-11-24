namespace TestListBoxCachonda.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Annotations;
    using CinchExtended.BusinessObjects;
    using CinchExtended.Validation;
    using CinchExtended.ViewModels;
    using GalaSoft.MvvmLight.Command;

    public class FieldViewModel : EditableValidatingViewModelBase
    {
        private readonly string name;
        private static readonly PropertyChangedEventArgs NamePropertyChangeArgs = new PropertyChangedEventArgs("Name");
        private static readonly PropertyChangedEventArgs DescriptionPropertyChangeArgs = new PropertyChangedEventArgs("Description");

        private RelayCommand cancelEditCommand;

        public FieldViewModel([NotNull] string name)
        {
            this.name = name;
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

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
            Angles = FixedData.Angles;
            FieldTypes = FixedData.FieldTypes;
        }

        private void SetupDataWrappers()
        {
            Name = new DataWrapper<string>(this, NamePropertyChangeArgs) { DataValue = name };
            Name.AddRule(DataWrapperRules.NotNullOrEmtpyRule("Pon algo"));

            Description = new DataWrapper<string>(this, DescriptionPropertyChangeArgs) { DataValue = string.Empty };
            Description.AddRule(DataWrapperRules.NotNullOrEmtpyRule("Pon algo"));

            IsActive = new DataWrapper<bool>(this, new PropertyChangedEventArgs("IsActive"));
            IsRequired = new DataWrapper<bool>(this, new PropertyChangedEventArgs("IsRequired"));

            SelectedFieldType = new DataWrapper<FieldType>(this, new PropertyChangedEventArgs("SelectedFieldType"));
            Mask = new DataWrapper<string>(this, new PropertyChangedEventArgs("Mask"));
            Min = new DataWrapper<int?>(this, new PropertyChangedEventArgs("SelectedAngle"));
            Max = new DataWrapper<int?>(this, new PropertyChangedEventArgs("SelectedAngle"));
            FixedValue = new DataWrapper<string>(this, new PropertyChangedEventArgs("FixedValue"));

            ValidChars = new DataWrapper<string>(this, new PropertyChangedEventArgs("ValidChars"));
            SelectedAngle = new DataWrapper<int>(this, new PropertyChangedEventArgs("SelectedAngle")) { DataValue = 0 };

            SubscribeToChangesInAllDataWrappers();
        }

        private void SubscribeToChangesInAllDataWrappers()
        {
            foreach (var dataWrapperBase in AllDataWrappers)
            {
                dataWrapperBase.PropertyChanged += OnPropertyChanged;
            }
        }

        public IEnumerable<DataWrapperBase> AllDataWrappers
        {
            get { return DataWrapperHelper.GetWrapperProperties(this); }
        }

        public IEnumerable<IChangeIndicator> EditableDataWrappers
        {
            get { return AllDataWrappers.Where(IsChangeIndicator).Cast<IChangeIndicator>(); }
        }

        public bool IsDirty
        {
            get
            {
                var isDirty = EditableDataWrappers.Any(dw => dw.IsDirty);
                return isDirty;
            }
        }

        public DataWrapper<string> Name { get; set; }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            NotifyPropertyChanged("IsDirty");
            CancelEditCommand.RaiseCanExecuteChanged();
        }

        private bool IsChangeIndicator(DataWrapperBase dw)
        {
            var type = typeof(IChangeIndicator);
            return type.IsInstanceOfType(dw);
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
                return cancelEditCommand ?? (cancelEditCommand = new RelayCommand(
                    () =>
                    {
                        EndEdit();
                        BeginEdit();
                        NotifyPropertyChanged("IsDirty");
                    }));
            }
        }

        public RelayCommand CancelEditCommand { get; }

        public DataWrapper<string> Description { get; set; }

        public DataWrapper<bool> IsActive { get; set; }

        public DataWrapper<bool> IsRequired { get; set; }

        public IEnumerable<FieldType> FieldTypes { get; set; }

        public DataWrapper<FieldType> SelectedFieldType { get; set; }

        public DataWrapper<string> Mask { get; set; }

        public DataWrapper<int?> Min { get; set; }

        public DataWrapper<int?> Max { get; set; }

        public DataWrapper<string> FixedValue { get; set; }

        public DataWrapper<string> ValidChars { get; set; }

        public IEnumerable<int> Angles { get; set; }
        public DataWrapper<int> SelectedAngle { get; set; }
    }
}