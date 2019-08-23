using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BereavedDiscussionOutcome
    {
        CauseOfDeathAccepted = 0,
        ConcernsCoronerInvestigation = 1,
        ConcernsRequires100a = 2,
        ConcernsAddressedWithoutCoroner = 3,
        DiscussionUnableToHappen = 4
    }
}