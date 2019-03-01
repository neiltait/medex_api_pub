using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.Validators
{
    /// <summary>
    /// Validates a string input against the nhs validation
    /// </summary>
    public class NhsNumberValidator : IValidator<NhsNumberString>
    {
        private readonly int[] _factors =
        {
            10,
            9,
            8,
            7,
            6,
            5,
            4,
            3,
            2
        };

        public async Task<IEnumerable<ValidationError>> ValidateAsync(NhsNumberString nhsNumberString)
        {
            var nhsNumber = nhsNumberString.Value;
            var errors = new List<ValidationError>();
            nhsNumber = nhsNumber.Replace(" ", string.Empty);
            nhsNumber = nhsNumber.Replace("-", string.Empty);
            if (nhsNumber.Length != 10)
            {
                errors.Add(new ValidationError(ValidationErrorCode.Invalid, "NhsNumber", "Incorrect NHS Number"));
                return errors;
            }

            if (!ValidateNhsNumber(nhsNumber))
            {
                errors.Add(new ValidationError(ValidationErrorCode.Invalid, "NhsNumber", "Incorrect NHS Number"));
            }

            return errors;
        }

        private bool ValidateNhsNumber(string nhsNumber)
        {
            int[] numericNhsNumber;
            try
            {
                numericNhsNumber = nhsNumber.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
            }
            catch
            {
                return false;
            }

            var sumNhsFactor = (numericNhsNumber[0] * _factors[0])
                               + (numericNhsNumber[1] * _factors[1])
                               + (numericNhsNumber[2] * _factors[2])
                               + (numericNhsNumber[3] * _factors[3])
                               + (numericNhsNumber[4] * _factors[4])
                               + (numericNhsNumber[5] * _factors[5])
                               + (numericNhsNumber[6] * _factors[6])
                               + (numericNhsNumber[7] * _factors[7])
                               + (numericNhsNumber[8] * _factors[8]);

            var remainder = sumNhsFactor % 11;
            var check = 11 - remainder;

            if (check == 11)
            {
                check = 0;
            }

            return check == numericNhsNumber[9];
        }
    }
}
