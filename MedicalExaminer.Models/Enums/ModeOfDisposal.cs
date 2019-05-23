using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]

    public enum ModeOfDisposal
    {
        Unknown = 0,
        Cremation = 1,
        Burial = 2,
        BuriedAtSea = 3,
        Repatriation = 4,
    }
}