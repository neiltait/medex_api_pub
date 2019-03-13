namespace MedicalExaminer.API.Models.v1.PatientDetails
{
    /// <summary>
    /// The response returned when a request to PUT the patients details
    /// </summary>
    public class PutPatientDetailsResponse : ResponseBase
    {
        /// <summary>
        /// The id of the examination
        /// </summary>
        public string ExaminationId { get; set; }
    }
}
