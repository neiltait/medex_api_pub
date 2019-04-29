using System;

namespace MedicalExaminer.API.Authorization.ExaminationContext
{
    /// <summary>
    /// Examination Validation Context Provider.
    /// </summary>
    public class ExaminationValidationContextProvider
    {
        private ExaminationValidationContext _context;

        /// <summary>
        /// Current Context.
        /// </summary>
        public ExaminationValidationContext Current
        {
            get
            {
                if (_context == null)
                {
                    throw new InvalidOperationException("The examination validation context has not been initialised.");
                }

                return _context;
            }
        }

        /// <summary>
        /// Set.
        /// </summary>
        /// <param name="context">The context to set.</param>
        public void Set(ExaminationValidationContext context)
        {
            if (_context != null)
            {
                throw new InvalidOperationException("Examination validation context has already been set.");
            }

            _context = context;
        }
    }
}
