using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.Validators
{
    public class Error
    {
        /// <summary>
        /// Initialises a new instance of the Error class.
        /// </summary>
        public Error()
        {
        }

        /// <summary>
        /// Initialises a new instance of the Error class.
        /// </summary>
        /// <param name="message">The user friendly error message.</param>
        public Error(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets a user friendly error message.
        /// </summary>
        public string Message { get; set; }
    }
}
