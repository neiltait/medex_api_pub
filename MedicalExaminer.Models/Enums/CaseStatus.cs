using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CaseStatus
    {
        HaveUnknownBasicDetails,
        ReadyForMEScrutiny,
        Unassigned,
        HaveBeenScrutinisedByME,
        PendingAdditionalDetails,
        PendingDiscussionWithQAP,
        PendingDiscussionWithRepresentative,
        HaveFinalCaseOutstandingOutcomes
    }
}