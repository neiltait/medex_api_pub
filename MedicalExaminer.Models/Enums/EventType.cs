using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventType
    {
        Other,
        PreScrutiny,
        BereavedDiscussion,
        MedicalHistory,
        MeoSummary,
        QapDiscussion,
        Admission,
        PatientDied
    }
}
