namespace ComplexValidation.Configuration.Model
{
    using System.ComponentModel;

    public enum FieldType
    {
        [Description("Global")]
        Global,
        [Description("Numérico")]
        Numeric,
        [Description("Alfabético")]
        Alphabetic,
        [Description("Alfanumérico")]
        Alphanumeric,
        [Description("OMR")]
        Omr,
        [Description("Barcode")]
        Barcode,
        [Description("Ean13")]
        Ean13,
        [Description("Ean8")]
        Ean8,
        [Description("Code39")]
        Code39,
        [Description("Code128")]
        Code128,
        [Description("Code2Of5")]
        Code2Of5,
        [Description("General Barcode")]
        GeneralBarcode,
    }
}