using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class OutstandingCaseItems
    {
        /// <summary>
        /// MCCD Issued?
        /// </summary>
        [JsonProperty(PropertyName = "mccd_issued")]
        public bool? MccdIssued { get; set; }

        /// <summary>
        /// Cremation Form Status
        /// </summary>
        [JsonProperty(PropertyName = "cremation_form_status")]
        public CremationFormStatus? CremationFormStatus { get; set; }

        /// <summary>
        /// GP Notified Status
        /// </summary>
        [JsonProperty(PropertyName = "gp_notified_status")]
        public GPNotified? GpNotifiedStatus { get; set; }
    }
}
