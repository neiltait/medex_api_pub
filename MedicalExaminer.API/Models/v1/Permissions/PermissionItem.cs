namespace MedicalExaminer.API.Models.v1.Permissions
{
    /// <summary>
    ///     A user item as part of multiple permission responses.
    /// </summary>
    public class PermissionItem
    {
        /// <summary>
        ///     Gets or sets the permission ID.
        /// </summary>
        public string PermissionId { get; set; }

        /// <summary>
        ///     Gets or sets the User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets the location ID.
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        ///     Gets or sets the User Role for the Permission.
        /// </summary>
        public int UserRole { get; set; }
    }
}