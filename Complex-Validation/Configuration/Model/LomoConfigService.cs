namespace ComplexValidation.Configuration.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class LomoConfigService : ILomoConfigService
    {
        public IList<LomoConfig> LomoConfigs { get; private set; }

        public LomoConfigService()
        {
            LomoConfigs = new List<LomoConfig>
            {
                new LomoConfig
                {
                    Id  = 1,
                    Name = "Configuración 1",
                    Customer = new Customer { Id = 1, Name = "Cliente 1"},
                    BoxCount = 10,
                    ImagePath = "C:\\Windows",
                    Fields = GetSampleFields(),
                    Description = string.Empty,
                },
                new LomoConfig
                {
                    Id  = 2,
                    Name = "Configuración 2",
                    Customer = new Customer { Id = 1, Name = "Cliente 2"},
                    BoxCount = 10,
                    ImagePath = "C:\\Windows",
                    Fields = GetSampleFields(),
                    Description = string.Empty,
                },
                new LomoConfig
                {
                    Id  = 3,
                    Name = "Configuración 3",
                    Customer = new Customer { Id = 1, Name = "Cliente 3"},
                    BoxCount = 10,
                    ImagePath = "C:\\Windows",
                    Fields = GetSampleFields(),
                    Description = string.Empty,
                },
            };
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

        public int Add(LomoConfig lomoConfig)
        {
            var newId = GenerateNewId();
            LomoConfigs.Add(lomoConfig);
            lomoConfig.Id = newId;
            return newId;
        }

        private int GenerateNewId()
        {
            return LomoConfigs
                .Where(lc => lc.Id.HasValue)
                .Max(lc => lc.Id.Value) + 1;
        }

        public void Update(LomoConfig lomoConfig)
        {
            var toReplace = LomoConfigs.Single(config => config.Id == lomoConfig.Id);
            var id = LomoConfigs.IndexOf(toReplace);
            if (id != -1)
            {
                LomoConfigs.RemoveAt(id);
                LomoConfigs.Insert(id - 1, lomoConfig);
            }
        }

        public void Delete(int lomoConfigId)
        {
            var toDelete = LomoConfigs.Single(config => config.Id == lomoConfigId);
            LomoConfigs.Remove(toDelete);
        }
    }
}