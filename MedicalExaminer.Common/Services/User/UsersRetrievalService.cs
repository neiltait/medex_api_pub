using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;

namespace MedicalExaminer.Common.Services.Examination
{
    public class UsersRetrievalService : IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<Models.MeUser>>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public UsersRetrievalService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public Task<IEnumerable<Models.MeUser>> Handle(UsersRetrievalQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            // can put whatever filters in the param, just empty for now
            return _databaseAccess.GetItemsAsync<Models.MeUser>(_connectionSettings, x => true);
        }
    }
}