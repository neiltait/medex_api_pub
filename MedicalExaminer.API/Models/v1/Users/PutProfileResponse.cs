namespace MedicalExaminer.API.Models.v1.Users
{
    /// <summary>
    /// Response for Put Profile.
    /// </summary>
    /// <inheritdoc />
    public class PutProfileResponse : ResponseBase
    {
        /// <summary>
        /// The User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The User's GMC Number.
        /// </summary>
        public string GmcNumber { get; set; }
    }
}