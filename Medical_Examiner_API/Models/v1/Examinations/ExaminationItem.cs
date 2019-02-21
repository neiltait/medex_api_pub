using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Models.V1.Examinations
{
    public class ExaminationItem
    {
        /// <summary>
        /// The Examination Identifier.
        /// </summary>
        public string ExaminationId { get; set; }

        /// <summary>
        /// The Full Name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The NHS Number.
        /// </summary>
        public string NHSNumber { get; set; }
    }
}
