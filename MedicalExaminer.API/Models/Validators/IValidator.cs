using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.Validators
{
    public interface IValidator<TEntity>
    {
        /// <summary>
        /// Validates the given evaluationItem.
        /// </summary>
        /// <param name="evaluationItem">Entity to validate</param>
        /// <returns>Collection of <see cref="ValidationError">ValidationErrors</see>.</returns>
        Task<IEnumerable<ValidationError>> ValidateAsync(TEntity evaluationItem);
    }
}
