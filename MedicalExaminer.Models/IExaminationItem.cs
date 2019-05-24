using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public interface IExaminationItem
    {
        /// <summary>
        ///     Examination Id
        /// </summary>
        string ExaminationId { get; set; }

        /// <summary>
        ///     Where the death occured
        /// </summary>
        string PlaceDeathOccured { get; set; }

        /// <summary>
        ///     Medical Examiner Office Responsible for dealing with the examination
        /// </summary>
        string MedicalExaminerOfficeResponsible { get; set; }

        /// <summary>
        ///     Patients surname
        /// </summary>
        string Surname { get; set; }

        /// <summary>
        ///     Patients given names
        /// </summary>
        string GivenNames { get; set; }

        /// <summary>
        ///     Gender patient identifies as
        /// </summary>
        ExaminationGender? Gender { get; set; }

        /// <summary>
        ///     Comments regarding the patients gender identification
        /// </summary>
        string GenderDetails { get; set; }

        /// <summary>
        ///     Patients NHS Number
        /// </summary>
        string NhsNumber { get; set; }

        /// <summary>
        ///     Patients first hospital number
        /// </summary>
        string HospitalNumber_1 { get; set; }

        /// <summary>
        ///     Patients second hospital number
        /// </summary>
        string HospitalNumber_2 { get; set; }

        /// <summary>
        ///     Patients third hospital number
        /// </summary>
        string HospitalNumber_3 { get; set; }

        /// <summary>
        ///     Patients date of birth
        /// </summary>
        DateTime? DateOfBirth { get; set; }

        /// <summary>
        ///     Patients date of death
        /// </summary>
        DateTime? DateOfDeath { get; set; }

        /// <summary>
        ///     Patients time of death
        /// </summary>
        TimeSpan? TimeOfDeath { get; set; }

    }
}