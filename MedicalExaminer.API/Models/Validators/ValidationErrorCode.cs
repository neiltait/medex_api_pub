using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.API.Models.Validators
{
    /// <summary>
    /// Validation Error Code List
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ValidationErrorCode
    {
        /// <summary>
        /// Item already exists
        /// </summary>
        Exists,

        /// <summary>
        /// Item was not found
        /// </summary>
        NotFound,

        /// <summary>
        /// Item would have been duplicated
        /// </summary>
        Duplicated,

        /// <summary>
        /// Item is null
        /// </summary>
        IsNull,

        /// <summary>
        /// Item is null or empty
        /// </summary>
        IsNullOrEmpty,

        /// <summary>
        ///  Item is empty
        /// </summary>
        IsEmpty,

        /// <summary>
        /// Item is invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Item is set to its default value
        /// </summary>
        IsDefaultValue,

        /// <summary>
        /// Maximum length exceeded
        /// </summary>
        ExceedsMaximumLength
    }

}
