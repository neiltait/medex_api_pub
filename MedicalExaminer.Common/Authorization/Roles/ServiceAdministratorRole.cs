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
            Grant(Permission.GetLocations);
            Grant(Permission.GetLocation);
        }
    }
}
