using MedicalExaminer.API.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.v1.Report
{
    public class GetFinanceDownloadRequest
    {
        public DateTime ExaminationsCreatedFrom { get; set; }
        public DateTime ExaminationsCreatedTo { get; set; }

        [ValidMedicalExaminerOfficeNullAllowed]
        public string LocationId { get; set; }
    }
}
