using System;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    public class PatientCardItem
    {
        public int UrgencyScore { get; set; }

        public string GivenNames { get; set; }

        public string Surname { get; set; }

        public string NhsNumber { get; set; }

        public string id { get; set; }

        public TimeSpan? TimeOfDeath { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        public DateTime? AppointmentDate { get; set; }

        public TimeSpan? AppointmentTime { get; set; }

        public DateTime? LastAdmission { get; set; }

        public DateTime CaseCreatedDate { get; set; }
    }
}
