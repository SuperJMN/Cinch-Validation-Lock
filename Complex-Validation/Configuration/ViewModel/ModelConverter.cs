namespace ComplexValidation.Configuration.ViewModel
{
    using Model;

    internal static class ModelConverter
    {
        public static LomoConfig ConvertToModel(LomoConfigViewModel lomoConfigViewModel)
        {
            var converted = new LomoConfig
            {
                Id = lomoConfigViewModel.Id,
                BoxCount = lomoConfigViewModel.BoxCount.DataValue.Value,
                Customer = ConvertToModel(lomoConfigViewModel.SelectedCustomer.DataValue),
                Description = lomoConfigViewModel.Description.DataValue,
                ImagePath = lomoConfigViewModel.ImagePath.DataValue,
                Name = lomoConfigViewModel.Name.DataValue
            };

            return converted;
        }

        private static Customer ConvertToModel(CustomerViewModel lomoConfigViewModel)
        {
            return new Customer
            {
                Id = lomoConfigViewModel.Id,
                Name = lomoConfigViewModel.Name
            };
        }
    }
}