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
        /// Have Unknown Basic Details (Name, DOB, DOD and NHS Number)
        /// </summary>
        public bool HaveUnknownBasicDetails { get; set; }

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
        /// Awaiting Additional Details
        /// </summary>
        public bool PendingAdditionalDetails { get; set; }

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

        /// <summary>
        /// Main Status flag: Basic Details Entered
        /// </summary>
        public StatusBarResult BasicDetailsEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Patient Name Entered
        /// </summary>
        public StatusBarResult NameEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: DOB Entered
        /// </summary>
        public StatusBarResult DobEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: DOD Entered
        /// </summary>
        public StatusBarResult DodEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Nhs Number Entered
        /// </summary>
        public StatusBarResult NhsNumberEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Main Status flag: Additional Details Entered
        /// </summary>
        public StatusBarResult AdditionalDetailsEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Latest Admission Details Entered
        /// </summary>
        public StatusBarResult LatestAdmissionDetailsEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Doctor In Charge Entered
        /// </summary>
        public StatusBarResult DoctorInChargeEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Qap Entered
        /// </summary>
        public StatusBarResult QapEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Bereaved Info Entered
        /// </summary>
        public StatusBarResult BereavedInfoEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: ME Assigned
        /// </summary>
        public StatusBarResult MeAssigned { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Main Status flag: Is Scrutiny Completed
        /// </summary>
        public StatusBarResult IsScrutinyCompleted { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Pre-Scrutiny Event Entered
        /// </summary>
        public StatusBarResult PreScrutinyEventEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: QAP Discussion Event Entered
        /// </summary>
        public StatusBarResult QapDiscussionEventEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Bereaved Discussion Event Entered
        /// </summary>
        public StatusBarResult BereavedDiscussionEventEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: ME confirmation of scrutiny
        /// </summary>
        public StatusBarResult MeScrutinyConfirmed { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Main Status flag: Is Case Items Completed
        /// </summary>
        public StatusBarResult IsCaseItemsCompleted { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: MCCD Issued
        /// </summary>
        public StatusBarResult MccdIssued { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Cremation Form Info Entered
        /// </summary>
        public StatusBarResult CremationFormInfoEntered { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: GP Notified
        /// </summary>
        public StatusBarResult GpNotified { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Sent To Coroner
        /// </summary>
        public StatusBarResult SentToCoroner { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Status flag: Case Closed?
        /// </summary>
        public StatusBarResult CaseClosed { get; set; } = StatusBarResult.Incomplete;

        /// <summary>
        /// Case Outcome
        /// </summary>
        public CaseOutcomeSummary? CaseOutcome { get; set; }

    }
}
