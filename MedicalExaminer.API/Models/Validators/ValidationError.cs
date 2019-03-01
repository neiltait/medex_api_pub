using System;
using System.Linq.Expressions;

namespace MedicalExaminer.API.Models.Validators
{
    /// <summary>
    /// Validation Error object, returned when there are errors validating dto
    /// </summary>
    public class ValidationError : Error
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        public ValidationError()
            : this(ValidationErrorCode.Invalid, (string)null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="code">The <see cref="ValidationErrorCode"/>.</param>
        public ValidationError(ValidationErrorCode code)
            : this(code, (string)null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="code">The <see cref="ValidationErrorCode"/>.</param>
        /// <param name="message">The message for the validation error.</param>
        public ValidationError(ValidationErrorCode code, string message)
            : base(message)
        {
            this.Code = code.ToString();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="code">The validation error code</param>
        /// <param name="property">The property that failed validation</param>
        /// <param name="message">The validation message</param>
        public ValidationError(ValidationErrorCode code, string property, string message)
            : base(message)
        {
            this.Code = code.ToString();
            this.Property = property;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="property">The property.</param>
        /// <param name="message">The message.</param>
        public ValidationError(string code, string property = null, string message = null)
            : base(message)
        {
            this.Code = code;
        }

        /// <summary>
        /// Gets or sets the <see cref="ValidationErrorCode"/>
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string Property { get; set; }
    }
}
