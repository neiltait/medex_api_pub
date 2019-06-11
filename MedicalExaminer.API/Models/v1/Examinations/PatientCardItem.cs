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
        public string ExaminationId { get; set; }

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

        /// <summary>
        /// Has the admission notes event been finalised
        /// </summary>
        public bool AdmissionNotesHaveBeenAdded { get; set; }

        /// <summary>
        /// Is the case ready for ME Scrutiny
        /// </summary>
        public bool ReadyForMEScrutiny { get; set; }

        /// <summary>
        /// The case does not have anyone assigned to it
        /// </summary>
        public bool Unassigned { get; set; }

        /// <summary>
        /// The case has been scrutinised by the ME
        /// </summary>
        public bool HaveBeenScrutinisedByME { get; set; }

        /// <summary>
        /// Awaiting Admission Notes
        /// </summary>
        public bool PendingAdmissionNotes { get; set; }

        /// <summary>
        /// Awaiting discussion with the QAP
        /// </summary>
        public bool PendingDiscussionWithQAP { get; set; }

        /// <summary>
        /// The case is awaiting discussion with the representative
        /// </summary>
        public bool PendingDiscussionWithRepresentative { get; set; }

        /// <summary>
        /// The case is awaiting scrutiny notes from the ME
        /// </summary>
        public bool PendingScrutinyNotes { get; set; }

        /// <summary>
        /// Awaiting the final outcome of the case
        /// </summary>
        public bool HaveFinalCaseOutcomesOutstanding { get; set; }

    }
}
