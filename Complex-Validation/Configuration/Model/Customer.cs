namespace ComplexValidation.Configuration.Model
{
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(string name, int? id)
        {
            Name = name;
            Id = id;
        }
        
        public string Name { get; set; }
        public int? Id { get; set; }
    }
}