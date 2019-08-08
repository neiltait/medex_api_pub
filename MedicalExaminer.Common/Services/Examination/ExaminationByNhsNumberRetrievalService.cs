using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationByNhsNumberRetrievalService : QueryHandler<ExaminationByNhsNumberRetrievalQuery, Models.Examination>
    {
        public ExaminationByNhsNumberRetrievalService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings) : base(databaseAccess, connectionSettings)
        {
        }

        public override Task<Models.Examination> Handle(ExaminationByNhsNumberRetrievalQuery param)
        {
            return DatabaseAccess.GetItemAsync<Models.Examination>(ConnectionSettings, x => x.NhsNumber == param.NhsNumber);
        }
    }
}
