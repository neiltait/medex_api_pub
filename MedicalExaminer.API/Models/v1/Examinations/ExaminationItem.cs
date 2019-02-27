using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    public class ExaminationItem
    {
        public string ExaminationId { get; set; }
        public string PlaceDeathOccured { get; set; }
        public string MedicalExaminerOfficeResponsible { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public ExaminationGender? Gender { get; set; }
        public string NhsNumber { get; set; }
        public bool NhsNumberKnown { get; set; }
        public string HospitalNumber_1 { get; set; }
        public string HospitalNumber_2 { get; set; }
        public string HospitalNumber_3 { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool DateOfBirthKnown { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public bool DateOfDeathKnown { get; set; }
        public TimeSpan? TimeOfDeath { get; set; }
        public bool TimeOfDeathKnown { get; set; }
    }
}
