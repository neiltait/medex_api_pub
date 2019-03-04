using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.Validators
{
    public class NhsNumberString
    {
        public string Value { get; }
        public NhsNumberString(string value)
        {
            Value = value;
        }
    }
}
