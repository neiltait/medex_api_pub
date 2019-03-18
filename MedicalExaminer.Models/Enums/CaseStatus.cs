using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CaseStatus
    {
        AdmissionNotesHaveBeenAdded,
        ReadyForMEScrutiny,
        Assigned,
        HaveBeenScrutinisedByME,
        PendingAdmissionNotes,
        PendingDiscussionWithQAP,
        PendingDiscussionWithRepresentative,
        HaveFinalCaseOutstandingOutcomes
    }
}
