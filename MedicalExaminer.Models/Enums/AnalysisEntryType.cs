using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AnalysisEntryType
    {
        MEOSummary = 0,
        BereavedInformation = 1,
        QAPInformation = 2,
        MEScrutiny = 3,
        CoronerInformation = 4,
        OtherInformation = 5
    }
}