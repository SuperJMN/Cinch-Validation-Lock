namespace ComplexValidation.Configuration.Model.RealPersistence
{
    using System.Collections.Generic;

    public interface ICustomerRepository
    {
        Customer Get(int id);
        IEnumerable<Customer> GetAll();
    }
}