namespace TestListBoxCachonda.Configuration
{
    using System.Collections.Generic;

    public interface ILomoFieldsRepository
    {
        IEnumerable<FieldViewModel> GetFields(int lomoConfigId);
    }
}