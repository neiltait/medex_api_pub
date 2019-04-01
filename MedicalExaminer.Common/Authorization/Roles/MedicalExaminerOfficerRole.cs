using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization.Roles
{
    /// <summary>
    /// Medical Examiner Officer Role.
    /// </summary>
    public class MedicalExaminerOfficerRole : Role
    {
        /// <summary>
        /// Initialise a new instance of <see cref="MedicalExaminerOfficerRole"/>.
        /// </summary>
        public MedicalExaminerOfficerRole()
            : base(UserRoles.MedicalExaminerOfficer)
        {
            Grant(Permission.GetLocations);
            Grant(Permission.GetLocation);
        }
    }
}
