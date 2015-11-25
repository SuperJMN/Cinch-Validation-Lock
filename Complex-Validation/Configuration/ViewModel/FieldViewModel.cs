namespace ComplexValidation.Configuration.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using CinchExtended.BusinessObjects;
    using CinchExtended.ViewModels;
    using GalaSoft.MvvmLight.Command;
    using Model;
    using Properties;
    using Supporters;

    public class FieldViewModel : EditableValidatingViewModelBase
    {
        private readonly string name;
        private static readonly PropertyChangedEventArgs NamePropertyChangeArgs = new PropertyChangedEventArgs("Name");
        private static readonly PropertyChangedEventArgs DescriptionPropertyChangeArgs = new PropertyChangedEventArgs("Description");

        private const string Required = "Requerido";
        private const string InvalidRange = "Rango no válido";

        private RelayCommand saveCommand;
        private ICommand scapeAttemptCommand;

        public FieldViewModel([NotNull] string name)
        {
            this.name = name;
            if (name == null)
            {
                throw new ArgumentNullException(name);
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
            Name.AddRule(DataWrapperRules.NotNullOrEmtpyRule(Required));

            Description = new DataWrapper<string>(this, DescriptionPropertyChangeArgs) { DataValue = string.Empty };

            IsActive = new DataWrapper<bool>(this, new PropertyChangedEventArgs("IsActive"));
            IsRequired = new DataWrapper<bool>(this, new PropertyChangedEventArgs("IsRequired"));

            SelectedFieldType = new DataWrapper<FieldType>(this, new PropertyChangedEventArgs("SelectedFieldType"));
            Mask = new DataWrapper<string>(this, new PropertyChangedEventArgs("Mask"));
            Min = new DataWrapper<int?>(this, new PropertyChangedEventArgs("Min"));
            Max = new DataWrapper<int?>(this, new PropertyChangedEventArgs("Max"));

            Min.AddRule(DataWrapperRules.MinMax(Min, Max, InvalidRange));
            Max.AddRule(DataWrapperRules.MinMax(Min, Max, InvalidRange));

            Max.PropertyChanged += UpdateMinMaxState;
            Min.PropertyChanged += UpdateMinMaxState;


            FixedValue = new DataWrapper<string>(this, new PropertyChangedEventArgs("FixedValue"));

            ValidChars = new DataWrapper<string>(this, new PropertyChangedEventArgs("ValidChars"));
            SelectedAngle = new DataWrapper<int>(this, new PropertyChangedEventArgs("SelectedAngle")) { DataValue = 0 };

            SubscribeToChangesInAllDataWrappers();
        }

        private void UpdateMinMaxState(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Max");
            NotifyPropertyChanged("Min");
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
            SaveCommand.RaiseCanExecuteChanged();
        }

        private static bool IsChangeIndicator(DataWrapperBase dw)
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
                return saveCommand ?? (saveCommand = new RelayCommand(
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

        public RelayCommand CancelEditCommand { get; set; }

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

        public ICommand ScapeAttemptCommand
        {
            get { return scapeAttemptCommand ?? (scapeAttemptCommand = new RelayCommand(OnScapeAttempt)) ; }
        }

        private static void OnScapeAttempt()
        {
            MessageBox.Show("No te me escapes, cabrón, que hay cambios sin guardar");
        }
    }
}