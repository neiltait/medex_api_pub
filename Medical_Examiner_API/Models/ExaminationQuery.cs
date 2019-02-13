using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Models
{
    public class ExaminationQuery : Query
    {
        public DateTime DateOfDeath { get; set; }
        public int Priority { get; set; }
        public string CaseType { get; set; }

        public ExaminationQuery(int page = 0, int pagesize = 30, string filter = "", string sort = "") : base(page, pagesize, filter, sort)
        { }
    }
}
