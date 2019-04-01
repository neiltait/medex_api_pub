using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.CaseBreakdown;

namespace MedicalExaminer.Common.Services.CaseBreakdown
{
    public class CreatePreScrutinyEventService : IAsyncQueryHandler<CreatePreScrutinyEventQuery, string>
    {
        private readonly IConnectionSettings _connectionSettings;
        private readonly IDatabaseAccess _databaseAccess;

        public CreatePreScrutinyEventService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }

        public async Task<string> Handle(CreatePreScrutinyEventQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            param.PreScrutinyEvent.EventId = Guid.NewGuid().ToString();
            var result = await _databaseAccess.CreateItemAsync(_connectionSettings, param.PreScrutinyEvent, false);
            return result.EventId;
        }
    }
}
