using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;
using MedicalExaminer.Models.Enums;
using Microsoft.CodeAnalysis.Operations;

namespace MedicalExaminer.API.Models.v1.Permissions
{
    /// <summary>
    ///     Post Permission Request.
    /// </summary>
    public class PostPermissionRequest : IUserRequest
    {
        /// <summary>
        ///     Gets or sets the User identifier.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets the location ID.
        /// </summary>
        [Required]
        public string LocationId { get; set; }

        /// <summary>
        ///     Gets or sets the User Role for the Permission.
        /// </summary>
        [ValidRolePerUser(nameof(UserId))]
        public UserRoles UserRole { get; set; }
    }
}