using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// Create User Service.
    /// </summary>
    public class CreateUserService : QueryHandler<CreateUserQuery, MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CreateUserService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">User Connection Settings.</param>
        public CreateUserService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<MeUser> Handle(CreateUserQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            // Make sure date time is same for both fields.
            var now = DateTime.Now;

            param.UserToCreate.UserId = Guid.NewGuid().ToString();
            param.UserToCreate.CreatedAt = now;
            param.UserToCreate.ModifiedAt = now;
            param.UserToCreate.CreatedBy = param.CurrentUser.UserId;
            param.UserToCreate.LastModifiedBy = param.CurrentUser.UserId;

            var result = await CreateItemAsync(param.UserToCreate);
            return result;
        }
    }
}