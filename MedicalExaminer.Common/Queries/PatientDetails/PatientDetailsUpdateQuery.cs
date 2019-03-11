using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Queries.PatientDetails
{
    public class PatientDetailsUpdateQuery : IQuery<Models.Examination>
    {
        public string CaseId { get; }
        public Models.PatientDetails PatientDetails { get; }

        public PatientDetailsUpdateQuery(string caseId, Models.PatientDetails patientDetails)
        {
            CaseId = caseId;
            PatientDetails = patientDetails;
        }
    }
}
