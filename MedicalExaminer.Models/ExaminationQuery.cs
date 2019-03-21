using System;

namespace MedicalExaminer.Models
{
    public class ExaminationQuery : Query
    {
        public ExaminationQuery(int page = 0, int pagesize = 30, string filter = "", string sort = "")
            : base(page, pagesize, filter, sort)
        {
        }

        public DateTime DateOfDeath { get; set; }

        public int Priority { get; set; }

        public string CaseType { get; set; }
    }
}