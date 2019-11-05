namespace MedicalExaminer.Models.Enums
{
    public enum OpenClosedCases
    {
        Open,
        ClosedOrVoid,

        // Fix to allow the original bool values to be convertible to the new enums and represent the same value.
        // @TODO: Remove once the CMS has implement the "void case" functionality and will use the enum values intead.
        True = Open,
        False = ClosedOrVoid,
    }
}
