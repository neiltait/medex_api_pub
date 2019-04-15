using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.PatientDetails
{
    public class GetPatientDetailsHeaderResponse : ResponseBase
    {
        public PatientCardItem Header { get; set; }
    }
}
