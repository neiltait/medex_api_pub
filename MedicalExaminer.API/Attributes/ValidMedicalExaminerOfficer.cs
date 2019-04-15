using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    ///     Validates that a user exists and is in the role of medical examiner officer.
    /// </summary>
    public class ValidMedicalExaminerOfficer : ValidUserBase
    {
        /// <summary>
        /// Instantiate a new instance of <see cref="ValidMedicalExaminerOfficer"/>.
        /// </summary>
        public ValidMedicalExaminerOfficer()
            : base(UserRoles.MedicalExaminerOfficer)
        {

        }
    }
}
