using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ClinicalGovernanceReview
    {
        Unknown = 1,
        Yes = 2,
        No = 3
    }
}
