namespace ComplexValidation.Configuration.Model.RealPersistence
{
    public interface ICustomerRepository
    {
        Customer Get(int id);
    }
}