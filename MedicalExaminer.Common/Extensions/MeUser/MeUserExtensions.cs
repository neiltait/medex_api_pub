namespace MedicalExaminer.Common.Extensions.MeUser
{
    /// <summary>
    /// MeUser Extensions.
    /// </summary>
    public static class MeUserExtensions
    {
        /// <summary>
        /// Get the full name, combining last name and first name
        /// </summary>
        /// <param name="meUser">User object.</param>
        /// <returns>Full name string.</returns>
        public static string FullName(this MedicalExaminer.Models.MeUser meUser)
        {
            // MVP: Just concat
            return $"{meUser.FirstName} {meUser.LastName}";
        }
    }
}
