using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    public class PostClinicalProfessional
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Organisation { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Notes { get; set; }
    }
}