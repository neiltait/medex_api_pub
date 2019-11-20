namespace MedicalExaminer.API.Models.v1.Users
{
    /// <summary>
    /// Get Profile Response.
    /// </summary>
    /// <inheritdoc />
    public class GetProfileResponse : ResponseBase
    {
        /// <summary>
        /// The User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// GMC Number.
        /// </summary>
        public string GmcNumber { get; set; }
    }
}
