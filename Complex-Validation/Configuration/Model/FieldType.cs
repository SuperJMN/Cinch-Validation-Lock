namespace ComplexValidation.Configuration.Model
{
    using System.ComponentModel;

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