using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.Users
{
    /// <inheritdoc />
    /// <summary>
    /// Response object for a single user.
    /// </summary>
    public class GetUserResponse : ResponseBase
    {
        /// <summary>
        /// The User identifier.
        /// </summary>
        public string UserId { get; set; }

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
        public string Email { get; set; }

        /// <summary>
        /// The User's Id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The User's role
        /// </summary>
        public UserRoles UserRole { get; set; }


    }
}
