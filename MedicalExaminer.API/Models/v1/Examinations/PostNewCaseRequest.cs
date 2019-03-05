using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.API.Models.Validators;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    public class PostNewCaseRequest
    {
        /// <summary>
        ///  Where the death occured
        /// </summary>
        public string PlaceDeathOccured { get; set; }

        /// <summary>
        /// Medical Examiner Office Responsible for dealing with the examination
        /// </summary>
        public string MedicalExaminerOfficeResponsible { get; set; }

        /// <summary>
        /// Patients surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Patients given names
        /// </summary>
        public string GivenNames { get; set; }

        /// <summary>
        /// Gender patient identifies as
        /// </summary>
        public ExaminationGender? Gender { get; set; }

        /// <summary>
        /// Comments regarding the patients gender identification
        /// </summary>
        public string GenderDetails { get; set; }

        /// <summary>
        /// Patients NHS Number
        /// </summary>
        public string NhsNumber { get; set; }

        /// <summary>
        /// Is the patients NHS Number known
        /// </summary>
        public bool NhsNumberKnown { get; set; }

        /// <summary>
        /// Patients first hospital number
        /// </summary>
        public string HospitalNumber_1 { get; set; }

        /// <summary>
        /// Patients second hospital number
        /// </summary>
        public string HospitalNumber_2 { get; set; }

        /// <summary>
        /// Patients third hospital number
        /// </summary>
        public string HospitalNumber_3 { get; set; }

        /// <summary>
        /// Patients date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Is the patients date of birth known
        /// </summary>
        public bool DateOfBirthKnown { get; set; }

        /// <summary>
        /// Patients date of death
        /// </summary>
        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        /// Is the patients date of death known
        /// </summary>
        public bool DateOfDeathKnown { get; set; }

        /// <summary>
        /// Patients time of death
        /// </summary>
        public TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        /// Is the patients time of death known
        /// </summary>
        public bool TimeOfDeathKnown { get; set; }

        /// <summary>
        /// Out of hours/urgent scrutiny has taken place out of hours
        /// </summary>
        public bool OutOfHours { get; set; }
    }
}
