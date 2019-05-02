using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]

    public enum ModeOfDisposal
    {
        Cremation = 0,
        Burial = 1,
        BuriedAtSea = 2,
        Repatriation = 3
    }
}