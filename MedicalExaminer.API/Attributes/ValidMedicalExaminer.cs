using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Attributes
{
    /// <inheritdoc />
    /// <summary>
    ///     Validates that a user exists and is in the role of medical examiner.
    /// </summary>
    public class ValidMedicalExaminer : ValidUserBase
    {
        /// <summary>
        /// Instantiate a new instance of <see cref="ValidMedicalExaminer"/>.
        /// </summary>
        public ValidMedicalExaminer()
            : base(UserRoles.MedicalExaminer)
        {

        }
    }
}
