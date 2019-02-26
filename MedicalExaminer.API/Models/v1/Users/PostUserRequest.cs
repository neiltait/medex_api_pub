using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Models.v1.Users
{
    /// <summary>
    /// Post User Request.
    /// </summary>
    public class PostUserRequest
    {
        /// <summary>
        /// The User's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The User's email address
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }
    }
}
