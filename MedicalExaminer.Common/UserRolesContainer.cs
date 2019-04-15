using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common
{
    /// <summary>
    ///     Container for a UserRoles enum
    /// </summary>
    /// <remarks>Used to filter results by user role</remarks>
    internal class UserRolesContainer
    {
        internal UserRoles Role { get; set; }
    }
}