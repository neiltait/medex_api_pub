using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ClinicalGovernanceReview
    {
        Yes = 1,
        No = 2,
        Unknown = 3
    }
}
