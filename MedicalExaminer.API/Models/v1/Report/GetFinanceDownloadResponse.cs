using System.Collections.Generic;
using MedicalExaminer.API.Models.v1.Examinations;

namespace MedicalExaminer.API.Models.v1.Report
{
    public class GetFinanceDownloadResponse
    {
        public List<ExaminationFinanceItem> Data { get; set; }
    }
}
