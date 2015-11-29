namespace ComplexValidation.Configuration.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using CinchExtended.Services.Interfaces;
    using Model;
    using Model.RealPersistence;

    internal static class ViewModelModelConverter
    {
        public static LomoConfigViewModel ConvertToViewModel(LomoConfig lomoConfig, ICustomerRepository customerRepository, IOpenFileService openFileService)
        {
            var viewModel = new LomoConfigViewModel(lomoConfig.Name, openFileService, customerRepository)
            {
                Id = lomoConfig.Id,
                BoxCount = { DataValue = lomoConfig.BoxCount },
                SelectedCustomer = { DataValue = lomoConfig.Customer != null ? ConvertToViewModel(lomoConfig.Customer) : null },
                Description = { DataValue = lomoConfig.Description },
                ImagePath = { DataValue = lomoConfig.ImagePath },
                Fields = ConvertToViewModel(lomoConfig.Fields),
            };

            return viewModel;
        }

        private static ObservableCollection<FieldViewModel> ConvertToViewModel(IEnumerable<Field> fields)
        {
            var viewModels = new List<FieldViewModel>();
            foreach (var field in fields)
            {
                viewModels.Add(ConvertToViewModel(field));
            }
            return new ObservableCollection<FieldViewModel>(viewModels);
        }

        private static FieldViewModel ConvertToViewModel(Field field)
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