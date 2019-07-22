using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Models.v1.Users
{
    /// <summary>
    /// Put User Request.
    /// </summary>
    public class PutUserRequest
    {
        /// <summary>
        /// The User's email address.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }
    }
}