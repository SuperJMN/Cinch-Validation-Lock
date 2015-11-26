namespace ComplexValidation.Configuration.Model
{
    using System.Collections.Generic;

    public class LomoConfig
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int BoxCount { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<Field> Fields { get; set; }
    }
}