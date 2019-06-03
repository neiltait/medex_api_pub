namespace MedicalExaminer.API.Authorization
{
    /// <summary>
    /// Custom Claim Types used by MedicalExaminer.
    /// </summary>
    public static class MEClaimTypes
    {
        /// <summary>
        /// The Okta User Id.
        /// </summary>
        public const string OktaUserId = "uid";

        /// <summary>
        /// The User Id in the Cosmos Database.
        /// </summary>
        public const string UserId = "userId";
    }
}
