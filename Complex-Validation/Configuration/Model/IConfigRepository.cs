namespace ComplexValidation.Configuration.Model
{
    using System.Collections.Generic;

    public interface IConfigRepository
    {
        IEnumerable<LomoConfig> GetAll();
        int Create(LomoConfig lomoConfig);
        LomoConfig Get(int id);
        void Remove(int id);
        void Update(LomoConfig newLomoConfig);
    }
}