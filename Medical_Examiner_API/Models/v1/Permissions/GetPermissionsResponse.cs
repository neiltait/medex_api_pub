using System.Collections.Generic;

namespace Medical_Examiner_API.Models.V1.Users
{
    /// <inheritdoc />
    /// <summary>
    /// Response object for a list of users.
    /// </summary>
    public class GetPermissionsResponse : ResponseBase
    {
        /// <summary>
        /// List of Permissions.
        /// </summary>
        public IEnumerable<PermissionItem> Permissions { get; set; }
    }
}
