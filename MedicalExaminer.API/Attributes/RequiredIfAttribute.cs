using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// Custom Validation attribute - 
    /// </summary>
    public class RequiredIfAttribute : RequiredAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="desiredValue">The desired valuer of the property</param>
        public RequiredIfAttribute(string propertyName, object desiredValue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredValue;
        }

        private string PropertyName { get; }

        private object DesiredValue { get; }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();
            var propertyValue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (propertyValue.ToString() != DesiredValue.ToString())
            {
                return ValidationResult.Success;
            }

            var result = base.IsValid(value, context);
            return result;
        }
    }
}