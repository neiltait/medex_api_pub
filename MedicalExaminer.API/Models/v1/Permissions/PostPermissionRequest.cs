namespace MedicalExaminer.API.Models.v1.Permissions
{
    /// <summary>
    ///     Post Permission Request.
    /// </summary>
    public class PostPermissionRequest
    {
        /// <summary>
        ///     Gets or sets the User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets the location ID
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        ///     Gets or sets the User Role for the Permission
        /// </summary>
        public int UserRole { get; set; }
    }
}