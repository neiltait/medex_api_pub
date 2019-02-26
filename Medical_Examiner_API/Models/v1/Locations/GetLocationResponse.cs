using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Models.V1.Locations
{
    public class GetLocationResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
