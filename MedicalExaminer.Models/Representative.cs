using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.Models
{
    public class Representative : Contact
    {
        [Required] public bool PresentAtDeath { get; set; }

        [Required] public bool InformedOfDeath { get; set; }
    }
}