using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    /// <summary>
    /// Data types used to identify the mode of disposal 
    /// </summary>
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