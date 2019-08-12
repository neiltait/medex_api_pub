using System;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.Report
{
    /// <summary>
    /// the GetCoronerReferralDownloadResponse
    /// </summary>
    public class GetCoronerReferralDownloadResponse
    {
        /// <summary>
        /// The patients given name
        /// </summary>
        public string GivenNames { get; set; }

        /// <summary>
        /// The patients surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// The patients nhs number
        /// </summary>
        public string NhsNumber { get; set; }

        /// <summary>
        /// Are you able to issue a MCCD? Y/N
        /// </summary>
        public bool AbleToIssueMCCD { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1a
        /// </summary>
        public string CauseOfDeath1a { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1b
        /// </summary>
        public string CauseOfDeath1b { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 1c
        /// </summary>
        public string CauseOfDeath1c { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Medical Examiner 2
        /// </summary>
        public string CauseOfDeath2 { get; set; }

        /// <summary>
        /// the patients date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// the patients gender
        /// </summary>
        public ExaminationGender Gender { get; set; }

        /// <summary>
        /// the patients address
        /// </summary>
        public string HouseNameNumber { get; set; }

        /// <summary>
        /// the street where the patient resides
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// the town where the patient resides
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// the contry wherethe patient resides
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// the patients postcode
        /// </summary>
        public string Postcode { get; set; }


        /// <summary>
        /// the place where the patient became extinct
        /// </summary>
        public string PlaceOfDeath { get; set; }

        /// <summary>
        /// the date the patient died
        /// </summary>
        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        /// the time the patient died
        /// </summary>
        public TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        /// does the patient have any implants
        /// </summary>
        public bool? AnyImplants { get; set; }

        /// <summary>
        /// details of the patients implants
        /// </summary>
        public string ImplantDetails { get; set; }

        /// <summary>
        ///     Bereaved Discussion Event
        /// </summary>
        public BereavedDiscussionEventItem LatestBereavedDiscussion { get; set; }

        /// <summary>
        /// The clinical professional that performed the QAP Event
        /// </summary>
        public ClinicalProfessionalItem Qap { get; set; }

        /// <summary>
        /// the first consultant from the medical team
        /// </summary>
        public ClinicalProfessionalItem Consultant { get; set; }

        /// <summary>
        /// the GP from the medical team
        /// </summary>
        public ClinicalProfessionalItem GP { get; set; }

        /// <summary>
        /// the most recent admission details from the case breakdown
        /// </summary>
        public AdmissionEventItem LatestAdmissionDetails { get; set; }

        /// <summary>
        /// The medical history information taken from the latest medical history event on the case breakdown
        /// </summary>
        public string DetailsAboutMedicalHistory { get; set; }
    }
}
