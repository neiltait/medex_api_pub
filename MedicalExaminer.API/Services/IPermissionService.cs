using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Services
{
    /// <summary>
    /// Permission Service Interface
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Has Permission on Document
        /// </summary>
        /// <param name="emailAddress">Email Address.</param>
        /// <param name="document">Document.</param>
        /// <param name="permission">Permission.</param>
        /// <returns>True if has permission.</returns>
        Task<bool> HasPermission(string emailAddress, ILocationBasedDocument document, Permission permission);

        /// <summary>
        /// Has Permission
        /// </summary>
        /// <param name="emailAddress">Email Address.</param>
        /// <param name="permission">Permission.</param>
        /// <returns>True if has permission.</returns>
        Task<bool> HasPermission(string emailAddress, Permission permission);

        /// <summary>
        /// Location Ids with Permission
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="permission">Permission.</param>
        /// <returns>List of Location Ids that have permission.</returns>
        IEnumerable<string> LocationIdsWithPermission(MeUser user, Permission permission);
    }
}
