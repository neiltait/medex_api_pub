using MedicalExaminer.API.Models.v1.Examinations;

namespace MedicalExaminer.API.Models.v1.PatientDetails
{
    /// <summary>
    ///     The response returned when a request to PUT the patients details
    /// </summary>
    public class PutPatientDetailsResponse : ResponseBaseEnumErrors
    {
        /// <summary>
        ///     The id of the examination
        /// </summary>
        public PatientCardItem Header { get; set; }
    }
}