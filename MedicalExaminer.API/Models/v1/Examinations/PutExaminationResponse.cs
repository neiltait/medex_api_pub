using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    public class PutExaminationResponse : ResponseBase
    {
        public Link ExaminationLink { get; set; }
    }
}
