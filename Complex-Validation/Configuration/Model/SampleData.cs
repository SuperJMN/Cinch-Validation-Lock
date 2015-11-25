namespace TestListBoxCachonda.Configuration.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using ViewModel;

    public static class SampleData
    {
        public static IEnumerable<LomoConfigViewModel> Configs
        {
            get
            {
                return new[]
                {
                    new LomoConfigViewModel("Config1") {Id = 1, Fields = GetSampleFields()},
                    new LomoConfigViewModel("Config2") {Id = 2},
                    new LomoConfigViewModel("Config3") {Id = 3}
                };
            }
        }

        public static ObservableCollection<FieldViewModel> GetSampleFields()
        {
            return new ObservableCollection<FieldViewModel>
            {
                new FieldViewModel("Field1"),
                new FieldViewModel("Field2"),
                new FieldViewModel("Field3")
            };
        }
    }
}