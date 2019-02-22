using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    /// <summary>
    /// Defines the analysis type used and entered by an MEO to enter summary information
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AnalysisEntryType
    {
        /// <summary>
        /// Medical Examiner Officer summary
        /// </summary>
        MEOSummary = 0,

        /// <summary>
        /// Qualified Acting Professional Summary
        /// </summary>
        QAPInformation = 1,

        /// <summary>
        /// Medical Examiner Scrutiny
        /// </summary>
        MEScrutiny = 2,

        /// <summary>
        /// Coroner Information
        /// </summary>
        CoronerInformation = 3,

        /// <summary>
        /// Other Information
        /// </summary>
        OtherInformation = 4
    }
}