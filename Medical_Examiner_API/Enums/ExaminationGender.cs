using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    /// <summary>
    /// Gender options for examination 
    /// </summary>
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