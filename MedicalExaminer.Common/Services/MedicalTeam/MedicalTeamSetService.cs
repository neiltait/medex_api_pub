using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Services.MedicalTeam 
{
    //public class MedicalTeamSetService : IAsyncQueryHandler<ExaminationMedicalTeamPostQuery, IExamination>
    //{
    //    private readonly IDatabaseAccess _databaseAccess;
    //    private readonly IConnectionSettings _connectionSettings;

    //    public MedicalTeamSetService(IDatabaseAccess databaseAccess, IExaminationConnectionSettings connectionSettings)
    //    {
    //        _databaseAccess = databaseAccess;
    //        _connectionSettings = connectionSettings;
    //    }

    //    public Task<IExamination> Handle(IExamination examination)
    //    {
    //        if (examination == null)
    //        {
    //            throw new ArgumentNullException(nameof(examination));
    //        }
    //        // can put whatever filters in the param, just empty for now
    //        return _databaseAccess.Update(_connectionSettings, examination);
    //    }
    //}
}
