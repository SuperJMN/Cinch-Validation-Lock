namespace ComplexValidation.Configuration.Model.RealPersistence
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class RealLomoConfigService : ILomoConfigService
    {
        private readonly IConfigRepository configRepository;
        private readonly IList<LomoConfig> lomoConfigs;

        public RealLomoConfigService(IConfigRepository configRepository, ICustomerRepository customerRepository)
        {
            this.configRepository = configRepository;
        }


        public IEnumerable<LomoConfig> LomoConfigs
        {
            get
            {
                return new ReadOnlyCollection<LomoConfig>(configRepository.GetAll().ToList());
            }
        }

        public int Add(LomoConfig lomoConfig)
        {
            if (lomoConfig.Id.HasValue)
            {
                throw new InvalidOperationException("The entity should not explicitly choose its Id. It should be set to null.");
            }

            var newId = GenerateNewId();
            lomoConfigs.Add(lomoConfig);
            lomoConfig.Id = newId;
            return newId;
        }

        private int GenerateNewId()
        {
            return LomoConfigs
                .Where(lc => lc.Id.HasValue)
                .Select(config => config.Id.Value)
                .DefaultIfEmpty(0)
                .Max(lc => lc) + 1;
        }

        public void Update(LomoConfig lomoConfig)
        {
            var toReplace = LomoConfigs.Single(config => config.Id == lomoConfig.Id);
            var id = lomoConfigs.IndexOf(toReplace);
            if (id != -1)
            {
                lomoConfigs.RemoveAt(id);
                lomoConfigs.Insert(id, lomoConfig);
            }
        }

        public void Delete(int lomoConfigId)
        {
            var toDelete = LomoConfigs.Single(config => config.Id == lomoConfigId);
            lomoConfigs.Remove(toDelete);
        }

        public LomoConfig Get(int id)
        {
            return lomoConfigs.Single(config => config.Id == id);
        }
    }
}