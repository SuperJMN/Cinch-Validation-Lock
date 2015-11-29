namespace ComplexValidation.Configuration.Model.InMemory
{
    using System.Collections.Generic;

    public class InMemoryDefaultFieldsForNewConfigsRepository : IDefaultFieldsForNewConfigsRepository
    {
        public IEnumerable<Field> GetAll()
        {
            return new[]
            {
                new Field { Id = 1, Name = "Field 1"},
                new Field { Id = 2, Name = "Field 2"},
                new Field { Id = 3, Name = "Field 3"},
                new Field { Id = 4, Name = "Field 4"},
                new Field { Id = 5, Name = "Field 5"},
                new Field { Id = 6, Name = "Field 6"},
            };
        }
    }
}