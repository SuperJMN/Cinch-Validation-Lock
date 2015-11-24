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
        private static readonly PropertyChangedEventArgs NamePropertyChangeArgs = new PropertyChangedEventArgs("Name");
        private static readonly PropertyChangedEventArgs DescriptionPropertyChangeArgs = new PropertyChangedEventArgs("Description");

        private RelayCommand cancelEditCommand;

        public FieldViewModel([NotNull] string name)
        {
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

            Name = new DataWrapper<string>(this, NamePropertyChangeArgs) { DataValue = name };
            Name.AddRule(new SimpleRule("DataValue", "Pon algo, coño", o => string.IsNullOrEmpty(((DataWrapper<string>)o).DataValue)));
            Name.PropertyChanged += OnPropertyChanged;

            Description = new DataWrapper<string>(this, DescriptionPropertyChangeArgs) { DataValue = string.Empty };
            Description.AddRule(new SimpleRule("DataValue", "Pon algo, coño", o => string.IsNullOrEmpty(((DataWrapper<string>)o).DataValue)));
            Description.PropertyChanged += OnPropertyChanged;
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
    }
}