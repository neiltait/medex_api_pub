using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalExaminer.ToolBox.MVC.Models
{
    public class ImpersonateModel
    {
        public string Email { get; set; }
        public string MeUsedId { get; set; }
        public IEnumerable<SelectListItem> UserOptions { get; set; }
    }
}
