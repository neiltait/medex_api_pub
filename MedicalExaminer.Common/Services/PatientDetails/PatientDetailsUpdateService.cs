using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.PatientDetails;

namespace MedicalExaminer.Common.Services.PatientDetails
{
    public class PatientDetailsUpdateService : IAsyncQueryHandler<PatientDetailsUpdateQuery, Models.Examination>
    {
        private readonly IExaminationConnectionSettings connectionSettings;
        private readonly IDatabaseAccess databaseAccess;

        private readonly IMapper mapper;

        public PatientDetailsUpdateService(
            IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings, 
            IMapper mapper)
        {
            this.databaseAccess = databaseAccess;
            this.connectionSettings = connectionSettings;
            this.mapper = mapper;
        }

        public async Task<Models.Examination> Handle(PatientDetailsUpdateQuery param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param));

            var caseToReplace = await
                databaseAccess
                    .GetItemAsync<Models.Examination>(connectionSettings,
                        examination => examination.ExaminationId == param.CaseId);

            mapper.Map(param.PatientDetails, caseToReplace);

            var result = await databaseAccess.UpdateItemAsync(connectionSettings, caseToReplace);
            return result;
        }
    }
}