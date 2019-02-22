using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    /// <summary />
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CoronerStatus
    {
        /// <summary>
        /// 
        /// </summary>
        None,

        /// <summary>
        /// 
        /// </summary>
        PendingSend,

        /// <summary>
        /// 
        /// </summary>
        SentAwaitingConfirm,

        /// <summary>
        /// 
        /// </summary>
        SentConfirmed
    }
}