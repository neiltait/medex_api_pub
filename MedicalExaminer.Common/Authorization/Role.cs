using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Authorization
{
    public abstract class Role
    {
        public UserRoles UserRole { get; }

        private readonly HashSet<Permission> _granted = new HashSet<Permission>();

        public Role(UserRoles userRole)
        {
            UserRole = userRole;
        }

        public void Grant(Permission feature)
        {
            _granted.Add(feature);
        }

        public bool Can(Permission feature)
        {
            return _granted.Contains(feature);
        }
    }
}
