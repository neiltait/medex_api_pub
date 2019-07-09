using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.Queries.Permissions;

namespace MedicalExaminer.Common.Services.Permissions
{
    public class InvalidUserPermissionUpdateService : IAsyncQueryHandler<InvalidUserPermissionQuery, bool>
    {
        public Task<bool> Handle(InvalidUserPermissionQuery param)
        {
            throw new NotImplementedException();
        }
    }
}
