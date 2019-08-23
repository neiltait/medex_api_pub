using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]

    public enum QapDiscussionOutcome
    {
        MccdCauseOfDeathProvidedByQAP,
        MccdCauseOfDeathProvidedByME,
        MccdCauseOfDeathAgreedByQAPandME,
        ReferToCoronerFor100a,
        ReferToCoronerInvestigation,
        DiscussionUnableToHappen
    }
}
