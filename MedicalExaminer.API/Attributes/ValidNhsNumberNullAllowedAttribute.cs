using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MedicalExaminer.API.Attributes
{
    public class ValidNhsNumberNullAllowedAttribute : ValidationAttribute
    {
        private readonly int[] factors = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
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
            int[] seq;
            try
            {
                seq = nhsNumber.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
            }
            catch
            {
                return false;
            }

            var sumNhsFactor = seq[0] * factors[0] + seq[1] * factors[1] + seq[2] * factors[2] + seq[3] * factors[3]
                               + seq[4] * factors[4] + seq[5] * factors[5] + seq[6] * factors[6] + seq[7] * factors[7]
                               + seq[8] * factors[8];

            var remainder = sumNhsFactor % 11;
            var check = 11 - remainder;

            if (check == 11)
            {
                check = 0;
            }

            return check == seq[9];
        }
    }
}