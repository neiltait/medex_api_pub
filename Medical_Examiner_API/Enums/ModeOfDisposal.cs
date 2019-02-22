using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ModeOfDisposal
    {
        /// <summary>
        /// Cremation
        /// </summary>
        Cremation,
        
        /// <summary>
        /// Burial
        /// </summary>
        Burial
    }
}