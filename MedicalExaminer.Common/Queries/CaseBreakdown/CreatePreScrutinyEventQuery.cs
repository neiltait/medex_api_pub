using MedicalExaminer.Models;

namespace MedicalExaminer.Common.Queries.CaseBreakdown
{
    public class CreatePreScrutinyEventQuery : IQuery<string>
    {
        public PreScrutinyEvent PreScrutinyEvent { get; }

        public string CaseId { get; set; }

        public CreatePreScrutinyEventQuery(string caseId, PreScrutinyEvent preScrutinyEvent)
        {
            CaseId = caseId;
            PreScrutinyEvent = preScrutinyEvent;
        }
    }
}
