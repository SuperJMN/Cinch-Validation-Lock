namespace ComplexValidation.Configuration.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class FixedData
    {
        static FixedData()
        {
            Angles = new[] { 0, 90, 180, 270, };
            FieldTypes = Enum.GetValues(typeof (FieldType)).Cast<FieldType>();
            Customers = new[]
            {
                new Customer { Id = 1, Name = "Cliente 1"},
                new Customer { Id = 1, Name = "Cliente 2"},
                new Customer { Id = 1, Name = "Cliente 3"},
                new Customer { Id = 1, Name = "Cliente 4"},
                new Customer { Id = 1, Name = "Cliente 5"},
                new Customer { Id = 1, Name = "Cliente 6"},
            };
        }
        public static IEnumerable<int> Angles { get; set; }
        public static IEnumerable<FieldType> FieldTypes { get; set; }
        public static IEnumerable<Customer> Customers { get; set; }
    }
}