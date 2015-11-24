namespace TestListBoxCachonda.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class FixedData
    {
        static FixedData()
        {
            Angles = new[] { 0, 90, 180, 270, };
            FieldTypes = Enum.GetValues(typeof (FieldType)).Cast<FieldType>();
        }
        public static IEnumerable<int> Angles { get; set; }
        public static IEnumerable<FieldType> FieldTypes { get; set; }
    }

    public enum FieldType
    {
        One,
        Two,
        Three,
        Four,
    }
}