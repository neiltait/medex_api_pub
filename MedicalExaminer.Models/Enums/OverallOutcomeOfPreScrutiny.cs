using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OverallOutcomeOfPreScrutiny
    {
        IssueAnMccd = 1,
        ReferToCoroner = 2
    }
}
