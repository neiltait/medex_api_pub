using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ModeOfDisposal
    {
        /// <summary>
        /// 
        /// </summary>
        Cremation,
        
        /// <summary>
        /// 
        /// </summary>
        Burial
    }
}