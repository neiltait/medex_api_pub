using MedicalExaminer.Models;

namespace MedicalExaminer.API.Authorization.ExaminationContext
{
    /// <summary>
    /// Examination Validation Context.
    /// </summary>
    /// <remarks>The context in which something regarding an examination is taking place.</remarks>
    public class ExaminationValidationContext
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationValidationContext"/>.
        /// </summary>
        /// <param name="examination">The Examination.</param>
        public ExaminationValidationContext(Examination examination)
        {
            Examination = examination;
        }

        /// <summary>
        /// The Examination as part of the context.
        /// </summary>
        public Examination Examination { get; }
    }
}
