using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using Microsoft.Azure.Documents.SystemFunctions;

namespace MedicalExaminer.Common.Services.Examination
{
    public class ExaminationMigrationService : QueryHandler<NullQuery, bool>
    {
        public ExaminationMigrationService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings) : base(databaseAccess, connectionSettings)
        {
        }

        public async override Task<bool> Handle(NullQuery param)
        {
            var examinations = await DatabaseAccess.GetItemsAsync<Models.Examination>(ConnectionSettings, x => x.Version < 1 || !x.Version.IsDefined());
            foreach (var examination in examinations)
            {
                examination.Version = 1;
                await DatabaseAccess.UpdateItemAsync(ConnectionSettings, examination);
            }

            return true;
        }
    }
}
