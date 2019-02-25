using System.ComponentModel.DataAnnotations;

namespace Medical_Examiner_API.Models
{
    public class Representative : Contact
    {
        [Required] public bool PresentAtDeath { get; set; }

        [Required] public bool InformedOfDeath { get; set; }
    }
}