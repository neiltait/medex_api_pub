using MedicalExaminer.Models.Enums;
using System;

namespace MedicalExaminer.API.Models.v1.Examinations
{
    public class ExaminationFinanceItem
    {
        public string ExaminationId { get; set; }
        public string SiteName { get; set; }
        public string TrustName { get; set; }
        public string RegionName { get; set; }
        public string NationalName { get; set; }
        public string MedicalExaminerId { get; set; }
        public DateTime CaseCreated { get; set; }
        public DateTime? CaseClosed { get; set; }
        public bool HasNhsNumber { get; set; }
        public ModeOfDisposal ModeOfDisposal { get; set; }
        public bool? WaiverFee { get; set; }
    }
}
