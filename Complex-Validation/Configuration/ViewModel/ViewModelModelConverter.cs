namespace ComplexValidation.Configuration.ViewModel
{
    using System;
    using CinchExtended.Services.Interfaces;
    using Model;

    internal static class ViewModelModelConverter
    {
        public static LomoConfigViewModel ConvertToViewModel(LomoConfig lomoConfig, IOpenFileService openFileService)
        {
            var viewModel = new LomoConfigViewModel(lomoConfig.Name, openFileService)
            {
                Id = lomoConfig.Id,
                BoxCount = { DataValue = lomoConfig.BoxCount },
                SelectedCustomer = { DataValue = lomoConfig.Customer != null ? ConvertToViewModel(lomoConfig.Customer) : null },
                Description = { DataValue = lomoConfig.Description },
                ImagePath = { DataValue = lomoConfig.ImagePath }
            };

            return viewModel;
        }

        private static CustomerViewModel ConvertToViewModel(Customer lomoConfigViewModel)
        {
            if (lomoConfigViewModel == null)
            {
                throw new ArgumentNullException("lomoConfigViewModel");
            }

            return new CustomerViewModel
            {
                Id = lomoConfigViewModel.Id,
                Name = lomoConfigViewModel.Name
            };
        }
    }
}