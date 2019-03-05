using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.API.Models.Validators;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    public class PutExaminationResponse : ResponseBase
    {
        public string ExaminationId { get; set; }
    }
}
