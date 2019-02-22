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
        /// Male Gender
        /// </summary>
        Male,
        
        /// <summary>
        /// Female Gender
        /// </summary>
        Female
        // More to come 
    }
}