namespace ComplexValidation.Configuration.Model
{
    using System.Collections.Generic;

    public interface IDefaultFieldsForNewConfigsRepository
    {
        IEnumerable<Field> GetAll();
        int Create(Field field);
    }
}