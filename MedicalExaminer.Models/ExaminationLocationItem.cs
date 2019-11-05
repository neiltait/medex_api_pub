using System.Collections.Generic;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    public class ExaminationLocationItem
    {
        public Examination Examination { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public IEnumerable<MeUser> Users { get; set; }
    }
}
