using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Common.Queries.Examination;

namespace MedicalExaminer.Common.Services.Examination
{
    public class PatientCardsRetrievalService
    {
        public async Task Handle(PatientCardsRetrievalQuery patientCardQuery)
        {
            if (patientCardQuery == null)
            {
                throw new ArgumentNullException(nameof(patientCardQuery));
            }
        }
    }
}
