namespace ComplexValidation.Configuration.ViewModel
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using CinchExtended.BusinessObjects;
    using CinchExtended.ViewModels;
    using GalaSoft.MvvmLight.Command;

    public class FullyFledgedViewModel : EditableValidatingViewModelBase
    {
        private RelayCommand cancelCommand;
        private RelayCommand saveCommand;
        private ICommand scapeAttemptCommand;

        private IEnumerable<DataWrapperBase> AllDataWrappers
        {
            get { return DataWrapperHelper.GetWrapperProperties<ViewModelBase>(this); }
        }

        private IEnumerable<IChangeIndicator> EditableDataWrappers
        {
            get { return AllDataWrappers.Where(IsChangeIndicator).Cast<IChangeIndicator>(); }
        }

        public virtual bool IsDirty
        {
            get
            {
                var isDirty = EditableDataWrappers.Any(dw => dw.IsDirty);
                return isDirty;
            }
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
                        SaveCommand.RaiseCanExecuteChanged();
                        CancelEditCommand.RaiseCanExecuteChanged();
                    },
                    () => IsValid && IsDirty));
            }
        }

        public RelayCommand CancelEditCommand
        {
            get
            {
                return cancelCommand ?? (cancelCommand = new RelayCommand(
                    () =>
                    {

                        CancelEdit();
                        BeginEdit();
                    }, () => IsDirty));
            }
        }

        public int? Id { get; set; }

        public ICommand ScapeAttemptCommand
        {
            get { return scapeAttemptCommand ?? (scapeAttemptCommand = new RelayCommand(OnScapeAttempt)); }
        }

        public override bool IsValid
        {
            get { return AllDataWrappers.All(dw => dw.IsValid); }
        }

        protected void SubscribeToChangesInAllDataWrappers()
        {
            foreach (var dataWrapperBase in AllDataWrappers)
            {
                dataWrapperBase.PropertyChanged += OnPropertyChanged;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            NotifyPropertyChanged("IsDirty");
            NotifyPropertyChanged("IsValid");
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

        private void OnScapeAttempt()
        {
            MessageBox.Show(string.Format("Ha realizado cambios en {0}.\nPor favor, guárdelos o descártelos antes de continuar.", DisplayName), "Aviso");
        }
    }
}