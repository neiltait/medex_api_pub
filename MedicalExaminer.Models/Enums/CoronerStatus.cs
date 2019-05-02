using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
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