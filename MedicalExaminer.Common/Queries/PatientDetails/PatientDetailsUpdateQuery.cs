using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.PatientDetails
{
    public class PatientDetailsUpdateQuery : IQuery<Models.Examination>
    {
        public PatientDetailsUpdateQuery(string caseId, Models.PatientDetails patientDetails, MeUser user)
        {
            CaseId = caseId;
            PatientDetails = patientDetails;
            User = user;
        }

        public string CaseId { get; }

        public MeUser User { get; }

        public Models.PatientDetails PatientDetails { get; }
    }
}