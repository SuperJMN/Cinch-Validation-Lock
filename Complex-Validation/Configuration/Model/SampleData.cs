namespace ComplexValidation.Configuration.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class SampleData
    {       
        public static IEnumerable<LomoConfig> Configs
        {
            get
            {
                return new Collection<LomoConfig>()
                {
                    new LomoConfig
                    {
                        Id = 1,
                        Name = "Configuración 1",
                        Customer = new Customer {Id = 1, Name = "Cliente 1"},
                        BoxCount = 10,
                        ImagePath = "C:\\Windows",
                        Fields = GetSampleFields(),
                        Description = string.Empty,
                    },
                    new LomoConfig
                    {
                        Id = 2,
                        Name = "Configuración 2",
                        Customer = new Customer {Id = 1, Name = "Cliente 2"},
                        BoxCount = 10,
                        ImagePath = "C:\\Windows",
                        Fields = GetSampleFields(),
                        Description = string.Empty,
                    },
                    new LomoConfig
                    {
                        Id = 3,
                        Name = "Configuración 3",
                        Customer = new Customer {Id = 1, Name = "Cliente 3"},
                        BoxCount = 10,
                        ImagePath = "C:\\Windows",
                        Fields = GetSampleFields(),
                        Description = string.Empty,
                    },

                };
            }
        }

        private static IEnumerable<Field> GetSampleFields()
        {
            return new Collection<Field>
            {
                new Field { Id = 1, Name = "Campo 1"},
                new Field { Id = 2, Name = "Campo 2"},
                new Field { Id = 3, Name = "Campo 3"},
            };
        }
    }
}