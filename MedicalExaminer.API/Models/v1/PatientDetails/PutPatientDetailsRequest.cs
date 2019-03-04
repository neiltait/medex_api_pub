using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.API.Models.v1.PatientDetails
{
    public class PutPatientDetailsRequest
    {
        /// <summary>
        /// Patients Postcode
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// First line of patients addess
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Second line of patients address
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Patients town or city
        /// </summary>
        public string AddressCity { get; set; }

        /// <summary>
        /// Patients county
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// Patients country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Free text for any relevant patient occupation details
        /// </summary>
        public string RelevantOccupationDetails { get; set; }

        /// <summary>
        /// Organisation responsible for patient at time of death
        /// </summary>
        public string OrganisationResponsibleAtTimeOfDeath { get; set; }

        /// <summary>
        /// Patients funeral arrangements
        /// </summary>
        public ModeOfDisposal FuneralArrangements { get; set; }

        /// <summary>
        /// Does the patient have any implants that may impact on cremation
        /// </summary>
        public bool AnyImplants { get; set; }

        /// <summary>
        /// Free text for the implant details
        /// </summary>
        public string ImplantDetails { get; set; }

        /// <summary>
        /// Details of the patients funeral directors
        /// </summary>
        public string FuneralDirectors { get; set; }

        /// <summary>
        /// Does the patient have any personal effects
        /// </summary>
        public bool AnyPersonalEffects { get; set; }

        /// <summary>
        /// Free text details of any personal effects
        /// </summary>
        public string PersonalEffectDetails { get; set; }
    }
}
