namespace TestListBoxCachonda.Configuration
{
    using System.Collections.Generic;

    public interface ILomoConfigRepository
    {
        IEnumerable<LomoConfigViewModel> Configs { get; }
    }
}