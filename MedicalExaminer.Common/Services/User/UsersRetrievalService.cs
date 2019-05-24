using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.User
{
    /// <summary>
    /// Users Retrieval Service.
    /// </summary>
    public class UsersRetrievalService : QueryHandler<UsersRetrievalQuery, IEnumerable<Models.MeUser>>
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
        public override async Task<IEnumerable<Models.MeUser>> Handle(UsersRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            // can put whatever filters in the param, just empty for now
            var result = await GetItemsAsync<Models.MeUser>(x => true);

            return result;
        }
    }
}