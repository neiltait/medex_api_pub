using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AnalysisEntryType
    {
        /// <summary>
        /// Defines the analysis type used and entered by an MEO to enter summary information 
        /// </summary>
        MEOSummary = 0,
        
        /// <summary>
        /// 
        /// </summary>
        QAPInformation = 1,
        
        /// <summary>
        /// 
        /// </summary>
        MEScrutiny = 2,
        
        /// <summary>
        /// 
        /// </summary>
        CoronerInformation = 3,
        
        /// <summary>
        /// 
        /// </summary>
        OtherInformation = 4
    }
}