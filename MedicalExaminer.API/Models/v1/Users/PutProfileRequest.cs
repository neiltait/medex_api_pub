using System.ComponentModel.DataAnnotations;
using MedicalExaminer.API.Attributes;

namespace MedicalExaminer.API.Models.v1.Users
{
    /// <summary>
    /// Put User Request.
    /// </summary>
    public class PutProfileRequest
    {
        /// <summary>
        /// The User's email address.
        /// </summary>
        [Required]
        [GmcNumber]
        public string GmcNumber { get; set; }
    }
}