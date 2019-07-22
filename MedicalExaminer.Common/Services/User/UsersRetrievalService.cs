using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// Users Retrieval Service.
    /// </summary>
    public class UsersRetrievalService : QueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UsersRetrievalService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public UsersRetrievalService(IDatabaseAccess databaseAccess, IUserConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<MeUser>> Handle(UsersRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            IEnumerable<MeUser> result;

            if (param.ForLookup)
            {
                // Need to include permissions to ensure lookups can be filtered.
                result = await GetItemsAsync<MeUser>(x => true, user => new
                {
                    id = user.UserId,
                    first_name = user.FirstName,
                    last_name = user.LastName,
                    permissions = user.Permissions,
                });
            }
            else
            {
                result = await GetItemsAsync<MeUser>(x => true);
            }

            return result;
        }
    }
}