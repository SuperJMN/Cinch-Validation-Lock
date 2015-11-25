namespace TestListBoxCachonda.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
        [Description("Global")]
        Global,
        [Description("Númérico")]
        Numeric,
        [Description("Alfabético")]
        Alphabetic,
        [Description("Alfanumérico")]
        Alphanumeric,
        [Description("OMR")]
        Omr,
        [Description("Barcode")]
        Barcode,
        Ean13,
        Ean8,
        Code39,
        Code128,
        Code2Of5,
        GeneralBarcode,
    }
}