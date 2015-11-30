namespace ComplexValidation.Configuration.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Cinch;
    using Model;
    using Model.RealPersistence;

    internal static class ModelToViewModelConverter
    {
        public static LomoConfigViewModel Convert(LomoConfig lomoConfig, ICustomerRepository customerRepository, IOpenFileService openFileService)
        {
            var viewModel = new LomoConfigViewModel(lomoConfig.Name, openFileService, customerRepository)
            {
                Id = lomoConfig.Id,
                BoxCount = { DataValue = lomoConfig.BoxCount },
                SelectedCustomer = { DataValue = lomoConfig.Customer != null ? Convert(lomoConfig.Customer) : null },
                Description = { DataValue = lomoConfig.Description },
                ImagePath = { DataValue = lomoConfig.ImagePath },
                Fields = new ObservableCollection<FieldViewModel>(lomoConfig.Fields.Select(Convert)),
            };

            return viewModel;
        }

        public static FieldViewModel Convert(Field field)
        {
            var name = field.Name;

            var viewModel = new FieldViewModel(name);
            viewModel.Id = field.Id;
            viewModel.SelectedAngle.DataValue = field.Angle;
            viewModel.Description.DataValue = field.Description;
            viewModel.IsActive.DataValue = field.IsActive;
            viewModel.IsRequired.DataValue = field.IsRequired;
            viewModel.Mask.DataValue = field.Mask;
            viewModel.FixedValue.DataValue = field.FixedValue;
            viewModel.ValidChars.DataValue = field.ValidChars;
            viewModel.SelectedFieldType.DataValue = field.FieldType;

            return viewModel;
        }

        private static CustomerViewModel Convert(Customer lomoConfigViewModel)
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