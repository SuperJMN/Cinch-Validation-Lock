namespace ComplexValidation.Configuration.ViewModel
{
    public class CustomerViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }

        protected bool Equals(CustomerViewModel other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((CustomerViewModel) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}