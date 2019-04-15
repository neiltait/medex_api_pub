using System.Collections.Generic;

namespace MedicalExaminer.API.Models.v1.Permissions
{
    /// <inheritdoc />
    /// <summary>
    ///     Response object for a list of Permissions.
    /// </summary>
    public class GetPermissionsResponse : ResponseBase
    {
        /// <summary>
        ///     List of Permissions.
        /// </summary>
        public IEnumerable<PermissionItem> Permissions { get; set; }
    }
}