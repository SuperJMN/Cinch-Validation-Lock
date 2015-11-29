namespace ComplexValidation.Configuration.Model
{
    public class Field
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public FieldType FieldType { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public string Mask { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public string ValidChars { get; set; }
        public int Angle { get; set; }
        public decimal SpineCode { get; set; }
        public string Description { get; set; }
        public string FixedValue { get; set; }
        public decimal Left { get; set; }
        public decimal Top { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }

        protected bool Equals(Field other)
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
            return Equals((Field) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}