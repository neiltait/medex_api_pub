using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.Users
{
    /// <summary>
    /// Put User Request.
    /// </summary>
    public class PutUserRequest
    {
        /// <summary>
        /// The User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The User's email address.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }
    }
}