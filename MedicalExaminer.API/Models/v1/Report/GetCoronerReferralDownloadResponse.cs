using MedicalExaminer.Models;
using System;
using System.Collections.Generic;

namespace MedicalExaminer.API.Models.v1.Report
{
    /// <summary>
    /// the getcoronerreferraldownloadresponse
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
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// the patients gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// the patients address
        /// </summary>
        public string AddressAndPostcode { get; set; }

        /// <summary>
        /// the place where the patient became extinct
        /// </summary>
        public string PlaceOfDeath { get; set; }

        /// <summary>
        /// the date the patient died
        /// </summary>
        public DateTime DateOfDeath { get; set; }

        /// <summary>
        /// the time the patient died
        /// </summary>
        public TimeSpan TimeOfDeath { get; set; }

        /// <summary>
        /// does the patient have any implants
        /// </summary>
        public bool AnyImplants { get; set; }

        /// <summary>
        /// details of the patients implants
        /// </summary>
        public string ImplantDetails { get; set; }

        /// <summary>
        ///     Details of any representatives
        /// </summary>
        public IEnumerable<Representative> Representatives { get; set; }

    }
}
