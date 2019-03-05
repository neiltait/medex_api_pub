using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models.V1.Locations
{
    public class GetLocationResponse
    {
        public string LocationId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string ParentId { get; set; }

        public bool IsActive { get; set; }

        public LocationType Type {get; set;}
    }
}
