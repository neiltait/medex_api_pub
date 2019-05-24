using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]

    public enum CremationFormStatus
    {
        No,
        Yes,
    }
}
