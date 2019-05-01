using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Extensions.Models
{
    /// <summary>
    /// Me User Extensions
    /// </summary>
    public static class MeUserExtensions
    {
        /// <summary>
        /// Role For Examination.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="examination">The examination.</param>
        /// <returns>The highest role they have for this examination.</returns>
        public static UserRoles? RoleForExamination(this MeUser user, Examination examination)
        {
            var locations = examination.LocationIds();

            var permissions = user.Permissions.Where(p => locations.Contains(p.LocationId)).ToList();

            if (permissions.Any())
            {
                var topPermission = permissions.OrderByDescending(p => p.UserRole).First();

                return (UserRoles)topPermission.UserRole;
            }

            return null;
        }
    }
}
