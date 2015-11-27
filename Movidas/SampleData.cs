namespace ComplexValidation.Tests
{
    using System.Collections.Generic;
    using Configuration.Model;

    public static class SampleData
    {
        public static IEnumerable<LomoConfig> UniqueConfigThatIsValid
        {
            get
            {
                return new List<LomoConfig>
                {
                    new LomoConfig
                    {
                        Name = "Hola",
                        Id = 1,
                        Customer = new Customer("CustomerName", 1),
                        ImagePath = "C:\\",
                        BoxCount = 1,
                    }
                };
            }
        }

        public static IEnumerable<LomoConfig> UnsavedUniqueConfigThatIsValid
        {
            get
            {
                return new List<LomoConfig>
                {
                    new LomoConfig
                    {
                        Name = "Hola",
                        Id = null,
                        Customer = new Customer("CustomerName", 1),
                        ImagePath = "C:\\",
                        BoxCount = 1,
                    }
                };
            }
        }

        public static IEnumerable<LomoConfig> WithNameAndNoSelectedCustomer
        {
            get
            {
                return new List<LomoConfig>() { new LomoConfig()
                {
                    Name = "Hola",
                    Id = 1,
                } };
            }
        }
    }
}