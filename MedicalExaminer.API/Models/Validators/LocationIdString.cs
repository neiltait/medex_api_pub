using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.Validators
{
    public class LocationIdString
    {
        public string Value { get; }
        public LocationIdString(string value)
        {
            Value = value;
        }
    }
}
