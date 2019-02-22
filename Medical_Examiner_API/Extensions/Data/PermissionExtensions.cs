using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Users;
using Microsoft.Azure.Documents;
using Permission = Medical_Examiner_API.Models.Permission;

namespace Medical_Examiner_API.Extensions.Data
{
    /// <summary>
    /// User Extensions
    /// </summary>
    public static class PermissionExtensions
    {
        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="UserItem"/>.
        /// </summary>
        /// <param name="permission">the Permission</param>
        /// <returns>A UserItem.</returns>
        public static PermissionItem ToPermissionItem(this Permission permission)
        {
            return new PermissionItem()
            {
                PermissionId = permission.PermissionId,
                UserId = permission.UserId,
                LocationId = permission.LocationId,
                UserRole = permission.UserRole,
            };
        }

        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="GetUserResponse"/>.
        /// </summary>
        /// <param name="permission">The Permission.</param>
        /// <returns>A GetUserResponse.</returns>
        public static GetPermissionResponse ToGetPermissionResponse(this Permission permission)
        {
            return new GetPermissionResponse
            {
                PermissionId = permission.PermissionId,
                UserId = permission.UserId,
                LocationId = permission.LocationId,
                UserRole = permission.UserRole,
            };
        }

        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="GetUserResponse"/>.
        /// </summary>
        /// <param name="permission">The Permission</param>
        /// <returns>A GetUserResponse.</returns>
        public static PutPermissionResponse ToPutPermissionResponse(this Permission permission)
        {
            return new PutPermissionResponse
            {
                PermissionId = permission.PermissionId,
                UserId = permission.UserId,
                LocationId = permission.LocationId,
                UserRole = permission.UserRole,
            };
        }

        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="PutUserResponse"/>.
        /// </summary>
        /// <param name="permission">The Permission</param>
        /// <returns>A GetUserResponse.</returns>
        public static PostPermissionResponse ToPostPermissionResponse(this Permission permission)
        {
            return new PostPermissionResponse
            {
                PermissionId = permission.PermissionId,
                UserId = permission.UserId,
                LocationId = permission.LocationId,
                UserRole = permission.UserRole,
            };
        }

        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="PutUserResponse"/>.
        /// </summary>
        /// <param name="permission">The Permission</param>
        /// <returns>A GetUserResponse.</returns>
        public static PostPermissionRequest ToPostPermissionRequest(this Permission permission)
        {
            return new PostPermissionRequest
            {
                UserId = permission.UserId,
                LocationId = permission.LocationId,
                UserRole = permission.UserRole,
            };
        }

        /// <summary>
        /// Convert a <see cref="PostUserRequest"/>  to a <see cref="MeUser"/>.
        /// </summary>
        /// <param name="postPermission">The postPermission</param>
        /// <returns>A Models.permisison.</returns>
        public static Permission ToPermission(this PostPermissionRequest postPermission)
        {
            return new Permission
            {
                UserId = postPermission.UserId,
                LocationId = postPermission.LocationId,
                UserRole = postPermission.UserRole,
            };
        }

        /// <summary>
        /// Convert a <see cref="PutUserRequest"/>  to a <see cref="MeUser"/>.
        /// </summary>
        /// <param name="putPermission">The putPermission</param>
        /// <returns>A Models.Permission.</returns>
        public static Permission ToPermission(this PutPermissionRequest putPermission)
        {
            return new Permission
            {
                PermissionId = putPermission.PermissionId,
                UserId = putPermission.UserId,
                LocationId = putPermission.LocationId,
                UserRole = putPermission.UserRole,
            };
        }
    }
}