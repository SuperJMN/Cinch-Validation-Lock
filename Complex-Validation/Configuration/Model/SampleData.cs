namespace ComplexValidation.Configuration.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using CinchExtended.Services.Interfaces;
    using ViewModel;

    public class SampleData
    {
        private readonly IOpenFileService fileOpenFileService;

        public SampleData(IOpenFileService fileOpenFileService)
        {
            this.fileOpenFileService = fileOpenFileService;
        }

        public IEnumerable<LomoConfigViewModel> Configs
        {
            get
            {
                return new[]
                {
                    new LomoConfigViewModel("Config1", fileOpenFileService) {Id = 1, Fields = GetSampleFields()},
                    new LomoConfigViewModel("Config2", fileOpenFileService) {Id = 2, Fields = GetSampleFields()} ,
                    new LomoConfigViewModel("Config3", fileOpenFileService) {Id = 3, Fields = GetSampleFields()}
                };
            }
        }

        public ObservableCollection<FieldViewModel> GetSampleFields()
        {
            return new ObservableCollection<FieldViewModel>
            {
                new FieldViewModel("Field1") { Id = 1},
                new FieldViewModel("Field2") { Id = 2},
                new FieldViewModel("Field3") { Id = 3},
            };
        }
    }
}