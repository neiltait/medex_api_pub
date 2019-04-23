using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class CaseOutcome
    {
        /// <summary>
        /// Whether the MCCD is Issued.
        /// </summary>
        [JsonProperty(PropertyName = "mccd_issued")]
        public bool MccdIssued { get; set; }

        /// <summary>
        /// Whether the Cremation form is completed.
        /// </summary>
        [JsonProperty(PropertyName = "cremation_form_status")]
        public CremationFormStatus CremationFormStatus { get; set; }

        /// <summary>
        /// Whether the GP is notified.
        /// </summary>
        [JsonProperty(PropertyName = "gp_notified")]
        public GPNotified GpNotified { get; set; }
    }
}
