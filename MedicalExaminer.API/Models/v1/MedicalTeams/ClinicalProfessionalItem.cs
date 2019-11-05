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

        /// <summary>
        /// The clinical professionals GMC Number
        /// </summary>
        public string GMCNumber { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Clinical Professional 1a
        /// </summary>
        public string CauseOfDeath1a { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Clinical Professional 1b
        /// </summary>
        public string CauseOfDeath1b { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Clinical Professional 1c
        /// </summary>
        public string CauseOfDeath1c { get; set; }

        /// <summary>
        /// Possible cause of death established during scrutiny by Clinical Professional 2
        /// </summary>
        public string CauseOfDeath2 { get; set; }
    }
}
