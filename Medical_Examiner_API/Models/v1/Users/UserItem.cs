using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Models.v1.Users
{
    /// <summary>
    /// A user item as part of multiple user responses.
    /// </summary>
    public class UserItem
    {
        /// <summary>
        /// The User identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The User's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The User's email address
        /// </summary>
        public string Email { get; set; }
    }
}
