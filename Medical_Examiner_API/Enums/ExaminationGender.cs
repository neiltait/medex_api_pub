using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExaminationGender
    {
        /// <summary>
        /// 
        /// </summary>
        Male,
        
        /// <summary>
        /// 
        /// </summary>
        Female
        // More to come 
    }
}