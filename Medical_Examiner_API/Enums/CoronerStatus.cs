using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Medical_Examiner_API.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CoronerStatus
    {
        None = 0,
        PendingSend = 1,
        SentAwaitingConfirm = 2,
        SentConfirmed = 3
    }
}