using System;
using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.PatientInformants
{
    public class GetInformantsResponse
    {
        public IEnumerable<PatientInformant> Informants { get; set; }
    }
}
