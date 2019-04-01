using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace MedicalExaminer.API.Authorization
{
    public class AuthorizePermissionAttribute : AuthorizeAttribute
    {
        public AuthorizePermissionAttribute(Permission permission)
            : base($"HasPermission={permission.ToString()}")
        {
        }
    }
}
