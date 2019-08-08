namespace MedicalExaminer.API.Models.v1.Users
{
    /// <inheritdoc />
    /// <summary>
    ///     Response for Post User.
    /// </summary>
    public class PostUserResponse : ResponseBase
    {
        /// <summary>
        ///     Identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     The User's email address.
        /// </summary>
        public string Email { get; set; }
    }
}