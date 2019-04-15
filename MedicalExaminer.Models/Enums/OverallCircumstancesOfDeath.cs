using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OverallCircumstancesOfDeath
    {
        Expected = 1,
        Unexpected = 2,
        SuddenButNotUnexpected = 3,
        PartOfAnIndividualisedEndOfLifeCarePlan = 4
    }
}
