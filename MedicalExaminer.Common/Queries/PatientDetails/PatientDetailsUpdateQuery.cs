namespace MedicalExaminer.Common.Queries.PatientDetails
{
    public class PatientDetailsUpdateQuery : IQuery<Models.Examination>
    {
        public PatientDetailsUpdateQuery(string caseId, Models.PatientDetails patientDetails)
        {
            CaseId = caseId;
            PatientDetails = patientDetails;
        }

        public string CaseId { get; }
        public Models.PatientDetails PatientDetails { get; }
    }
}