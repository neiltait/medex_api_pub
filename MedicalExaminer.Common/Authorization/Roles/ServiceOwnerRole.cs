using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization.Roles
{
    /// <summary>
    /// Service Owner Role.
    /// </summary>
    public class ServiceOwnerRole : Role
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ServiceOwnerRole"/>.
        /// </summary>
        public ServiceOwnerRole()
            : base(UserRoles.ServiceOwner)
        {
            Grant(
                Permission.GetUsers,
                Permission.GetUser,
                Permission.InviteUser,
                Permission.SuspendUser,
                Permission.EnableUser,
                Permission.DeleteUser,
                Permission.GetUserPermissions,
                Permission.GetUserPermission,
                Permission.CreateUserPermission,
                Permission.UpdateUserPermission,
                Permission.DeleteUserPermission,

                Permission.GetLocations,
                Permission.GetLocation,

                Permission.GetExaminations,
                Permission.GetExamination,

                Permission.GetProfile,
                Permission.UpdateProfile);
        }
    }
}
