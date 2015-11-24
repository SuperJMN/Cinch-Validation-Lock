namespace TestListBoxCachonda.Configuration
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using CinchExtended.BusinessObjects;
    using CinchExtended.ViewModels;
    using GalaSoft.MvvmLight.Command;

    public class LomoConfigViewModel : ValidatingViewModelBase, ICloneable
    {
        public static readonly PropertyChangedEventArgs NameChangedArgs = new PropertyChangedEventArgs("Name");
        private RelayCommand addFieldCommand;
        private FieldViewModel selectedField;

        private LomoConfigViewModel(LomoConfigViewModel lomoConfigViewModel)
        {
            Name = new DataWrapper<string>(this, NameChangedArgs);
            Name.DataValue = string.Copy(lomoConfigViewModel.Name.DataValue);
            Fields = CloneFields(lomoConfigViewModel.Fields);
        }

        public LomoConfigViewModel(string name)
        {
            Name = new DataWrapper<string>(this, NameChangedArgs);
            Name.DataValue = name;
            Fields = new ObservableCollection<FieldViewModel>();
        }

        public DataWrapper<string> Name { get; set; }
        public int Id { get; set; }
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
    }
}