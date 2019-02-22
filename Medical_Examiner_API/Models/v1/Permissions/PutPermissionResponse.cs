namespace Medical_Examiner_API.Models.V1.Users
{
    /// <inheritdoc />
    /// <summary>
    /// Response for Put Permission.
    /// </summary>
    public class PutPermissionResponse : ResponseBase
    {
        /// <summary>
        /// Gets or sets the permission ID
        /// </summary>
        public string PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the location ID
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// Gets or sets the User Role for the Permission
        /// </summary>
        public int UserRole { get; set; }
    }
}
