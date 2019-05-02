using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]

    public enum LocationType
    {
        Site = 0,
        Trust = 1,
        Region = 2,
        National = 3
    }
}