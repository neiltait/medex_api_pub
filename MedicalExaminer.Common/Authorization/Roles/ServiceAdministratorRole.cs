using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization.Roles
{
    /// <summary>
    /// Service Administrator Role.
    /// </summary>
    public class ServiceAdministratorRole : Role
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ServiceAdministratorRole"/>.
        /// </summary>
        public ServiceAdministratorRole()
            : base(UserRoles.ServiceAdministrator)
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

                Permission.GetProfile,
                Permission.UpdateProfile);
        }
    }
}
