using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common
{
    public interface IPermissionPersistence
    {
        Task<Permission> UpdatePermissionAsync(Permission permission);

        Task<Permission> CreatePermissionAsync(Permission permission);

        Task<Permission> GetPermissionAsync(string meUserId, string permissionId);

        Task<IEnumerable<Permission>> GetPermissionsAsync(IEnumerable<string> locations, string meUserId);

        Task<IEnumerable<Permission>> GetPermissionsAsync(string meUserId);
    }
}