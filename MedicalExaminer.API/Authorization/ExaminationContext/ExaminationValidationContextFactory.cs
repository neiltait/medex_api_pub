using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Authorization.ExaminationContext
{
    /// <summary>
    /// Examination Validation Context Factory.
    /// </summary>
    public class ExaminationValidationContextFactory
    {
        /// <summary>
        /// Create a new Examination Validation Context.
        /// </summary>
        /// <param name="examination">Examination to pass into the context.</param>
        /// <returns><see cref="ExaminationValidationContext"/>.</returns>
        public Task<ExaminationValidationContext> Create(Examination examination)
        {
            return Task.FromResult(
                new ExaminationValidationContext(examination));
        }
    }
}
