using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Queries.Permissions
{
    public class InvalidUserPermissionQuery : IQuery<bool>
    {
        public string InvalidId { get; private set; }

        public InvalidUserPermissionQuery()
        {
            InvalidId = null;
        }
    }
}
