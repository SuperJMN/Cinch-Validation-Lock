namespace ComplexValidation.Configuration.Model
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public interface ILomoConfigService
    {
        IList<LomoConfig> LomoConfigs { get; }
        int Add(LomoConfig lomoConfig);
        void Update(LomoConfig lomoConfig);
        void Delete(int lomoConfigId);
    }
}