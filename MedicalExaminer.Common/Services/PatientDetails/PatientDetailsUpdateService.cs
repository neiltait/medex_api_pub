using System;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.PatientDetails;

namespace MedicalExaminer.Common.Services.PatientDetails
{
    public class PatientDetailsUpdateService : IAsyncQueryHandler<PatientDetailsUpdateQuery, Models.Examination>
    {
        private readonly IDatabaseAccess _databaseAccess;
        private readonly IExaminationConnectionSettings _connectionSettings;

        public PatientDetailsUpdateService(IDatabaseAccess databaseAccess,
            IExaminationConnectionSettings connectionSettings)
        {
            _databaseAccess = databaseAccess;
            _connectionSettings = connectionSettings;
        }
        public async Task<Models.Examination> Handle(PatientDetailsUpdateQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var caseToReplace = await
                _databaseAccess
                    .GetItemAsync<Models.Examination>(_connectionSettings, examination => examination.Id == param.CaseId);

            var patientDetails = param.PatientDetails;

            caseToReplace.GivenNames = patientDetails.GivenNames;
            caseToReplace.Surname = patientDetails.Surname;
            caseToReplace.Country = patientDetails.Country;
            caseToReplace.Postcode = patientDetails.PostCode;
            caseToReplace.HouseNameNumber = patientDetails.HouseNameNumber;
            caseToReplace.Street = patientDetails.Street;
            caseToReplace.Town = patientDetails.Town;
            caseToReplace.County = patientDetails.County;
            caseToReplace.LastOccupation = patientDetails.LastOccupation;
            caseToReplace.OrganisationCareBeforeDeathLocationId =
                patientDetails.OrganisationCareBeforeDeathLocationId;
            caseToReplace.FuneralDirectors = patientDetails.FuneralDirectors;
            caseToReplace.AnyPersonalEffects = patientDetails.AnyPersonalEffects;
            caseToReplace.PersonalEffectDetails = patientDetails.PersonalEffectDetails;
            caseToReplace.AnyImplants = patientDetails.AnyImplants;
            caseToReplace.ImplantDetails = patientDetails.ImplantDetails;
            caseToReplace.Representatives = patientDetails.Representatives;

            var result = await _databaseAccess.UpdateItemAsync(_connectionSettings, caseToReplace);
            return result;
        }
    }
}
