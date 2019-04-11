using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// Users Retrieval by Role Location Query Service.
    /// </summary>
    public class UsersRetrievalByRoleLocationQueryService : QueryHandler<UsersRetrievalByRoleLocationQuery, IEnumerable<MeUser>>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UsersRetrievalByRoleLocationQueryService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public UsersRetrievalByRoleLocationQueryService(
            IDatabaseAccess databaseAccess,
            IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override Task<IEnumerable<MeUser>> Handle(UsersRetrievalByRoleLocationQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            Expression<Func<MeUser, bool>> predicate = mu =>
                mu.Permissions.Any(p => param.Locations.Contains(p.LocationId) && p.UserRole == (int) param.Role);

            return GetItemsAsync(predicate);
        }
    }
}