namespace ComplexValidation.Configuration.Model.InMemory
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class InMemoryDefaultFieldsRepository : IDefaultFieldsForNewConfigsRepository
    {
        private readonly IList<Field> fields = new Collection<Field>

        {
            new Field {Id = 1, Name = "Field 1"},
            new Field {Id = 2, Name = "Field 2"},
            new Field {Id = 3, Name = "Field 3"},
            new Field {Id = 4, Name = "Field 4"},
            new Field {Id = 5, Name = "Field 5"},
            new Field {Id = 6, Name = "Field 6"},
        };
        

        public IEnumerable<Field> GetAll()
        {
            return fields;
        }

        private int GenerateNewId()
        {
            return fields                
                .Select(config => config.Id)
                .DefaultIfEmpty(0)
                .Max(lc => lc) + 1;
        }

        public int Create(Field field)
        {
            var id = GenerateNewId();
            field.Id = id;

            fields.Add(field);

            return id;
        }
    }
}