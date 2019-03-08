using System;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    public class PutExaminationResponse : ResponseBase
    {
        public string ExaminationId { get; set; }
    }
}
