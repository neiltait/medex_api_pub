namespace MedicalExaminer.Common.Queries.PatientDetails
{
    public class PatientDetailsUpdateQuery : IQuery<Models.Examination>
    {
        public PatientDetailsUpdateQuery(string caseId, Models.PatientDetails patientDetails, string userId)
        {
            CaseId = caseId;
            PatientDetails = patientDetails;
            UserId = userId;
        }

        public string CaseId { get; }

        public string UserId { get; }

        public Models.PatientDetails PatientDetails { get; }
    }
}