using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MedicalExaminer.API.Attributes
{
    public class ValidNhsNumberNullAllowedAttribute : ValidationAttribute
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

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            //TODO:
            return ValidationResult.Success;
            if (!(value is string nhsNumber))
            {
                return ValidationResult.Success;
            }
            
            nhsNumber = nhsNumber.Replace(" ", string.Empty);
            nhsNumber = nhsNumber.Replace("-", string.Empty);
            if (nhsNumber.Length != 10)
            {
                return new ValidationResult("Invalid NHS Number");
            }

            if (!ValidateNhsNumber(nhsNumber))
            {
                return new ValidationResult("Invalid NHS Number");
            }

            return ValidationResult.Success;
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
