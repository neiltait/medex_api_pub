using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.Validators
{
    public interface IValidationRule
    {
        ValidationError Error { get; }
    }
}
