using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    /// <summary>
    /// Data type used to define the coroner status of an examination
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CoronerStatus
    {
        /// <summary>
        /// No Coroner Status
        /// </summary>
        None,

        /// <summary>
        /// Pending Send to coroners
        /// </summary>
        PendingSend,

        /// <summary>
        /// Send and awaiting confirmation
        /// </summary>
        SentAwaitingConfirm,

        /// <summary>
        /// Confirmation send from coroners 
        /// </summary>
        SentConfirmed
    }
}