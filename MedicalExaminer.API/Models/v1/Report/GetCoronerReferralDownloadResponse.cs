using System;

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

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string PlaceOfDeath { get; set; }

        public DateTime DateOfDeath { get; set; }

        public TimeSpan TimeOfDeath { get; set; }

        public string AnyImplants { get; set; }

        public string BereavedName { get; set; }

        public string BereavedPhoneNumber { get; set; }
    }
}
