using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ModeOfDisposal
    {
        Cremation = 0,
        Burial = 1
    }
}