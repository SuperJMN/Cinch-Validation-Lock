namespace ComplexValidation.Tests
{
    using System.Collections.Generic;
    using ComplexValidation.Configuration.Model;

    public class LomoConfigServiceMock : ILomoConfigService
    {
        private readonly IEnumerable<LomoConfig> lomoConfigs;

        public LomoConfigServiceMock(IEnumerable<LomoConfig> lomoConfigs)
        {
            this.lomoConfigs = lomoConfigs;
        }

        public IEnumerable<LomoConfig> LomoConfigs
        {
            get { return lomoConfigs; }
        }

        public int Add(LomoConfig lomoConfig)
        {
            throw new System.NotImplementedException();
        }

        public void Update(LomoConfig lomoConfig)
        {
            lomoConfig.Id = 1;
        }

        public void Delete(int lomoConfigId)
        {
            throw new System.NotImplementedException();
        }

        public LomoConfig Get(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
