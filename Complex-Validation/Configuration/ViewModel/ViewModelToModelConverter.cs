namespace ComplexValidation.Configuration.ViewModel
{
    using Model;

    internal static class ViewModelToModelConverter
    {
        public static LomoConfig Convert(LomoConfigViewModel lomoConfigViewModel)
        {
            var converted = new LomoConfig
            {
                Id = lomoConfigViewModel.Id,
                BoxCount = lomoConfigViewModel.BoxCount.DataValue.Value,
                Customer = Convert(lomoConfigViewModel.SelectedCustomer.DataValue),
                Description = lomoConfigViewModel.Description.DataValue,
                ImagePath = lomoConfigViewModel.ImagePath.DataValue,
                Name = lomoConfigViewModel.Name.DataValue
            };

            return converted;
        }

        private static Customer Convert(CustomerViewModel lomoConfigViewModel)
        {
            return new Customer
            {
                Id = lomoConfigViewModel.Id,
                Name = lomoConfigViewModel.Name
            };
        }
    }
}