using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExaminationGender
    {
        Male = 1,
        Female = 2
        // More to come 
    }
}