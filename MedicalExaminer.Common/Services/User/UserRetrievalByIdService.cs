using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// User Retrieval By Id Service.
    /// </summary>
    public class UserRetrievalByIdService : QueryHandler<UserRetrievalByIdQuery, Models.MeUser>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserRetrievalByIdService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public UserRetrievalByIdService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override Task<Models.MeUser> Handle(UserRetrievalByIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var result = GetItemByIdAsync(param.UserId);
            return result;
        }
    }
}
