using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Models.v1.MedicalTeams
{
    /// <summary>
    /// Clinical Professional Item.
    /// </summary>
    public class ClinicalProfessionalItem
    {
        /// <summary>
        /// Name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Organisation.
        /// </summary>
        public string Organisation { get; set; }

        /// <summary>
        /// Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Notes.
        /// </summary>
        public string Notes { get; set; }
    }
}
