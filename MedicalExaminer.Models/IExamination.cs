using System;
using System.Collections.Generic;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models
{
    public interface IExamination
    {
        string HospitalNumber_1 { get; set; }
        string HospitalNumber_2 { get; set; }
        string HospitalNumber_3 { get; set; }
        string MedicalExaminerOfficeResponsible { get; set; }
        string GenderDetails { get; set; }
        string Id { get; set; }
        TimeSpan? TimeOfDeath { get; set; }
        string GivenNames { get; set; }
        string Surname { get; set; }
        string NhsNumber { get; set; }
        ExaminationGender Gender { get; set; }
        string HouseNameNumber { get; set; }
        string Street { get; set; }
        string Town { get; set; }
        string County { get; set; }
        string Postcode { get; set; }
        string Country { get; set; }
        string LastOccupation { get; set; }
        string OrganisationCareBeforeDeathLocationId { get; set; }
        //string DeathOccuredLocationId { get; set; }
        ModeOfDisposal ModeOfDisposal { get; set; }
        string FuneralDirectors { get; set; }
        bool AnyPersonalEffects { get; set; }
        string PersonalEffectDetails { get; set; }
        string PlaceDeathOccured { get; set; }
        DateTime DateOfBirth { get; set; }
        DateTime DateOfDeath { get; set; }
        bool CulturalPriority { get; set; }
        bool FaithPriority { get; set; }
        bool ChildPriority { get; set; }
        bool CoronerPriority { get; set; }
        bool OtherPriority { get; set; }
        string PriorityDetails { get; set; }
        bool Completed { get; set; }
        CoronerStatus CoronerStatus { get; set; }
        bool AnyImplants { get; set; }
        string ImplantDetails { get; set; }
        IEnumerable<Representative> Representatives { get; set; }
        bool OutOfHours { get; set; }
    }
}