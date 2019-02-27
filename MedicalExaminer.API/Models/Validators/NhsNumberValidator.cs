using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.Validators
{
    public class NhsNumberValidator : IValidator<string>
    {
        private int[] _factors =
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
        public async Task<IList<ValidationError>> ValidateAsync(string nhsNumber)
        {
            var errors = new List<ValidationError>();
            nhsNumber = nhsNumber.Replace(" ", "");
            if (nhsNumber.Length != 10)
            {
                errors.Add(new ValidationError(ValidationErrorCode.Invalid, "Incorrect NHS Number"));
                return errors;
            }

            if (!ValidateNhsNumber(nhsNumber))
            {
                errors.Add(new ValidationError(ValidationErrorCode.Invalid, "Incorrect NHS Number"));
            }
            return errors;
        }

        private bool ValidateNhsNumber(string nhsNumber)
        {
            int[] numericNhsNumber = null; 
            try
            {
                numericNhsNumber = nhsNumber.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
            }
            catch (Exception ex)
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

            if (check == numericNhsNumber[9])
            {
                return true;
            }
            
            return false;
        }
    }
}
