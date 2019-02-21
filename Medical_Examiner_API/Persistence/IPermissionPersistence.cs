using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;

namespace Medical_Examiner_API.Persistence
{
    public interface IPermissionPersistence
    {
        Task<Permission> UpdatePermissionAsync(Permission permission);
        Task<Permission> CreatePermissionAsync(Permission permission);
        Task<Permission> GetPermissionAsync(string meUserId, string permissionId);
        Task<IEnumerable<Permission>> GetPermissionsAsync(string userId);
    }
}