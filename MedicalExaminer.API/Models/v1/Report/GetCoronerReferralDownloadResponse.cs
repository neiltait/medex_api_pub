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
        /// the patients surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// the patients nhs number
        /// </summary>
        public string NhsNumber { get; set; }
    }
}
