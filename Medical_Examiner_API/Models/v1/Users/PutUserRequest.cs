using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Models.v1.Users
{
    public class PutUserRequest
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
        [EmailAddress]
        public string Email { get; set; }
    }
}
