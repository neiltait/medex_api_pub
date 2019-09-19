using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusBarResult
    {
        Complete,
        Incomplete,
        NotApplicable,
        Unknown
    }
}
