using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExaminationGender
    {
        Male = 1,
        Female = 2,
        Other = 3
    }
}