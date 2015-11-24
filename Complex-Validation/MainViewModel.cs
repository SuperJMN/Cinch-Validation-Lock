namespace TestListBoxCachonda
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using Annotations;
    using GalaSoft.MvvmLight.CommandWpf;

    public class MainViewModel : INotifyPropertyChanged
    {
        private bool canSelectItem;

        public MainViewModel()
        {
            Items = new[] {"Hola", "cómo", "estás"};
            PromptCommand = new RelayCommand(Execute, () => HasPendingChanges);
        }

        private void Execute()
        {
            CanSelectItem = MessageBox.Show("Pepito", "Título", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        public bool HasPendingChanges { get; set; }

        public bool CanSelectItem
        {
            get { return canSelectItem; }
            set
            {
                if (value == canSelectItem)
                {
                    return;
                }
                canSelectItem = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> Items { get; set; }

        public string SelectedItem { get; set; }

        public ICommand PromptCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}