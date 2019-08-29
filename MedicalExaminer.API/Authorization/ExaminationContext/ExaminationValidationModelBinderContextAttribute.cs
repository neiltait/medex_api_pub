using System;

namespace MedicalExaminer.API.Authorization.ExaminationContext
{
    /// <summary>
    /// Examination Validation Model Binding Context Attribute
    /// </summary>
    public class ExaminationValidationModelBinderContextAttribute : Attribute
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationValidationModelBinderContextAttribute"/>.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        public ExaminationValidationModelBinderContextAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        /// Name of the parameter to get the examination Id from.
        /// </summary>
        public string ParameterName { get; }
    }
}
