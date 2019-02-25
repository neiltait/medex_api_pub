namespace Medical_Examiner_API.Models.V1.Users
{
    /// <inheritdoc />
    /// <summary>
    /// Response for Post User.
    /// </summary>
    public class PostUserResponse : ResponseBase
    {
        /// <summary>
        /// Identifier.
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
    }
}
