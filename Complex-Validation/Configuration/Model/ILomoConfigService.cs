namespace ComplexValidation.Configuration.Model
{
    using System.Collections.Generic;

    public interface ILomoConfigService
    {
        IEnumerable<LomoConfig> LomoConfigs { get; }
        int Add(LomoConfig lomoConfig);
        void Update(LomoConfig lomoConfig);
        void Delete(int lomoConfigId);
        LomoConfig Get(int id);
    }
}