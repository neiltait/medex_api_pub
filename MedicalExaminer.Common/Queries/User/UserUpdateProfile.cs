namespace MedicalExaminer.Common.Queries.User
{
    /// <summary>
    /// User Update Profile.
    /// </summary>
    public class UserUpdateProfile : IUserUpdate
    {
        /// <summary>
        /// User Id of user to update
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gmc Number.
        /// </summary>
        public string GmcNumber { get; set; }
    }
}
