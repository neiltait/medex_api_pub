using System.ComponentModel;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// User Roles.
    /// </summary>
    public enum UserRoles
    {
        /// <summary>
        /// Medical Examiner Officer.
        /// </summary>
        [Description("Medical Examiner Officer")]
        MedicalExaminerOfficer = 0,

        /// <summary>
        /// Medical Examiner.
        /// </summary>
        [Description("Medical Examiner")]
        MedicalExaminer = 1,

        /// <summary>
        /// Service Administrator.
        /// </summary>
        [Description("Service Administrator")]
        ServiceAdministrator = 2,

        /// <summary>
        /// Service Owner.
        /// </summary>
        [Description("Service Owner")]
        ServiceOwner = 3
    }
}
