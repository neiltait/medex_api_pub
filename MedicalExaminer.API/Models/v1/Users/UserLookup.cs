namespace MedicalExaminer.API.Models.v1.Users
{
    /// <summary>
    /// User Lookup
    /// </summary>
    /// <remarks>User fields when displayed in lookup lists.</remarks>
    public class UserLookup
    {
        /// <summary>
        /// The User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string FullName { get; set; }
    }
}
