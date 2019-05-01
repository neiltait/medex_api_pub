using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.ToolBox.Common.Dtos.Generate
{
    public class GenerateConfiguration
    {
        public int NumberOfRegions { get; set; } = 1;
        public int NumberOfTrusts { get; set; } = 1;
        public int NumberOfSites { get; set; } = 1;

        public int NumberOfMedicalExaminersPerSite { get; set; } = 1;
        public int NumberOfMedicalExaminerOfficersPerSite { get; set; } = 1;

        public int NumberOfMedicalExaminersPerTrust { get; set; } = 0;
        public int NumberOfMedicalExaminerOfficersPerTrust { get; set; } = 0;

        public int NumberOfMedicalExaminersPerRegion { get; set; } = 0;
        public int NumberOfMedicalExaminerOfficersPerRegion { get; set; } = 0;
        public int NumberOfServiceAdministratorsPerRegion { get; set; } = 1;

        public int NumberOfServiceOwnersPerNational { get; set; } = 1;
    }
}
