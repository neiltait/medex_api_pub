using System;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    /// <summary>
    /// class for displaying the examinations on the home page
    /// </summary>
    public class PatientCardItem
    {
        /// <summary>
        /// Urgency/Rapid Release score
        /// </summary>
        public int UrgencyScore { get; set; }

        /// <summary>
        /// patients given names
        /// </summary>
        public string GivenNames { get; set; }

        /// <summary>
        /// patients surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// patients nhs number, if known
        /// </summary>
        public string NhsNumber { get; set; }

        /// <summary>
        /// examination id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// patients time of death
        /// </summary>
        public TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        /// patients date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// patients date of death
        /// </summary>
        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        /// relatives next appointment time, null if there are non,
        /// or all the appointments were in the past
        /// </summary>
        public DateTime? AppointmentDate { get; set; }

        /// <summary>
        /// relatives next appointment time, null if there are non,
        /// or all the appointments were in the past
        /// </summary>
        public TimeSpan? AppointmentTime { get; set; }

        /// <summary>
        /// patients last admission date
        /// </summary>
        public DateTime? LastAdmission { get; set; }

        /// <summary>
        /// date the case was created
        /// </summary>
        public DateTime CaseCreatedDate { get; set; }


        public bool AdmissionNotesHaveBeenAdded { get; set; }

        public bool ReadyForMEScrutiny { get; set; }

        public bool Assigned { get; set; }

        public bool HaveBeenScrutinisedByME { get; set; }

        public bool PendingAdmissionNotes { get; set; }

        public bool PendingDiscussionWithQAP { get; set; }

        public bool PendingDiscussionWithRepresentative { get; set; }

        public bool HaveFinalCaseOutstandingOutcomes { get; set; }

        public string CaseOfficer { get; set; }

    }
}
