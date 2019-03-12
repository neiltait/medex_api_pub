using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.v1.PatientDetails
{
    public class PutPatientDetailsResponse : ResponseBase
    {
        /// <summary>
        /// The id of the examination
        /// </summary>
        public string ExaminationId { get; set; }
    }
}
