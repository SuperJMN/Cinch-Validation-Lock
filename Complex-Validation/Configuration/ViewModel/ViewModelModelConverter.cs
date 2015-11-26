namespace ComplexValidation.Configuration.ViewModel
{
    using CinchExtended.Services.Interfaces;
    using Model;

    internal static class ViewModelModelConverter
    {
        public static LomoConfigViewModel ConvertToViewModel(LomoConfig lomoConfig, IOpenFileService openFileService)
        {
            var viewModel = new LomoConfigViewModel(lomoConfig.Name, openFileService);
            viewModel.Id = lomoConfig.Id;
            viewModel.BoxCount.DataValue = lomoConfig.BoxCount;
            viewModel.SelectedCustomer.DataValue = ConvertToViewModel(lomoConfig.Customer);
            viewModel.Description.DataValue = lomoConfig.Description;
            viewModel.ImagePath.DataValue = lomoConfig.ImagePath;
            
            return viewModel;
        }

        private static CustomerViewModel ConvertToViewModel(Customer lomoConfigViewModel)
        {            
            return new CustomerViewModel
            {
                Id = lomoConfigViewModel.Id,
                Name = lomoConfigViewModel.Name
            };
        }
    }
}