using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SystemValidationErrors
    {
        DuplicateNhsNumber,
        ContainsWhitespace,
        ContainsInvalidCharacters,
        InvalidNhsNumber,
        Required,
        MaximumLength150,
        MinimumLengthOf1,
        InvalidDateFormat,
        EndDateNotFound,
        EndDateBeforeStartDate,
        RequiredIfOtherSelected,
        RequiredIfAnyImplants,
        LocationIdMustBeProvided,
        LocationIdNotFound,
    }
}
