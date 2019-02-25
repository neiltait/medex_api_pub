using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Helpers
{
    /// <summary>
    /// Authentication Settings
    /// </summary>
    public class AuthenticationSettings
    {
        /// <summary>
        /// Secret string used to generate keys
        /// </summary>
        public string Secret { get; set; }
    }
}
