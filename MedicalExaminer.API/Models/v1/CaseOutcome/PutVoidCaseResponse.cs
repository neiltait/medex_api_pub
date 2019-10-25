using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.API.Models.v1.Examinations;

namespace MedicalExaminer.API.Models.v1.CaseOutcome
{
    /// <summary>
    /// Response for void case
    /// </summary>
    public class PutVoidCaseResponse : ResponseBase
    {
        /// <summary>
        /// Patient card header
        /// </summary>
        public PatientCardItem Header { get; set; }

        /// <summary>
        /// Should the cremation fee be waived?
        /// </summary>
        public DateTime? VoidedDate { get; set; }
    }
}
