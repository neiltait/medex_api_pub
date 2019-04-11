using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Extensions.MeUser
{
    public static class MeUserExtensions
    {
        /// <summary>
        /// Get the full name, combining last name and first name
        /// </summary>
        /// <param name="meUser">User object.</param>
        /// <returns>Full name string.</returns>
        public static string FullName(this Models.MeUser meUser)
        {
            // TODO: Do we have a standard way of doing this? Can we assume this format? Does it need to be last name first?
            return $"{meUser.FirstName} {meUser.LastName}";
        }
    }
}
