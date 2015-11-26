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
    }
}