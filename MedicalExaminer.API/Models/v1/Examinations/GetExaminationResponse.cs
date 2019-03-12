using System;
using System.Collections.Generic;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    /// <inheritdoc />
    /// <summary>
    /// Response object for a single examination.
    /// </summary>
    public class GetExaminationResponse : ResponseBase
    {
        public IEnumerable<Representative> Representatives { get; set; }
        public string id { get; set ; }
        public string MedicalExaminerOfficeResponsible { get; set; }
        public string GenderDetails { get; set; }
        public TimeSpan? TimeOfDeath { get; set; }
        public string HospitalNumber_1 { get; set; }
        public string HospitalNumber_2 { get; set; }
        public string HospitalNumber_3 { get; set; }
        public string GivenNames { get; set; }
        public string Surname { get; set; }
        public string NhsNumber { get; set; }
        public ExaminationGender Gender { get; set; }
        public string HouseNameNumber { get; set; }
        public string Street { get; set; }
        public string Town { get; set ; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string LastOccupation { get; set; }
        public string OrganisationCareBeforeDeathLocationId { get; set; }
        
        public ModeOfDisposal ModeOfDisposal { get; set; }
        public string FuneralDirectors { get; set; }
        public bool AnyPersonalEffects { get; set; }
        public string PersonalEffectDetails { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTimeOffset DateOfDeath { get; set; }
        public bool FaithPriority { get; set; }
        public bool ChildPriority { get; set; }
        public bool CoronerPriority { get; set; }
        public bool OtherPriority { get; set; }
        public string PriorityDetails { get; set; }
        public bool Completed { get; set; }
        public CoronerStatus CoronerStatus { get; set; }
    }
}
