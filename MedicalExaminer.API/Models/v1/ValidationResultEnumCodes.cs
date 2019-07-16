using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1
{
    public class ValidationResultEnumCodes : ValidationResult
    {
        public ValidationResultEnumCodes(SystemValidationErrors error)
            : base(error.ToString())
        {
            
        }
    }
}
