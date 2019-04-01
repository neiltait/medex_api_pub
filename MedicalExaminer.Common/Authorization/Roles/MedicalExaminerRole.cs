using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization.Roles
{
    /// <summary>
    /// Medical Examiner Role.
    /// </summary>
    public class MedicalExaminerRole : Role
    {
        /// <summary>
        /// Initialise a new instance of <see cref="MedicalExaminerRole"/>.
        /// </summary>
        public MedicalExaminerRole()
            : base(UserRoles.MedicalExaminer)
        {
            Grant(Permission.GetLocations);
            Grant(Permission.GetLocation);
        }
    }
}
