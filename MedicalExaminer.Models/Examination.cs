using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace MedicalExaminer.Models
{
    public class Examination : Record,  IExamination, ILocationPath
    {
        /// <summary>
        /// Pre calculated Sort orders for the next N-days.
        /// </summary>
        /// <remarks>Configured by property in UrgencySettings</remarks>
        [JsonProperty(PropertyName = "urgency_sort")]
        public Dictionary<string, int> UrgencySort { get; set; }

        /// <summary>
        /// Patients first hospital number.
        /// </summary>
        [JsonProperty(PropertyName = "hospital_number_1")]
        public string HospitalNumber_1 { get; set; }

        /// <summary>
        /// Patients second hospital number.
        /// </summary>
        [JsonProperty(PropertyName = "hospital_number_2")]
        public string HospitalNumber_2 { get; set; }

        /// <summary>
        /// Patients third hospital number.
        /// </summary>
        [JsonProperty(PropertyName = "hospital_number_3")]
        public string HospitalNumber_3 { get; set; }

        /// <summary>
        /// time of death
        /// </summary>
        [JsonProperty(PropertyName = "time_of_death")]
        [Required]
        public TimeSpan? TimeOfDeath { get; set; }

        /// <summary>
        /// Given Names (first names).
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
        [JsonProperty(PropertyName = "given_names")]
        public string GivenNames { get; set; }

        /// <summary>
        /// surname / last name.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
        [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }

        /// <summary>
        /// the patients NHS number.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [StringLength(10)]
        [JsonProperty(PropertyName = "nhs_number")]
        public string NhsNumber { get; set; }

        /// <summary>
        /// The patients gender.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "gender")]
        public ExaminationGender Gender { get; set; }

        /// <summary>
        /// patients house name or number.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "house_name_number")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string HouseNameNumber { get; set; }

        /// <summary>
        /// patients street.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "street")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string Street { get; set; }

        /// <summary>
        /// patients town.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "town")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string Town { get; set; }

        /// <summary>
        /// patients county.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "county")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string County { get; set; }

        /// <summary>
        /// patients postcode.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "postcode")]
        [DataType(DataType.Text)]
        [StringLength(12)]
        public string Postcode { get; set; }

        /// <summary>
        /// ID of MEO user who will be working on the scrutiny
        /// </summary>
        [JsonProperty(PropertyName = "gender_details")]
        [DataType(DataType.Text)]
        public string GenderDetails { get; set; }

        /// <summary>
        /// patients country.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "country")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string Country { get; set; }

        /// <summary>
        /// patients last occupation.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "last_occupation")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string LastOccupation { get; set; }

        /// <summary>
        /// the location ID of the NHS organisation to administer care before the patient died.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "organisation_care_before_death_location_id")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string OrganisationCareBeforeDeathLocationId { get; set; }

        /// <summary>
        /// The mode of disposal - e.g. buried / cremation.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "mode_of_disposal")]
        [DataType(DataType.Text)]
        [StringLength(250)]
        public ModeOfDisposal ModeOfDisposal { get; set; }

        /// <summary>
        /// The name of the funeral directors.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "funderal_directors")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string FuneralDirectors { get; set; }

        /// <summary>
        /// Have any personal effects been collected from the patient?.
        /// </summary>
        // Personal affects
        [Required]
        [JsonProperty(PropertyName = "personal_effects_collected")]
        public bool AnyPersonalEffects { get; set; }

        /// <summary>
        /// If there have been personal effects collected from the patient, provide some details.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "personal_effects_details")]
        public string PersonalEffectDetails { get; set; }

        /// <summary>
        /// Where did the patient die?.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "place_death_occured")]
        public string PlaceDeathOccured { get; set; }

        /// <summary>
        /// The date of birth of the patient.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [JsonProperty(PropertyName = "date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// date of death of the patient.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        [JsonProperty(PropertyName = "last_admission")]
        public DateTime? LastAdmission { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [JsonProperty(PropertyName = "date_of_death")]
        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        /// The medical team associated with the patient
        /// </summary>
        [Required]
        [DataType(DataType.Custom)]
        [JsonProperty(PropertyName = "medical_team")]
        public MedicalTeam MedicalTeam { get; set; } = new MedicalTeam();

        /// <summary>
        /// Cultural priority flag
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "cultural_priority")]
        public bool CulturalPriority { get; set; }

        /// <summary>
        /// is a Faith priority flag
        /// </summary>
        // Flags that effect priority
        [Required]
        [JsonProperty(PropertyName = "faith_priority")]
        public bool FaithPriority { get; set; }

        /// <summary>
        /// is a Child priority flag
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "child_priority")]
        public bool ChildPriority { get; set; }

        /// <summary>
        /// is a Coroner priority flag
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "coroner_priority")]
        public bool CoronerPriority { get; set; }

        /// <summary>
        /// is a other priority flag
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "other_priority")]
        public bool OtherPriority { get; set; }

        /// <summary>
        /// details about the other priority selected.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "priority_details")]
        [DataType(DataType.Text)]
        public string PriorityDetails { get; set; }

        /// <summary>
        /// Not sure about this one, case closed?.
        /// </summary>
        // Status Fields
        [Required]
        [JsonProperty(PropertyName = "case_completed")]
        public bool CaseCompleted { get; set; }

        /// <summary>
        /// Coroner status, updated with interaction with coroner
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "coroner_status")]
        public CoronerStatus CoronerStatus { get; set; }

        /// <summary>
        /// Does the patient have any implants that may effect mode of disposal?.
        /// </summary>
        [JsonProperty(PropertyName = "any_implants")]
        public bool? AnyImplants { get; set; }

        /// <summary>
        /// Implant details if has implants.
        /// </summary>
        [JsonProperty(PropertyName = "implant_details")]
        [DataType(DataType.Text)]
        public string ImplantDetails { get; set; }

        /// <summary>
        /// ID of MEO user who will be working on the scrutiny
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "medical_examiner_office_responsible")]
        [DataType(DataType.Text)]
        public string MedicalExaminerOfficeResponsible { get; set; }

        /// <summary>
        /// ID of MEO user who will be working on the scrutiny
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "medical_examiner_office_responsible_name")]
        [DataType(DataType.Text)]
        public string MedicalExaminerOfficeResponsibleName { get; set; }

        /// <summary>
        /// enumerable of patients representatives
        /// </summary>
        [JsonProperty(PropertyName = "representatives")]
        public IEnumerable<Representative> Representatives { get; set; }

        /// <summary>
        /// Case break down events
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "case_break_down")]
        public CaseBreakDown CaseBreakdown { get; set; } = new CaseBreakDown();

        /// <summary>
        /// Have Unknown Basic Details (Name, DOB, DOD and NHS Number)
        /// </summary>
        [JsonProperty(PropertyName = "have_unknown_basic_details")]
        public bool HaveUnknownBasicDetails { get; set; } = true;

        /// <summary>
        /// Have the admission notes been added
        /// </summary>
        [JsonProperty(PropertyName = "admission_notes_have_been_added")]
        public bool AdmissionNotesHaveBeenAdded { get; set; } = false;

        /// <summary>
        /// is the examination ready for medical examiner scrutiny
        /// </summary>
        [JsonProperty(PropertyName = "ready_for_me_scrutiny")]
        public bool ReadyForMEScrutiny { get; set; } = false;

        /// <summary>
        /// is the case currently unassigned
        /// </summary>
        [JsonProperty(PropertyName = "unassigned")]
        public bool Unassigned { get; set; } = true;

        /// <summary>
        /// has the case been scrutinised by the medical examiner
        /// </summary>
        [JsonProperty(PropertyName = "have_been_scrutinised_by_me")]
        public bool HaveBeenScrutinisedByME { get; set; } = false;

        /// <summary>
        /// have admission notes been added for the examination
        /// </summary>
        [JsonProperty(PropertyName = "pending_admission_notes")]
        public bool PendingAdmissionNotes { get; set; } = true;

        /// <summary>
        /// Pending Additional Details
        /// </summary>
        [JsonProperty(PropertyName = "pending_additional_details")]
        public bool PendingAdditionalDetails { get; set; } = true;

        /// <summary>
        /// has the qap discussion occured
        /// </summary>
        [JsonProperty(PropertyName = "pending_discussion_with_qap")]
        public bool PendingDiscussionWithQAP { get; set; } = true;

        /// <summary>
        /// has the discussion with the representative
        /// </summary>
        [JsonProperty(PropertyName = "pending_discussion_with_representative")]
        public bool PendingDiscussionWithRepresentative { get; set; } = true;

        /// <summary>
        /// has the ME added scrutiny notes
        /// </summary>
        [JsonProperty(PropertyName = "pending_scrutiny_notes")]
        public bool PendingScrutinyNotes { get; set; } = true;

        /// <summary>
        /// have the final case outcomes been determined
        /// </summary>
        [JsonProperty(PropertyName = "have_final_case_outcomes_outstanding")]
        public bool HaveFinalCaseOutcomesOutstanding { get; set; } = true;

        /// <summary>
        /// the unique identifier for the examination
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string ExaminationId { get; set; }

        /// <summary>
        /// the national location id
        /// </summary>
        [JsonProperty(PropertyName = "national_location_id")]
        public string NationalLocationId { get; set; }

        /// <summary>
        /// the regional location id
        /// </summary>
        [JsonProperty(PropertyName = "region_location_id")]
        public string RegionLocationId { get; set; }

        /// <summary>
        /// the trusts location id
        /// </summary>
        [JsonProperty(PropertyName = "trust_location_id")]
        public string TrustLocationId { get; set; }

        /// <summary>
        /// the sites location id
        /// </summary>
        [JsonProperty(PropertyName = "site_location_id")]
        public string SiteLocationId { get; set; }

        /// <summary>
        /// Confirmation Of Scrutiny Completed At
        /// </summary>
        [JsonProperty(PropertyName = "confirmation_of_scrutiny_completed_at")]
        public DateTime? ConfirmationOfScrutinyCompletedAt { get; set; }

        /// <summary>
        /// Confirmation Of Scrutiny Completed By
        /// </summary>
        [JsonProperty(PropertyName = "confirmation_of_scrutiny_completed_by")]
        public string ConfirmationOfScrutinyCompletedBy { get; set; }

        /// <summary>
        /// has the coroners referral been sent?
        /// </summary>
        [JsonProperty(PropertyName = "coroner_referral_sent")]
        public bool CoronerReferralSent { get; set; } = false;

        /// <summary>
        /// has the Scrutiny been confirmed?
        /// </summary>
        [JsonProperty(PropertyName = "scrutiny_confirmed")]
        public bool ScrutinyConfirmed { get; set; } = false;

        /// <summary>
        /// have the outstanding case items been completed?
        /// </summary>
        [JsonProperty(PropertyName = "outstanding_case_items_completed")]
        public bool OutstandingCaseItemsCompleted { get; set; } = false;

        /// <summary>
        /// should the cremation fee be waived?
        /// </summary>
        [JsonProperty(PropertyName = "waive_fee")]
        public bool? WaiveFee { get; set; } = null;

        /// <summary>
        /// Date Case Closed
        /// </summary>
        [JsonProperty(PropertyName = "date_case_closed")]
        public DateTime? DateCaseClosed { get; set; } = null;

        /// <summary>
        /// Case Outcome Items
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "case_outcome")]
        public CaseOutcome CaseOutcome { get; set; } = new CaseOutcome();
    }
}