using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SystemValidationErrors
    {
        Duplicate,
        ContainsWhitespace,
        ContainsInvalidCharacters,
        Invalid,
        Required,
        MaximumLength150,
        MinimumLengthOf1,
        InvalidFormat,
        EndDateNotFound,
        EndDateBeforeStartDate,
        IdNotFound,
    }
}
