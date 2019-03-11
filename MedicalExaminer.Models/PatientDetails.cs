using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class PatientDetails
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
       // [JsonProperty(PropertyName = "given_names")]
        public string GivenNames { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(250)]
 //       [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }
    }
}
