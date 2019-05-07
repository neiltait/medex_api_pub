using System.Collections.Generic;
using System.Linq;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Extensions.MeUser
{
    /// <summary>
    /// Me User Extensions
    /// </summary>
    public static class MeUserExtensions
    {
        /// <summary>
        /// Role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The highest role they have in the system.</returns>
        public static UserRoles? Role(this MedicalExaminer.Models.MeUser user)
        {
            var permissions = user.Permissions;

            var topPermission = permissions?.OrderByDescending(p => p.UserRole).FirstOrDefault();

            return topPermission?.UserRole;
        }

        /// <summary>
        /// Has Role for Examination.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="examination">The Examination.</param>
        /// <param name="role">The Role required.</param>
        /// <returns>True if user has role.</returns>
        public static bool HasRoleForExamination(
            this MedicalExaminer.Models.MeUser user,
            Examination examination,
            UserRoles role)
        {
            var locations = examination.LocationIds();

            var permissions = user.Permissions.Where(p => locations.Contains(p.LocationId)).ToList();

            return permissions.Any(p => p.UserRole == role);
        }

        /// <summary>
        /// Role For Examination.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="examination">The examination.</param>
        /// <returns>The highest role they have for this examination.</returns>
        public static UserRoles? RoleForExamination(this MedicalExaminer.Models.MeUser user, Examination examination)
        {
            var locations = examination.LocationIds();

            var permissions = user.Permissions.Where(p => locations.Contains(p.LocationId)).ToList();

            if (permissions.Any())
            {
                var topPermission = permissions.OrderByDescending(p => p.UserRole).First();

                return topPermission.UserRole;
            }

            return null;
        }

        /// <summary>
        /// Get the users examination role part two.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="requiredRoles">List of required roles to filter by.</param>
        /// <returns><see cref="UserRoles"/>.</returns>
        public static UserRoles? UsersExaminationRole(
            this MedicalExaminer.Models.MeUser user,
            IEnumerable<UserRoles> requiredRoles)
        {
            if (requiredRoles == null)
            {
                return null;
            }

            if (user?.Permissions != null)
            {
                foreach (var role in requiredRoles)
                {
                    if (user.Permissions.Any(x => x.UserRole == role))
                    {
                        return role;
                    }
                }
            }

            return null;
        }


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
