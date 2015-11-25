namespace TestListBoxCachonda.Configuration.Model
{
    using System.Collections.Generic;
    using ViewModel;

    public interface ILomoConfigRepository
    {
        IEnumerable<LomoConfigViewModel> Configs { get; }
    }
}