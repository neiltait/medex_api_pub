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
            Grant(Permission.GetLocations);
            Grant(Permission.GetLocation);
        }
    }
}
