namespace MedicalExaminer.Common.Queries.PatientDetails
{
    public class PatientDetailsByCaseIdQuery : IQuery<Models.PatientDetails>
    {
        public readonly string ExaminationId;

        public PatientDetailsByCaseIdQuery(string examinationId)
        {
            ExaminationId = examinationId;
        }
    }
}
